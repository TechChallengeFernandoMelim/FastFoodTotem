using FastFoodTotem.Domain.Entities;
using FastFoodTotem.MercadoPago.Dtos.Response;
using FastFoodTotem.MercadoPago;
using Moq;
using System.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using FastFoodTotem.Application.UseCases.Order.GetOrderById;
using FastFoodTotem.Domain.Exceptions;

namespace FastFoodTotem.Tests.Infrastructure;

public class MercadoPagoPaymentTests
{
    [Fact]
    public async Task GerarQRCodeParaPagamentoDePedido_Failure()
    {
        // Arrange
        var orderEntity = new OrderEntity()
        {
            CreationDate = DateTime.Now,
            Id = 1,
            InStoreOrderId = "",
            UserCpf = "",
            UserName = "",
            OrderedItems = new List<OrderedItemEntity>()
        };
        Environment.SetEnvironmentVariable("PaymentServiceUrl", "http://teste.com");

        var accessToken = "fakeAccessToken";
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x[It.IsAny<string>()]).Returns("fakePaymentServiceUrl");
        var httpClientFactory = new Mock<IHttpClientFactory>();
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        var httpClient = new HttpClient(new FakeHttpMessageHandler(httpResponseMessage));
        httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        var mercadoPagoPayment = new MercadoPagoPayment(configuration.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await mercadoPagoPayment.GerarQRCodeParaPagamentoDePedido(orderEntity, accessToken));
    }
}

public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpResponseMessage _httpResponseMessage;

    public FakeHttpMessageHandler(HttpResponseMessage httpResponseMessage)
    {
        _httpResponseMessage = httpResponseMessage;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_httpResponseMessage);
    }
}

