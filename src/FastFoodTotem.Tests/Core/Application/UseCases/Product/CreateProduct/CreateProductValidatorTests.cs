using FastFoodTotem.Application.UseCases.Product.CreateProduct;
using FastFoodTotem.Domain.Enums;
using FluentValidation.TestHelper;

namespace FastFoodTotem.Tests.Core.Application.UseCases.Product.CreateProduct;


public class CreateProductValidatorTests
{
    [Fact]
    public void Name_Should_Not_Be_Empty()
    {
        // Arrange
        var validator = new CreateProductValidator();
        var request = new CreateProductRequest("", FastFoodTotem.Domain.Enums.CategoryType.Burguer, 10, "Description", "ImageUrl");

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Name_Should_Have_Minimum_Length()
    {
        // Arrange
        var validator = new CreateProductValidator();
        var request = new CreateProductRequest("a", FastFoodTotem.Domain.Enums.CategoryType.Burguer, 10, "Description", "ImageUrl");

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Name_Should_Have_Maximum_Length()
    {
        // Arrange
        var validator = new CreateProductValidator();
        var request = new CreateProductRequest(new string('a', 256), FastFoodTotem.Domain.Enums.CategoryType.Burguer, 10, "Description", "ImageUrl");

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Type_Should_Be_Valid_Enum_Value()
    {
        // Arrange
        var validator = new CreateProductValidator();
        var request = new CreateProductRequest("Name", (FastFoodTotem.Domain.Enums.CategoryType)100, 10, "Description", "ImageUrl");

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Type);
    }

    [Fact]
    public void Price_Should_Not_Be_Empty()
    {
        // Arrange
        var validator = new CreateProductValidator();
        var request = new CreateProductRequest("Name", FastFoodTotem.Domain.Enums.CategoryType.Burguer, 0, "Description", "ImageUrl");

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Description_Should_Not_Be_Empty()
    {
        // Arrange
        var validator = new CreateProductValidator();
        var request = new CreateProductRequest("Name", FastFoodTotem.Domain.Enums.CategoryType.Burguer, 10, "", "ImageUrl");

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Description_Should_Have_Minimum_Length()
    {
        // Arrange
        var validator = new CreateProductValidator();
        var request = new CreateProductRequest("Name", FastFoodTotem.Domain.Enums.CategoryType.Burguer, 10, "a", "ImageUrl");

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Description_Should_Have_Maximum_Length()
    {
        // Arrange
        var validator = new CreateProductValidator();
        var request = new CreateProductRequest("Name", FastFoodTotem.Domain.Enums.CategoryType.Burguer, 10, new string('a', 256), "ImageUrl");

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void ProductImageUrl_Should_Not_Be_Empty()
    {
        // Arrange
        var validator = new CreateProductValidator();
        var request = new CreateProductRequest("Name", FastFoodTotem.Domain.Enums.CategoryType.Burguer, 10, "Description", "");

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductImageUrl);
    }

}

