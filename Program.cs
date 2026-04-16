using FulfillmentCenter.Controllers;
using FulfillmentCenter.Data;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Implementations;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var connectionString = builder.Configuration.GetConnectionString("FulfilmentCenterDatabase");
builder.Services.AddDbContext<FulfillmentCenDbContext>(options =>
    {
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            if (builder.Environment.IsDevelopment())
            {
                options.LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging();
            }
        }
    }
);
//builder.Services.AddDbContext<FulfillmentCenDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IFulfillmentCenterService, FulfillmentCenterService>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();

builder.Services.AddScoped<IFulfillmentCenterRepository, SqlFulfillmentCenterRepository>();
builder.Services.AddScoped<IInventoryRepository, SqlInventoryRepository>();
builder.Services.AddScoped<IOrderItemRepository, SqlOrderItemRepository>();
builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();
builder.Services.AddScoped<IProductRepository, SqlProductRepository>();
builder.Services.AddScoped<IShipmentRepository, SqlShipmentRepository>();

builder.Services.AddControllers();
/*builder.Services.AddScoped<OrderItemController, OrderItemController>();
builder.Services.AddScoped<InventoryController, InventoryController>();
builder.Services.AddScoped<OrdersController, OrdersController>();
builder.Services.AddScoped<ProductsController, ProductsController>();
builder.Services.AddScoped<ShipmentsController, ShipmentsController>();*/

builder.Services.AddProblemDetails();
/*
builder.Services.AddSingleton<FulfillmentCenDbContext>();
builder.Services.AddSingleton<DbContext,FulfillmentCenDbContext>();

builder.Services.AddScoped<DbContext, FulfillmentCenDbContext>();//TODO: to check if this is okay, because i need to register interface first
*/


var app = builder.Build();

//DbContextOptions

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.MapGet("/testing", () => "TESTING");
    app.MapGet("/db-test", async (FulfillmentCenDbContext db) =>
    {
        var canConnect = await db.Database.CanConnectAsync();

        return canConnect ? "Database connection OK ✅" : "Database connection FAILED ❌";
    });
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.Map("/error", (HttpContext context) =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

    return Results.Problem(
        title: "Server error",
        detail: exception?.Message
    );
});



app.Run();