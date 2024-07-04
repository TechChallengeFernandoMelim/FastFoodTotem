using MediatR;

namespace FastFoodTotem.Application.UseCases.Order.DeleteUserData;

public record DeleteUserDataRequest(string cpf) : IRequest<DeleteUserDataResponse>;
