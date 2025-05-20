
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using ProductService.Context;
using ProductService.Services;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<InMemoryDBContext>();
builder.Services.AddScoped<IProductRepository,InMemoryProductRepositoty>();
builder.Services.AddGrpc();
builder.Services.AddAuthentication("Bearer")  // схема аутентификации - с помощью jwt-токенов
    .AddJwtBearer();      // подключение аутентификации с помощью jwt-токенов
builder.Services.AddAuthorization();  
builder.Services.AddControllers();

Log.Logger = new LoggerConfiguration()             // Create a new Serilog logger configuration
    .ReadFrom.Configuration(builder.Configuration) // Read settings from appsettings.json
    .CreateLogger();
builder.Host.UseSerilog(); // This ensures Serilog handles all logging

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();   // добавление middleware авторизации 
app.UseHttpsRedirection();
app.MapGrpcService<ProductStockService>();
app.UseRouting();
app.MapControllers();


app.Run();