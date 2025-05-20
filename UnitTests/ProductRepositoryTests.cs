using ProductService.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProductService.Models;

namespace UnitTests;


public class ProductRepositoryTests
{
    private InMemoryProductRepositoty _productRepository;
    private readonly DbContextOptions<InMemoryDBContext> _contextOptions;

    // Constructor initializes the InMemoryProductRepositoty instance
    public ProductRepositoryTests()
    {
        _contextOptions = new DbContextOptionsBuilder<InMemoryDBContext>()
            .UseInMemoryDatabase("ProductRepositoryTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        
    }

    public async Task InitializeRepository()
    {
        var context = new InMemoryDBContext(_contextOptions);

        _productRepository = new InMemoryProductRepositoty(context);
        
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    // Test to verify that GetByIdAsync returns the correct product
    [Fact]
    public async Task GetProductById_ReturnsCorrectProduct()
    {
        // Arrange
        await InitializeRepository();
        var productId = 1;

        // Act
        var result = await _productRepository.GetByIdAsync(productId);

        // Assert
        // Check that result is not null
        Assert.NotNull(result);

        // Check that the ID of the returned product is correct
        Assert.Equal(productId, result.Id);
    }

    // Test to verify that GetByIdAsync returns null when the product is not found
    [Fact]
    public async Task GetProductById_ReturnsNullWhenProductNotFound()
    {
        // Arrange
        // Assuming this ID does not exist
        await InitializeRepository();
        var productId = 99;

        // Act
        var result = await _productRepository.GetByIdAsync(productId);

        // Assert
        // Check that result is null
        Assert.Null(result);
    }

    // Test to verify that GetAllAsync returns all product
    [Fact]
    public async Task GetAllProducts_ReturnsAllProducts()
    {
        // Act
        await InitializeRepository();
        var result = await _productRepository.GetAllAsync();

        // Assert
        // Check that result is not null
        Assert.NotNull(result);

        // Assuming there are 3 products, check that the count is correct
        Assert.Equal(3, result.Count());
    }

    // Test to verify that InsertAsync adds a product correctly
    [Fact]
    public async Task AddProduct_AddsProductCorrectly()
    {
        // Arrange
        await InitializeRepository();
        var newProduct = new Product {Id = 4, Name = "Test Product", Stock = 50};

        // Act
        await _productRepository.InsertAsync(newProduct);
        var result = await _productRepository.GetByIdAsync(4);

        // Assert
        Assert.NotNull(result); // Check that the product was added and returned
        Assert.Equal(newProduct.Id, result.Id); // Check that the ID is correct
        Assert.Equal(newProduct.Name, result.Name); // Check that the name is correct
        Assert.Equal(newProduct.Stock, result.Stock); // Check that the stock is correct
    }

    // Test to verify that UpdateUser updates a product correctly
    [Fact]
    public async Task UpdateUser_UpdatesProductCorrectly()
    {
        // Arrange
        await InitializeRepository();
        var updatedProduct = new Product {Id = 1, Name = "Test Product", Stock = 50};

        // Act
        await _productRepository.UpdateAsync(updatedProduct.Id, updatedProduct.Stock);
        var result = await _productRepository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result); // Check that the product was found
        Assert.Equal(updatedProduct.Stock, result.Stock); // Check that the stock was updated
    }

    // Test to verify that DeleteAsync deletes a product correctly
    [Fact]
    public async Task DeleteUser_DeletesProductCorrectly()
    {
        // Arrange
        await InitializeRepository();
        var productId = 1;

        // Act
        await _productRepository.DeleteAsync(productId);
        var result = await _productRepository.GetByIdAsync(productId);

        // Assert
        Assert.Null(result); // Check that the product was deleted and cannot be found
    }
}
    