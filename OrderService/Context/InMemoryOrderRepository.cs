using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Context;

public class InMemoryOrderRepository : IOrderRepository
{
    private InMemoryDBContext _dbContext;

    public InMemoryOrderRepository(InMemoryDBContext context)
    {
        _dbContext = context;
    }
    public async Task<IEnumerable<Order>> GetAllAsync()
    {
         return  await _dbContext.Orders.ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int orderId)
    {
        return await _dbContext.Orders.FindAsync(orderId);
    }

    public async Task InsertAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int orderId)
    {
        throw new NotImplementedException();
    }
    
}