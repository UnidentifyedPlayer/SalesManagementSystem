namespace OrderService.Services;

public interface IProductClient
{
    Task<bool> CheckProduct(int productId);
    Task<bool> ReserveProduct(int productId, int stock);
}