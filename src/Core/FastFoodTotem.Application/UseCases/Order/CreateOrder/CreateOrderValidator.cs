using FluentValidation;

namespace FastFoodTotem.Application.UseCases.Order.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderValidator()
    {
        RuleFor(dto => dto.OrderedItems)
            .NotEmpty()
            .WithMessage("O pedido precisa ter pelo menos um produto para ser criado.")
            .ForEach(item => item.SetValidator(new OrderItensValidator()));

        RuleFor(dto => dto.UserCpf)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().When(dto => !string.IsNullOrWhiteSpace(dto.UserCpf))
                   .WithMessage("O CPF do usuário não pode estar vazio quando fornecido.")
               .Must(BeValidCpf).When(dto => !string.IsNullOrWhiteSpace(dto.UserCpf))
                   .WithMessage("O CPF do usuário precisa ser válido quando fornecido.");
    }

    public bool BeValidCpf(string cpf)
    {
        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCpf;
        string digito;
        int soma;
        int resto;
        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");
        if (cpf.Length != 11)
            return false;
        tempCpf = cpf.Substring(0, 9);
        soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digito = digito + resto.ToString();

        return cpf.EndsWith(digito);
    }
}

public class OrderItensValidator : AbstractValidator<OrderItens>
{
    public OrderItensValidator()
    {
        RuleFor(dto => dto.ProductId)
            .NotEmpty()
            .WithMessage("O id do produto precisa estar preenchido.");

        RuleFor(dto => dto.Amount)
            .GreaterThan(0)
            .WithMessage("A quantidade do produto precisa ser maior que zero.");

    }
}