using AutoMapper;
using FastFoodTotem.Domain.Contracts.Payments;
using FastFoodTotem.Domain.Contracts.Repositories;
using FastFoodTotem.Domain.Entities;
using FastFoodTotem.Domain.Exceptions;
using MediatR;

namespace FastFoodTotem.Application.UseCases.Order.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public readonly IOrderPayment _orderPayment;

    public CreateOrderHandler(
        IMapper mapper, 
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IOrderPayment orderPayment)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _orderPayment = orderPayment ?? throw new ArgumentNullException(nameof(orderPayment));
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var orderEntity = _mapper.Map<OrderEntity>(request); 

        var productIds = orderEntity.OrderedItems.Select(x => x.ProductId);
        var productsNotFound = string.Empty;
        foreach (var productId in productIds)
        {
            var product = await _productRepository.GetProduct(productId, cancellationToken);
            if (product == null)
                productsNotFound += $"{productId} - ";
        }

        if (!string.IsNullOrWhiteSpace(productsNotFound))
            throw new ObjectNotFoundException($"Os produtos {productsNotFound} não foram encontrados na base de dados.");

        await _orderRepository.AddOrderAsync(orderEntity, cancellationToken);

        var response = new CreateOrderResponse()
        {
            Id = orderEntity.Id,
            PaymentQrCode = await _orderPayment.GerarQRCodeParaPagamentoDePedido(orderEntity, request.PaymentAccessToken),
            Status = orderEntity.Status,
            TotalPrice = orderEntity.GetTotal()
        };

        return response;
    }
}
