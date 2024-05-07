using AutoMapper;
using FastFoodTotem.Application.UseCases.Product.DeleteProduct;
using FastFoodTotem.Domain.Contracts.Repositories;
using FastFoodTotem.Domain.Entities;
using FastFoodTotem.Domain.Exceptions;
using Moq;

namespace FastFoodTotem.Tests.Core.Application.UseCases.Product.DeleteProduct;

public class DeleteProductHandlerTests
{
    [Fact]
    public async Task Handle_Should_Delete_Product_And_Return_Response()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var mapperMock = new Mock<IMapper>();

        var request = new DeleteProductRequest(1);
        var validator = new DeleteProductValidator();
        var productEntity = new ProductEntity { Id = 1, Name = "Burger", Type = FastFoodTotem.Domain.Enums.CategoryType.Burguer, Price = 10, Description = "Delicious burger", ProductImageUrl = "burger.jpg" };
        var mapper = new DeleteProductMapper();
        productRepositoryMock.Setup(r => r.GetProduct(request.ProductId, CancellationToken.None)).ReturnsAsync(productEntity);

        var handler = new DeleteProductHandler(productRepositoryMock.Object, mapperMock.Object);

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        productRepositoryMock.Verify(r => r.DeleteProduct(productEntity, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_ObjectNotFoundException_When_Product_Not_Found()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var mapperMock = new Mock<IMapper>();

        var request = new DeleteProductRequest(1);

        productRepositoryMock.Setup(r => r.GetProduct(request.ProductId, CancellationToken.None)).ReturnsAsync((ProductEntity)null);

        var handler = new DeleteProductHandler(productRepositoryMock.Object, mapperMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ObjectNotFoundException>(() => handler.Handle(request, CancellationToken.None));
        Assert.NotNull(exception);
    }
}
