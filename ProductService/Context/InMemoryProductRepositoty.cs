using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Context;

public class InMemoryProductRepositoty : IProductRepository
{
    private InMemoryDBContext _dbContext;
    
    public InMemoryProductRepositoty(InMemoryDBContext context)
    {
        _dbContext = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int orderId)
    {
        return await _dbContext.Products.FindAsync(orderId);
    }

    public async Task InsertAsync(Product order)
    {
        await _dbContext.Products.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Product> UpdateAsync(int productId, int stock)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        product.Stock = stock;
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task DeleteAsync(int productId)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
    }
}