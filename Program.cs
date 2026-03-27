using FulfillmentCenter.Controllers;
using FulfillmentCenter.Data;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Implementations;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var connectionString = builder.Configuration.GetConnectionString("FulfilmentCenterDatabase");
builder.Services.AddDbContext<FulfillmentCenDbContext>(options => options.UseMySql(connectionString, 
    ServerVersion.AutoDetect(connectionString))
        .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
        .EnableSensitiveDataLogging() // покажет параметры
);
//builder.Services.AddDbContext<FulfillmentCenDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddControllers();

builder.Services.AddTransient<IInventoryService, InventoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IFulfillmentCenterService, FulfillmentCenterService>();
builder.Services.AddTransient<IShipmentService, ShipmentService>();

builder.Services.AddTransient<IFulfillmentCenterRepository, SqlFulfillmentCenterRepository>();
builder.Services.AddTransient<IInventoryRepository, SqlInventoryRepository>();
builder.Services.AddScoped<IOrderItemRepository, SqlOrderItemRepository>();
builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();
builder.Services.AddTransient<IProductRepository, SqlProductRepository>();
builder.Services.AddTransient<IShipmentRepository, SqlShipmentRepository>();

//builder.Services.AddControllers();

builder.Services.AddScoped<InventoryController, InventoryController>();
builder.Services.AddScoped<OrdersController, OrdersController>();
builder.Services.AddScoped<ProductsController, ProductsController>();
builder.Services.AddScoped<ShipmentsController, ShipmentsController>();

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
}

app.MapGet("/testing", () => "TESTING");
app.MapGet("/db-test", async (FulfillmentCenDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();

    return canConnect ? "Database connection OK ✅" : "Database connection FAILED ❌";
});

//app.MapGet("/api/products", (IProductService productService) => productService.GetProducts());

//app.MapGet("/api/products", (ProductsController productController) => productController.GetProducts());
//DONE
app.MapGet("/api/products", async (ProductsController productController) => 
    await productController.GetProducts());
//DONE
app.MapPost("/api/products", (ProductsController productController) => productController.AddProduct(new RequestProductDto
{
    Name = "AirPods 10",
    SKU = "33",
    Weight = 0
}));
app.MapGet("/api/inventory/{centerId}", async (InventoryController inventoryController) =>
    await inventoryController.InventoryRemaining(Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e")));
//DONE
app.MapPost("/api/orders", (OrdersController ordersController) => ordersController.CreateOrder(new RequestOrderDto
{
    CustomerName = "ANDREI",
    DeliveryAddress = "Pražská 636/38b, 642 00 Brno",
    Status = OrderStatus.Created
}));




app.MapPost("/api/inventory", (InventoryController inventoryController) => inventoryController.AddStock(new RequestInventoryDto
{//TODO: this one is TOUGH TOUGH TOUGH
    DistributionCenterId = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),
    ProductId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000"),
    Quantity = 2
}));




//TODO: to fix this
app.MapPut("/api/orders/{id}", (OrdersController ordersController) => ordersController.ChangeOrderStatus(Guid.NewGuid(), OrderStatus.Processing));

//[FromRoute] Guid id  //TODO: to see if i need to fix it to make it "[FromRoute] Guid id"
app.MapGet("/api/orders/{id}/status", (OrdersController ordersController) => ordersController.GetOrder(Guid.NewGuid()));


app.MapPost("/api/shipments", (ShipmentsController shipmentsController) => shipmentsController.CreateShipment(new RequestShipmentDto
{
    DistributionCenterId = Guid.NewGuid(),
    EstimatedDelivery = DateTime.Now,
    Id = Guid.NewGuid(),
    OrderId = Guid.NewGuid(),
    ShippedAt = DateTime.Today,
    Status = ShipmentStatus.Shipped
}));

app.MapPut("/api/shipments/{id}/status", (ShipmentsController shipmentsController) => shipmentsController.UpdateShipmentStatus(Guid.NewGuid(), ShipmentStatus.Pending));


app.Run();