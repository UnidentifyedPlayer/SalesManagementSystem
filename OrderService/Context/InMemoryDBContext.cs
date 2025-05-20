using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Context;

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
        modelBuilder.Entity<Order>()
            .HasData(new Order {Id = 1, ProductId = 1, Stock = 5}, 
                new Order {Id = 2, ProductId = 1, Stock = 2},
                new Order {Id = 3, ProductId = 1, Stock = 6});
    }
    
    
    public DbSet<Order> Orders { get; set; } = null!;
}