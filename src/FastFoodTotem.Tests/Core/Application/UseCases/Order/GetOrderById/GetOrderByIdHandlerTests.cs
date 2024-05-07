using AutoMapper;
using FastFoodTotem.Application.UseCases.Order.GetOrderById;
using FastFoodTotem.Domain.Contracts.Repositories;
using FastFoodTotem.Domain.Entities;
using FastFoodTotem.Domain.Exceptions;
using Moq;

namespace FastFoodTotem.Tests.Core.Application.UseCases.Order.GetOrderById;

public class GetOrderByIdHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Order_Successfully()
    {
        // Arrange
        var orderId = 1;
        var orderEntity = new OrderEntity
        {
            Id = orderId,
            UserCpf = "12345678900",
            UserName = "John Doe",
            OrderedItems = new[] { new OrderedItemEntity { ProductId = 1, Amount = 2 } },
            CreationDate = DateTime.Now
        };
        var mapperr = new GetOrderByIdMapper();
        var getOrderByIdResponse = new GetOrderByIdResponse
        {
            Id = orderId,
            UserCpf = "12345678900",
            UserName = "John Doe"
        };
        var validator = new GetOrderByIdValidator();
        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<GetOrderByIdResponse>(orderEntity)).Returns(getOrderByIdResponse);

        var orderRepository = new Mock<IOrderRepository>();
        orderRepository.Setup(x => x.GetOrderAsync(orderId, CancellationToken.None)).ReturnsAsync(orderEntity);

        var handler = new GetOrderByIdHandler(mapper.Object, orderRepository.Object);

        // Act
        var result = await handler.Handle(new GetOrderByIdRequest(orderId), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.Id);
    }

    [Fact]
    public async Task Handle_Should_Throw_ObjectNotFoundException_When_Order_Not_Found()
    {
        // Arrange
        var orderId = 1;

        var mapper = new Mock<IMapper>();

        var orderRepository = new Mock<IOrderRepository>();
        orderRepository.Setup(x => x.GetOrderAsync(orderId, CancellationToken.None)).ReturnsAsync((OrderEntity)null);

        var handler = new GetOrderByIdHandler(mapper.Object, orderRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ObjectNotFoundException>(() => handler.Handle(new GetOrderByIdRequest(orderId), CancellationToken.None));
    }
}
