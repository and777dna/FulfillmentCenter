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
//DONE
app.MapPost("/api/shipments", (ShipmentsController shipmentsController) => shipmentsController.CreateShipment(new RequestShipmentDto
{
    DistributionCenterId = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),
    EstimatedDelivery = DateTime.Now,
    Id = Guid.NewGuid(),
    OrderId = Guid.Parse("619550e6-d4a8-4a17-8069-5203ad823c72"),
    ShippedAt = DateTime.Today,
    Status = ShipmentStatus.Shipped
}));
app.MapGet("/api/inventory/{centerId}", async (InventoryController inventoryController) =>
    await inventoryController.InventoryRemaining(Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e")));
//[FromRoute] Guid id  //TODO: to see if i need to fix it to make it "[FromRoute] Guid id"
//DONE
app.MapGet("/api/orders/{id}/status", (OrdersController ordersController) => 
    ordersController.GetOrder(Guid.Parse("619550e6-d4a8-4a17-8069-5203ad823c72")));
app.MapPut("/api/shipments/{id}/status", (ShipmentsController shipmentsController) =>//TODO: ShipmentStatus.Failed 
    shipmentsController.UpdateShipmentStatus(Guid.Parse("8a5c1f2a-7f4b-4c7f-bf1e-5b9b7a0d03a8"), ShipmentStatus.Cancelled));
//DONE
app.MapPost("/api/orders", (OrdersController ordersController) => ordersController.CreateOrder(new RequestOrderDto
{
    CustomerName = "IVAN",
    DeliveryAddress = "not prazska, 642 00 Brno",
    Status = OrderStatus.Created
}));//TODO: to reduce/add quantity of products from inventory(orderItem), to add relation to DistributionCenter
app.MapPut("/api/orders/{id}", (OrdersController ordersController) => 
    ordersController.ChangeOrderStatus(Guid.Parse("8a5c1f2a-7f4b-4c7f-bf1e-5b9b7a0d03a3"), OrderStatus.Processing));
//"statusCode": 204





app.MapPost("/api/inventory", (InventoryController inventoryController) => inventoryController.AddStock(new RequestInventoryDto
{//TODO: this one is TOUGH TOUGH TOUGH
    DistributionCenterId = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),
    ProductId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000"),
    Quantity = 8
}));





app.Run();