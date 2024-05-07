using AutoMapper;
using FastFoodTotem.Domain.Entities;

namespace FastFoodTotem.Application.UseCases.Product.EditProduct;

public class EditProductMapper : Profile
{
    public EditProductMapper()
    {
        CreateMap<EditProductRequest, ProductEntity>();
        CreateMap<ProductEntity, EditProductResponse>();
    }
}
