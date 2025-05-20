using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OrderService.Context;
using OrderService.Models;
using Grpc.Net.Client;
using OrderService.Services;
using ProductService;


namespace OrderService.Controllers;

[Route("api/order")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductClient _productClient;

    public OrderController(ILogger<OrderController> logger, IOrderRepository orderRepository, IProductClient productClient)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _productClient = productClient;
    }

    [HttpPost]
    public async Task<IActionResult> Create(int productId, int stock)
    {
        if (! await _productClient.CheckProduct(productId))
        {
            _logger.LogInformation("No product with id={ProductId} available", productId);
            return NotFound("No product available");
        }
        if (! await _productClient.ReserveProduct(productId,stock))
        {
            _logger.LogInformation("Unable to create order of {Stock} of Product {ProductId}, either " +
                             "the product does not exists, or not enough left in the stock", stock, productId);
            return NotFound("Not enough product in stock or it doesn't exist");
        }

        Order newOrder = new Order() {ProductId = productId, Stock = stock};
        await _orderRepository.InsertAsync(newOrder);
        return Ok(newOrder);
    }

    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _orderRepository.GetByIdAsync(id);
        if (product is null)
        {
            _logger.LogInformation("Unable to find order with id={Id}", id);
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAll()
    {
        var orders =  await _orderRepository.GetAllAsync();
        return Ok(orders);
    }
    

}