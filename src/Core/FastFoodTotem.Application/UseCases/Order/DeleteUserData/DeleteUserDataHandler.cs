using FastFoodTotem.Domain.Contracts.Repositories;
using MediatR;

namespace FastFoodTotem.Application.UseCases.Order.DeleteUserData;

public class DeleteUserDataHandler : IRequestHandler<DeleteUserDataRequest, DeleteUserDataResponse>
{
    private IOrderRepository _orderRepository;

    public DeleteUserDataHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<DeleteUserDataResponse> Handle(DeleteUserDataRequest request, CancellationToken cancellationToken)
    {
        await _orderRepository.DeleteUserDataByCpf(request.cpf, cancellationToken);
        return new DeleteUserDataResponse();
    }
}
