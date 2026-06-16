using FulfillmentCenter.Data;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Implementations;
using FulfillmentCenter.Services.Interfaces;
using FulfillmentCenter.Services.MapperDto.Implementations;
using FulfillmentCenter.Services.MapperDto.Interfaces;
using FulfillmentCenter.Services.UpdateOrderStatus;
using FulfillmentCenter.Services.UpdateOrderStatus.Implementations;
using FulfillmentCenter.Services.UpdateOrderStatus.Interfaces;
using FulfillmentCenter.Strategies.Implementations;
using FulfillmentCenter.Strategies.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("FulfilmentCenterDatabase");
builder.Services.AddDbContext<FulfillmentCenDbContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            if (builder.Environment.IsDevelopment())
            {
                options.LogTo(
                    msg =>
                    {
                        var lines = msg.Split(Environment.NewLine);

                        var sqlLines = lines
                            .SkipWhile(l =>
                                !l.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase) &&
                                !l.TrimStart().StartsWith("INSERT", StringComparison.OrdinalIgnoreCase) &&
                                !l.TrimStart().StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) &&
                                !l.TrimStart().StartsWith("DELETE", StringComparison.OrdinalIgnoreCase));

                        var sql = string.Join(Environment.NewLine, sqlLines);

                        if (!string.IsNullOrWhiteSpace(sql))
                        {
                            Console.WriteLine(sql);
                        }
                    },
                    LogLevel.Information,
                    DbContextLoggerOptions.Id// | DbContextLoggerOptions.SingleLine
                    ).EnableSensitiveDataLogging();
            }
        }
);
builder.Services.AddMemoryCache();

builder.Services.AddTransient<IShipmentAssignmentStrategy, HighestStockStrategy>();
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

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

builder.Services.AddScoped<OrderHandlerFactory>();
builder.Services.AddScoped<IOrderStatusHandler, OrderCancelHandler>();
builder.Services.AddScoped<IMapper<Product, ResponseProductDto>, ProductMapper>();
builder.Services.AddScoped<IMapper<Order, ResponseOrderDto>, OrderMapper>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();

builder.Services.AddProblemDetails();

var app = builder.Build();

app.MapControllers();

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
    app.UseStatusCodePages();
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