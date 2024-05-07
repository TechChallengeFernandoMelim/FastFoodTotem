using FastFoodTotem.Domain.Entities;
using FastFoodTotem.MercadoPago.Dtos.Response;
using FastFoodTotem.MercadoPago;
using Moq;
using System.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FastFoodTotem.Tests.Infrastructure;

public class MercadoPagoPaymentTests
{
    //[Fact]
    //public async Task GerarQRCodeParaPagamentoDePedido_Success()
    //{
    //    // Arrange
    //    var orderEntity = new OrderEntity()
    //    {
    //        CreationDate = DateTime.Now,
    //        Id = 1,
    //        InStoreOrderId = "",
    //        UserCpf = "",
    //        UserName = "",
    //        OrderedItems = new List<OrderedItemEntity>()
    //    };
    //    Environment.SetEnvironmentVariable("PaymentServiceUrl", "http://teste.com");
    //    var accessToken = "fakeAccessToken";
    //    var configuration = new Mock<IConfiguration>();
    //    configuration.Setup(x => x[It.IsAny<string>()]).Returns("fakePaymentServiceUrl");
    //    var httpClientFactory = new Mock<IHttpClientFactory>();
    //    var request = new GerarQRCodeResponse { QrData = "fakeQRData", InStoreOrderId = "fakeOrderId" };
    //    var responseContent = JsonConvert.SerializeObject(request);
    //    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responseContent) };

    //    var httpClient = new HttpClient(new FakeHttpMessageHandler(httpResponseMessage));
    //    httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
    //    var mercadoPagoPayment = new MercadoPagoPayment(configuration.Object);

    //    // Act
    //    var result = await mercadoPagoPayment.GerarQRCodeParaPagamentoDePedido(orderEntity, accessToken);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.Equal("fakeQRData", result[0]);
    //    Assert.Equal("fakeOrderId", result[1]);
    //}

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

        // Act
        var result = await mercadoPagoPayment.GerarQRCodeParaPagamentoDePedido(orderEntity, accessToken);

        // Assert
        Assert.Null(result);
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

