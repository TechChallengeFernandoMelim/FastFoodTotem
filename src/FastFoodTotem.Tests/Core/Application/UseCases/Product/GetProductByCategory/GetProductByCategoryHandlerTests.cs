using AutoMapper;
using FastFoodTotem.Application.UseCases.Product.GetProductByCategory;
using FastFoodTotem.Domain.Contracts.Repositories;
using FastFoodTotem.Domain.Entities;
using Moq;

namespace FastFoodTotem.Tests.Core.Application.UseCases.Product.GetProductByCategory;

public class GetProductByCategoryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Products_By_Category()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var mapperMock = new Mock<IMapper>();
        var validator = new GetProductByCategoryValidator();
        var request = new GetProductByCategoryRequest(FastFoodTotem.Domain.Enums.CategoryType.Burguer);
        var products = new List<ProductEntity>
            {
                new ProductEntity { Id = 1, Name = "Burger1", Type = FastFoodTotem.Domain.Enums.CategoryType.Burguer, Price = 10, Description = "Delicious burger", ProductImageUrl = "burger.jpg" },
                new ProductEntity { Id = 2, Name = "Burger2", Type = FastFoodTotem.Domain.Enums.CategoryType.Burguer, Price = 12, Description = "Tasty burger", ProductImageUrl = "burger.jpg" }
            };

        productRepositoryMock.Setup(r => r.GetProductsByCategory(request.Type, CancellationToken.None)).ReturnsAsync(products);

        var handler = new GetProductByCategoryHandler(productRepositoryMock.Object, mapperMock.Object);

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
    }
}
