using Serilog;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using OrderService.Context;
using Grpc.Net.Client;
using OrderService.Services;
using ProductService;
using Serilog;

namespace OrderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // CreateHostBuilder(args).Build().Run();
            
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddGrpc();
            builder.Services.AddDbContext<InMemoryDBContext>();
            builder.Services.AddScoped<IOrderRepository,InMemoryOrderRepository>();
            builder.Services.AddScoped<IProductClient,GrpcProductClient>();
            
            Log.Logger = new LoggerConfiguration()             // Create a new Serilog logger configuration
                .ReadFrom.Configuration(builder.Configuration) // Read settings from appsettings.json
                .CreateLogger();
            builder.Host.UseSerilog(); // This ensures Serilog handles all logging
            
            builder.Services.AddControllers();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.MapControllers();
            app.Run();
        }
        
    }
}
