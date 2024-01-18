﻿using FluentValidation;

namespace FastFoodTotem.Application.UseCases.Order.UpdatePaymentOrder;

public class UpdatePaymentOrderValidator : AbstractValidator<UpdatePaymentOrderRequest>
{
    public UpdatePaymentOrderValidator()
    {
        RuleFor(dto => dto.Action)
            .NotEmpty()
            .WithMessage("Action precisa estar preenchido.")
            .Must(dto => dto.Equals("payment.created"))
            .WithMessage("Action inválido para recebimento.");

        RuleFor(dto => dto.Data)
            .NotNull().WithMessage("Data não pode ser nulo.");

        RuleFor(dto => dto.Data.Id)
            .NotNull().NotEmpty().WithMessage("Id de data precisa estar preenchido.");
    }
}
