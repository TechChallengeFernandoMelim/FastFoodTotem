using FastFoodTotem.Domain.Entities;

namespace FastFoodTotem.Domain.Contracts.Payments
{
    public interface IOrderPayment
    {
        Task<string[]> GerarQRCodeParaPagamentoDePedido(OrderEntity orderEntity, string accesstoken);
    }
}
