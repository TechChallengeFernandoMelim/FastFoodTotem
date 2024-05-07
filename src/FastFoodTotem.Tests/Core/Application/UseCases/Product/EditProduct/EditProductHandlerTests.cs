using AutoMapper;
using FastFoodTotem.Application.UseCases.Product.EditProduct;
using FastFoodTotem.Domain.Contracts.Repositories;
using FastFoodTotem.Domain.Entities;
using FastFoodTotem.Domain.Exceptions;
using Moq;

namespace FastFoodTotem.Tests.Core.Application.UseCases.Product.EditProduct;

public class EditProductHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly EditProductHandler _editProductHandler;

    public EditProductHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();
        _editProductHandler = new EditProductHandler(_mockProductRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async void Handle_ProductExists_ShouldEditProduct()
    {
        // Arrange
        var request = new EditProductRequest(1, "Burger", FastFoodTotem.Domain.Enums.CategoryType.Burguer, 10, "Delicious burger", "burger.jpg");
        var productEntity = new ProductEntity { Id = 1, Name = "Old Name", Type = FastFoodTotem.Domain.Enums.CategoryType.Drink, Price = 5 };
        var mapper = new EditProductMapper();
        _mockMapper.Setup(x => x.Map<ProductEntity>(request)).Returns(productEntity);
        _mockProductRepository.Setup(x => x.GetProduct(1, default)).ReturnsAsync(productEntity);
        _mockMapper.Setup(x => x.Map<EditProductResponse>(It.IsAny<ProductEntity>())).Returns(new EditProductResponse
        {
            Description = productEntity.Description,
            Id = productEntity.Id,
            Name = productEntity.Name,
            Price = productEntity.Price,
            ProductImageUrl = productEntity.ProductImageUrl,
            Type = productEntity.Type
        });

        // Act
        var response = await _editProductHandler.Handle(request, default);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(productEntity.Id, response.Id);
        Assert.Equal(productEntity.Name, response.Name);
        Assert.Equal(productEntity.Type, response.Type);
        Assert.Equal(productEntity.Price, response.Price);
    }

    [Fact]
    public void Handle_ProductDoesNotExist_ShouldThrowObjectNotFoundException()
    {
        // Arrange
        var request = new EditProductRequest(1, "Burger", FastFoodTotem.Domain.Enums.CategoryType.Burguer, 10, "Delicious burger", "burger.jpg");
        _mockMapper.Setup(x => x.Map<ProductEntity>(request)).Returns(new ProductEntity { Id = 1 });

        // Act & Assert
        Assert.ThrowsAsync<ObjectNotFoundException>(() => _editProductHandler.Handle(request, default));
    }
}
