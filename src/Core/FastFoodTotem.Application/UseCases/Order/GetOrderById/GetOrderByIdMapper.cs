using AutoMapper;
using FastFoodTotem.Domain.Entities;

namespace FastFoodTotem.Application.UseCases.Order.GetOrderById;

public class GetOrderByIdMapper : Profile
{

    public GetOrderByIdMapper()
    {
        CreateMap<OrderEntity, GetOrderByIdResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.UserCpf, opt => opt.MapFrom(src => src.UserCpf))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => MapOrderedItems(src.OrderedItems)));
    }

    private IEnumerable<GetOrderByIdProductData> MapOrderedItems(IEnumerable<OrderedItemEntity> orderedItems)
    {
        return orderedItems.Select(item => new GetOrderByIdProductData
        {
            ProductId = item.ProductId,
            Name = item.Product.Name,
            Amount = item.Amount,
            Price = item.Product.Price
        });
    }
}
