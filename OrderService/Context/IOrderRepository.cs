using OrderService.Models;

namespace OrderService.Context;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int orderId);
    Task InsertAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(int orderId);
}