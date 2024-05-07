using AutoMapper;
using FastFoodTotem.Application.UseCases.Product.CreateProduct;
using FastFoodTotem.Domain.Contracts.Repositories;
using FastFoodTotem.Domain.Entities;
using Moq;

namespace FastFoodTotem.Tests.Core.Application.UseCases.Product.CreateProduct;

public class CreateProductHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Product_And_Return_Response()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var mapperMock = new Mock<IMapper>();

        var request = new CreateProductRequest("Burger", FastFoodTotem.Domain.Enums.CategoryType.Burguer, 10, "Delicious burger", "burger.jpg");
        var productEntity = new ProductEntity { Name = "Burger", Type = FastFoodTotem.Domain.Enums.CategoryType.Burguer, Price = 10, Description = "Delicious burger", ProductImageUrl = "burger.jpg" };
        var createProductResponse = new CreateProductResponse { Id = 1, Name = "Burger", Type = FastFoodTotem.Domain.Enums.CategoryType.Burguer, Price = 10, Description = "Delicious burger", ProductImageUrl = "burger.jpg" };

        mapperMock.Setup(m => m.Map<ProductEntity>(request)).Returns(productEntity);
        productRepositoryMock.Setup(r => r.CreateProduct(productEntity, CancellationToken.None)).Returns(Task.FromResult(productEntity));
        mapperMock.Setup(m => m.Map<CreateProductResponse>(productEntity)).Returns(createProductResponse);

        var handler = new CreateProductHandler(productRepositoryMock.Object, mapperMock.Object);

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(createProductResponse, response);
    }
}
