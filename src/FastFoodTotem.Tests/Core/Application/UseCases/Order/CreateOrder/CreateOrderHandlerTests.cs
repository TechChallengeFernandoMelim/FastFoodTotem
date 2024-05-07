using AutoMapper;
using FastFoodTotem.Application.UseCases.Order.CreateOrder;
using FastFoodTotem.Domain.Contracts.Payments;
using FastFoodTotem.Domain.Contracts.Repositories;
using FastFoodTotem.Domain.Entities;
using FastFoodTotem.Domain.Exceptions;
using Moq;

namespace FastFoodTotem.Tests.Core.Application.UseCases.Order.CreateOrder;

public class CreateOrderHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Order_Successfully()
    {
        // Arrange
        var request = new CreateOrderRequest
        {
            PaymentAccessToken = "fakeAccessToken",
            OrderedItems = new List<OrderItens>
                {
                    new OrderItens { ProductId = 1, Amount = 2 },
                    new OrderItens { ProductId = 2, Amount = 1 }
                },
            // Add more request properties as needed
        };
        var orderEntity = new OrderEntity
        {
            Id = 1,
            UserCpf = "12345678900",
            UserName = "John Doe",
            OrderedItems = request.OrderedItems.Select(item => new OrderedItemEntity
            {
                ProductId = item.ProductId,
                Amount = item.Amount,
                Product = new ProductEntity { Id = item.ProductId, Price = 10 }
            }),
            // Add more order entity properties as needed
        };
        var orderPaymentResponse = new[] { "fakeQRData", "fakeOrderId" };
        var response = new CreateOrderResponse
        {
            Id = orderEntity.Id,
            PaymentQrCode = orderPaymentResponse[0],
            TotalPrice = orderEntity.GetTotal(),
            InStoreOrderId = orderPaymentResponse[1]
        };

        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<OrderEntity>(request)).Returns(orderEntity);

        var orderRepository = new Mock<IOrderRepository>();
        orderRepository.Setup(x => x.AddOrderAsync(orderEntity, CancellationToken.None)).Verifiable();
        orderRepository.Setup(x => x.EditOrderAsync(orderEntity, CancellationToken.None)).Verifiable();

        var productRepository = new Mock<IProductRepository>();
        productRepository.Setup(x => x.GetProduct(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(new ProductEntity { Price = 10 }).Verifiable();

        var orderPayment = new Mock<IOrderPayment>();
        orderPayment.Setup(x => x.GerarQRCodeParaPagamentoDePedido(orderEntity, request.PaymentAccessToken)).ReturnsAsync(orderPaymentResponse).Verifiable();

        var handler = new CreateOrderHandler(mapper.Object, orderRepository.Object, productRepository.Object, orderPayment.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Handle_Should_Throw_ObjectNotFoundException_When_Product_Not_Found()
    {
        // Arrange
        var request = new CreateOrderRequest
        {
            PaymentAccessToken = "fakeAccessToken",
            OrderedItems = new List<OrderItens>
                {
                    new OrderItens { ProductId = 1, Amount = 2 },
                    new OrderItens { ProductId = 2, Amount = 1 }
                },
            // Add more request properties as needed
        };
        var orderEntity = new OrderEntity
        {
            Id = 1,
            UserCpf = "12345678900",
            UserName = "John Doe",
            OrderedItems = request.OrderedItems.Select(item => new OrderedItemEntity
            {
                ProductId = item.ProductId,
                Amount = item.Amount,
                Product = new ProductEntity { Id = item.ProductId, Price = 10 }
            }),
            // Add more order entity properties as needed
        };

        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<OrderEntity>(request)).Returns(orderEntity);

        var orderRepository = new Mock<IOrderRepository>();

        var productRepository = new Mock<IProductRepository>();
        productRepository.Setup(x => x.GetProduct(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync((ProductEntity)null).Verifiable();

        var orderPayment = new Mock<IOrderPayment>();

        var handler = new CreateOrderHandler(mapper.Object, orderRepository.Object, productRepository.Object, orderPayment.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ObjectNotFoundException>(() => handler.Handle(request, CancellationToken.None));
    }
}
