using AutoMapper;
using FastFoodTotem.Application.UseCases.Order.GetAllOrders;
using FastFoodTotem.Domain.Contracts.Repositories;
using FastFoodTotem.Domain.Entities;
using Moq;

namespace FastFoodTotem.Tests.Core.Application.UseCases.Order.GetAllOrders;

public class GetAllOrdersHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_All_Orders_Successfully()
    {
        // Arrange
        var orderEntities = new List<OrderEntity>
        {
            new OrderEntity { Id = 1, UserCpf = "12345678900", UserName = "John Doe", OrderedItems = new List<OrderedItemEntity>(), CreationDate = DateTime.Now },
            new OrderEntity { Id = 2, UserCpf = "98765432100", UserName = "Jane Smith", OrderedItems = new List<OrderedItemEntity>(), CreationDate = DateTime.Now }
        };
        var mapperr = new GetAllOrdersMapper();
        var orders = new List<GetAllOrdersOrder>
        {
            new GetAllOrdersOrder { Id = 1, UserCpf = "12345678900", UserName = "John Doe"},
            new GetAllOrdersOrder { Id = 2, UserCpf = "98765432100", UserName = "Jane Smith"}
        };
        var getAllOrdersResponse = new GetAllOrdersResponse(orders);
        var validator = new GetAllOrdersValidators();
        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<IEnumerable<GetAllOrdersOrder>>(It.IsAny<IEnumerable<OrderEntity>>())).Returns(orders);

        var orderRepository = new Mock<IOrderRepository>();
        orderRepository.Setup(x => x.GetAllAsync(CancellationToken.None)).ReturnsAsync(orderEntities);

        var handler = new GetAllOrdersHandler(mapper.Object, orderRepository.Object);

        // Act
        var result = await handler.Handle(new GetAllOrdersRequest(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(getAllOrdersResponse.Orders.Count(), result.Orders.Count());
        // Add more assertions as needed
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Orders_Found()
    {
        // Arrange
        var orderEntities = new List<OrderEntity>();
        var orders = new List<GetAllOrdersOrder>();
        var getAllOrdersResponse = new GetAllOrdersResponse(orders);

        var mapper = new Mock<IMapper>();
        var orderRepository = new Mock<IOrderRepository>();
        orderRepository.Setup(x => x.GetAllAsync(CancellationToken.None)).ReturnsAsync(orderEntities);

        var handler = new GetAllOrdersHandler(mapper.Object, orderRepository.Object);

        // Act
        var result = await handler.Handle(new GetAllOrdersRequest(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Orders);
    }
}
