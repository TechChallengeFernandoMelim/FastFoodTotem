using AutoMapper;
using FastFoodTotem.Application.UseCases.Order.CreateOrder;
using FastFoodTotem.Domain.Contracts.Payments;
using FastFoodTotem.Domain.Contracts.Repositories;
using FastFoodTotem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TechTalk.SpecFlow;

namespace FastFoodTotem.Tests.BDD;

[Binding]
public class CreateOrderSteps
{
    private readonly Mock<IMapper> mapperMock = new Mock<IMapper>();
    private readonly Mock<IOrderRepository> orderRepositoryMock = new Mock<IOrderRepository>();
    private readonly Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
    private readonly Mock<IOrderPayment> orderPaymentMock = new Mock<IOrderPayment>();
    private readonly CreateOrderHandler handler;
    private CreateOrderRequest request;
    private CreateOrderResponse result;
    private OrderEntity orderEntity;
    private string[] orderPaymentResponse;

    public CreateOrderSteps()
    {
        handler = new CreateOrderHandler(mapperMock.Object, orderRepositoryMock.Object, productRepositoryMock.Object, orderPaymentMock.Object);
    }

    [Given(@"the user provides a payment access token ""(.*)""")]
    public void GivenTheUserProvidesAPaymentAccessToken(string paymentAccessToken)
    {
        request = new CreateOrderRequest
        {
            PaymentAccessToken = paymentAccessToken,
            OrderedItems = new List<OrderItens>()
        };
    }

    [Given(@"the following items are added to the order:")]
    public void GivenTheFollowingItemsAreAddedToTheOrder(Table table)
    {
        foreach (var row in table.Rows)
        {
            var productId = int.Parse(row["ProductId"]);
            var amount = int.Parse(row["Amount"]);
            request.OrderedItems.Add(new OrderItens { ProductId = productId, Amount = amount });
        }
    }

    [Given(@"the order details are mapped to an order entity")]
    public void GivenTheOrderDetailsAreMappedToAnOrderEntity()
    {
        orderEntity = new OrderEntity
        {
            Id = 1,
            UserCpf = "12345678900",
            UserName = "John Doe",
            OrderedItems = request.OrderedItems.Select(item => new OrderedItemEntity
            {
                ProductId = item.ProductId,
                Amount = item.Amount,
                Product = new ProductEntity { Id = item.ProductId, Price = 10 }
            })
        };

        mapperMock.Setup(x => x.Map<OrderEntity>(request)).Returns(orderEntity);
    }

    [Given(@"the product details are retrieved from the product repository")]
    public void GivenTheProductDetailsAreRetrievedFromTheProductRepository()
    {
        productRepositoryMock.Setup(x => x.GetProduct(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(new ProductEntity { Price = 10 });
    }

    [Given(@"the order is successfully added to the order repository")]
    public void GivenTheOrderIsSuccessfullyAddedToTheOrderRepository()
    {
        orderRepositoryMock.Setup(x => x.AddOrderAsync(orderEntity, CancellationToken.None)).Verifiable();
    }

    [When(@"the user attempts to create the order")]
    public async Task WhenTheUserAttemptsToCreateTheOrder()
    {
        orderPaymentResponse = new[] { "fakeQRData", "fakeOrderId" };
        orderPaymentMock.Setup(x => x.GerarQRCodeParaPagamentoDePedido(orderEntity, request.PaymentAccessToken)).ReturnsAsync(orderPaymentResponse);

        result = await handler.Handle(request, CancellationToken.None);
    }

    [Then(@"the system should generate a QR code for payment of the order")]
    public void ThenTheSystemShouldGenerateAQRCodeForPaymentOfTheOrder()
    {
        orderPaymentMock.Verify(x => x.GerarQRCodeParaPagamentoDePedido(orderEntity, request.PaymentAccessToken), Times.Once);
    }

    [Then(@"the system should return a response with order details")]
    public void ThenTheSystemShouldReturnAResponseWithOrderDetails()
    {
        Assert.NotNull(result);
        Assert.Equal("fakeQRData", result.PaymentQrCode);
    }
}
