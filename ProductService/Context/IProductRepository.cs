using ProductService.Models;

namespace ProductService.Context;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int orderId);
    Task InsertAsync(Product order);
    Task<Product> UpdateAsync(int productId, int stock); //TODO change parameters to a single Product object 
    Task DeleteAsync(int orderId);
}