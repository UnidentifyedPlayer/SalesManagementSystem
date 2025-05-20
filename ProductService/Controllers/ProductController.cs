using Microsoft.AspNetCore.Mvc;
using ProductService.Context;
using ProductService.Models;

namespace ProductService.Controllers;

[Route("api/product")]
[ApiController]
public class ProductController : Controller
{
    private ILogger<ProductController> _logger;
    private IProductRepository _productRepository;
    
    public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> Get()
    {
        var products = await _productRepository.GetAllAsync();
        return Ok(products);
    }

    [HttpGet]
    public async Task<IActionResult>  Get(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        return Ok(product);
    }
    
    [HttpPost]
    public async Task<IActionResult>  Create(Product product)
    {
        try
        {
            await _productRepository.InsertAsync(product);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(500);
        }
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpPut]
    public async Task<IActionResult>  Update(int id, int stock)
    {
        var product = new Product();
        try
        {
            product = await _productRepository.UpdateAsync(id, stock);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(500);
        }
        return Ok(product);
    }

    [HttpDelete]
    public async Task<IActionResult>  Delete(int id)
    {
        try
        { 
            await _productRepository.DeleteAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(500);
        }

        return Ok();
    }
}