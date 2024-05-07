using AutoMapper;
using FastFoodTotem.Domain.Contracts.Repositories;
using FastFoodTotem.Domain.Entities;
using FastFoodTotem.Domain.Exceptions;
using MediatR;

namespace FastFoodTotem.Application.UseCases.Product.EditProduct;

public class EditProductHandler : IRequestHandler<EditProductRequest, EditProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public EditProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
    }

    public async Task<EditProductResponse> Handle(EditProductRequest request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<ProductEntity>(request);

        var existentProduct = await _productRepository.GetProduct(product.Id, cancellationToken);

        if (existentProduct == null)
            throw new ObjectNotFoundException();

        existentProduct.Price = product.Price;
        existentProduct.Name = product.Name;
        existentProduct.Type = product.Type;

        await _productRepository.EditProduct(existentProduct, cancellationToken);


        return _mapper.Map<EditProductResponse>(existentProduct);
    }
}
