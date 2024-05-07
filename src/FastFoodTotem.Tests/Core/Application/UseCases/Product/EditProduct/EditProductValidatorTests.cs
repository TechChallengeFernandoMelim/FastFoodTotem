using FastFoodTotem.Application.UseCases.Product.EditProduct;
using FluentValidation.TestHelper;

namespace FastFoodTotem.Tests.Core.Application.UseCases.Product.EditProduct;

public class EditProductValidatorTests
{
    private readonly EditProductValidator _validator;

    public EditProductValidatorTests()
    {
        _validator = new EditProductValidator();
    }

    [Theory]
    [InlineData(0)]
    public void Id_WhenInvalid_ShouldHaveValidationError(int id)
    {
        // Act
        var result = _validator.TestValidate(new EditProductRequest(id, "Burger", FastFoodTotem.Domain.Enums.CategoryType.Burguer, 10, "Delicious burger", "burger.jpg"));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("O id deve estar preenchido");
    }


    [Theory]
    [InlineData(0)]
    public void Price_WhenInvalid_ShouldHaveValidationError(decimal price)
    {
        // Act
        var result = _validator.TestValidate(new EditProductRequest(1, "Burger", FastFoodTotem.Domain.Enums.CategoryType.Burguer, price, "Delicious burger", "burger.jpg"));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price)
            .WithErrorMessage("O preço do produto deve estar especificado.");
    }


    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ProductImageUrl_WhenInvalid_ShouldHaveValidationError(string productImageUrl)
    {
        // Act
        var result = _validator.TestValidate(new EditProductRequest(1, "Burger", FastFoodTotem.Domain.Enums.CategoryType.Burguer, 10, "Delicious burger", productImageUrl));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductImageUrl)
            .WithErrorMessage("A imagem do produto deve estar preenchida.");
    }

    [Fact]
    public void Type_WhenInvalid_ShouldHaveValidationError()
    {
        // Act
        var result = _validator.TestValidate(new EditProductRequest(1, "Burger", (FastFoodTotem.Domain.Enums.CategoryType)10, 10, "Delicious burger", "burger.jpg")); // Arbitrary invalid enum value

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Type)
            .WithErrorMessage("O tipo do produto especificado não é válido.");
    }

    [Fact]
    public void AllFields_WhenValid_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var request = new EditProductRequest(1, "Burger", FastFoodTotem.Domain.Enums.CategoryType.Burguer, 10, "Delicious burger", "burger.jpg");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
