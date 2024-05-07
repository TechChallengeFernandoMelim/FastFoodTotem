using FastFoodTotem.Application.MediatorPipes.Behavior;
using FastFoodTotem.Domain.Validations;
using FluentValidation;
using MediatR;
using Moq;

namespace FastFoodTotem.Tests.Core.Application;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_WithInvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var validatorMock = new Mock<IValidator<MyRequest>>();
        var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Property", "Error message")
            });
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<MyRequest>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(validationResult);

        var validationNotificationsMock = new Mock<IValidationNotifications>();
        var behavior = new ValidationBehavior<MyRequest, MyResponse>(validatorMock.Object, validationNotificationsMock.Object);
        var request = new MyRequest();
        var cancellationToken = new CancellationToken();

        // Act & Assert
        await Assert.ThrowsAsync<FastFoodTotem.Domain.Exceptions.ValidationException>(() => behavior.Handle(request, null, cancellationToken));
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_AddsErrorsToValidationNotifications()
    {
        // Arrange
        var validatorMock = new Mock<IValidator<MyRequest>>();
        var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Property", "Error message")
            });
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<MyRequest>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(validationResult);

        var validationNotificationsMock = new Mock<IValidationNotifications>();
        var behavior = new ValidationBehavior<MyRequest, MyResponse>(validatorMock.Object, validationNotificationsMock.Object);
        var request = new MyRequest();
        var cancellationToken = new CancellationToken();

        // Act
        await Assert.ThrowsAsync<FastFoodTotem.Domain.Exceptions.ValidationException>(() => behavior.Handle(request, null, cancellationToken));

        // Assert
        validationNotificationsMock.Verify(vn => vn.AddError("Property", "Error message"), Times.Once);
    }

    public class MyRequest : IRequest<MyResponse> { }

    public class MyResponse { }
}
