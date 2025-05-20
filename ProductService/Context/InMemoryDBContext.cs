using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Context;

public class InMemoryDBContext : DbContext
{
    public InMemoryDBContext(DbContextOptions<InMemoryDBContext> options) : base(options)
    {
        Database.EnsureCreated(); // создаем базу данных при первом обращении
    }
    
    protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "ProductDB");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasData(new Product {Id = 1, Name = "Orange", Stock = 37}, 
                new Product {Id = 2, Name = "Apple", Stock = 41},
                new Product {Id = 3, Name = "Pear", Stock = 24});
    }
    
    
    public DbSet<Product> Products { get; set; } = null!;
}