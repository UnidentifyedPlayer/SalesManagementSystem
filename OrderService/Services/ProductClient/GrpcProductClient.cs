using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using ProductService;
using ILogger = Serilog.ILogger;

namespace OrderService.Services;


public class GrpcProductClient : IProductClient
{
    private readonly string _productServiceAddress;
    private readonly ILogger<GrpcProductClient> _logger;

    public GrpcProductClient(ILogger<GrpcProductClient> logger)
    {
        _logger = logger;
        _productServiceAddress = "http://localhost:5002";
    }

    private ProductStock.ProductStockClient ConnectToService()
    {
        var defaultMethodConfig = new MethodConfig
        {
            Names = { MethodName.Default },
            RetryPolicy = new RetryPolicy
            {
                MaxAttempts = 5,
                InitialBackoff = TimeSpan.FromSeconds(1),
                MaxBackoff = TimeSpan.FromSeconds(5),
                BackoffMultiplier = 1.5,
                RetryableStatusCodes = { StatusCode.Unavailable }
            }
        };
        
        var channel = GrpcChannel.ForAddress(_productServiceAddress, new GrpcChannelOptions
        {
            ServiceConfig = new ServiceConfig { MethodConfigs = { defaultMethodConfig } }
        });
        var productStockClient = new ProductStock.ProductStockClient(channel);
        return productStockClient;
    }
    public async Task<bool> CheckProduct(int productId)
    {
        
        var productStockClient = ConnectToService();
        try
        {
            var response = await productStockClient.CheckProductAsync(new CheckProductRequest
                {ProductId = productId}, deadline: DateTime.UtcNow.AddSeconds(5));
            return response.IsAvailable;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
        {
            _logger.LogError("Deadline for Grpc Request to ProductService exceeded");
            return false;
        }
    }
    
    public async Task<bool> ReserveProduct(int productId, int stock)
    {
        var productStockClient = ConnectToService();
        try
        {
            var reservationResponse = await productStockClient.ReserveProductAsync(new ReserveProductRequest
                {ProductId = productId, Stock = stock}, deadline: DateTime.UtcNow.AddSeconds(5));
            return reservationResponse.IsSuccessful;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
        {
            _logger.LogError("Deadline for Grpc Request to ProductService exceeded");
            return false;
        }
    }
    
    
    
}