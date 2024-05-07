using FastFoodTotem.Application.UseCases.Order.CreateOrder;
using FluentValidation.TestHelper;

namespace FastFoodTotem.Tests.Core.Application.UseCases.Order.CreateOrder;

public class CreateOrderValidatorTests
{
    [Fact]
    public void Should_Have_Error_When_OrderedItems_Is_Empty()
    {
        // Arrange
        var validator = new CreateOrderValidator();
        var request = new CreateOrderRequest { OrderedItems = new OrderItens[0] };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OrderedItems)
            .WithErrorMessage("O pedido precisa ter pelo menos um produto para ser criado.");
    }

    [Theory]
    [InlineData("433.481.300-34")]
    [InlineData("43348130034")]
    public void Should_Not_Have_Error_When_UserCpf_Is_Valid(string cpf)
    {
        // Arrange
        var validator = new CreateOrderValidator();
        var request = new CreateOrderRequest { UserCpf = cpf };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserCpf);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Should_Not_Have_Error_When_UserCpf_Is_Null_Or_Empty(string cpf)
    {
        // Arrange
        var validator = new CreateOrderValidator();
        var request = new CreateOrderRequest { UserCpf = cpf };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserCpf);
    }

    [Theory]
    [InlineData("12345678901")]
    [InlineData("123")]
    public void Should_Have_Error_When_UserCpf_Is_Invalid(string cpf)
    {
        // Arrange
        var validator = new CreateOrderValidator();
        var request = new CreateOrderRequest { UserCpf = cpf };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserCpf)
            .WithErrorMessage("O CPF do usuário precisa ser válido quando fornecido.");
    }
}

public class OrderItensValidatorTests
{
    [Fact]
    public void Should_Have_Error_When_Amount_Is_Less_Than_Or_Equal_To_Zero()
    {
        // Arrange
        var validator = new OrderItensValidator();
        var orderItem = new OrderItens { ProductId = 1, Amount = 0 };

        // Act
        var result = validator.TestValidate(orderItem);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Amount)
            .WithErrorMessage("A quantidade do produto precisa ser maior que zero.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Amount_Is_Greater_Than_Zero()
    {
        // Arrange
        var validator = new OrderItensValidator();
        var orderItem = new OrderItens { ProductId = 1, Amount = 1 };

        // Act
        var result = validator.TestValidate(orderItem);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Amount);
    }
}
