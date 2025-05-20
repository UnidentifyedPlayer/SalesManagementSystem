using Grpc.Core;
using ProductService.Context;

namespace ProductService.Services;

public class ProductStockService : ProductStock.ProductStockBase
{
    private readonly IProductRepository _productRepository;
    public ProductStockService(IProductRepository productRepo)
    {
        _productRepository = productRepo;
    }
    public override async Task<CheckProductReply> CheckProduct(CheckProductRequest request, ServerCallContext context)
    {
        bool is_available = false;

        var product = await _productRepository.GetByIdAsync(request.ProductId);
        
        if(product is not null)
            is_available = true;
        
        return await Task.FromResult(new CheckProductReply
        {
            IsAvailable = is_available
        });
    }

    public override async Task<ReserveProductReply> ReserveProduct(ReserveProductRequest request, ServerCallContext context)
    {
        bool is_successful = false;

        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is not null)
            if (request.Stock <= product.Stock)
            {
                product.Stock -= request.Stock;
                await _productRepository.UpdateAsync(product.Id, product.Stock);
                is_successful = true;
            }


        return await Task.FromResult(new ReserveProductReply
        {
            IsSuccessful = is_successful
        });
    }
    
}