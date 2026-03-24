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
    ServerVersion.AutoDetect(connectionString)));
//builder.Services.AddDbContext<FulfillmentCenDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddControllers();

builder.Services.AddTransient<IInventoryService, InventoryService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IFulfillmentCenterService, FulfillmentCenterService>();

builder.Services.AddTransient<IShipmentService, ShipmentService>();

builder.Services.AddTransient<IFulfillmentCenterRepository, SqlFulfillmentCenterRepository>();
builder.Services.AddTransient<IInventoryRepository, SqlInventoryRepository>();
builder.Services.AddTransient<IOrderItemRepository, SqlOrderItemRepository>();
builder.Services.AddTransient<IOrderRepository, SqlOrderRepository>();
builder.Services.AddTransient<IProductRepository, SqlProductRepository>();
builder.Services.AddTransient<IShipmentRepository, SqlShipmentRepository>();

builder.Services.AddTransient<InventoryController, InventoryController>();
builder.Services.AddTransient<OrdersController, OrdersController>();
builder.Services.AddTransient<ProductsController, ProductsController>();
builder.Services.AddTransient<ShipmentsController, ShipmentsController>();

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
app.MapGet("/api/products", (ProductsController productController) => productController.GetProducts());

app.MapPost("/api/products", (ProductsController productController) => productController.AddProduct(new RequestProductDto
{
    Name = "a",
    SKU = "2",
    Weight = 0
}));


app.MapGet("/api/inventory/{centerId}", (InventoryController inventoryController) => inventoryController.InventoryRemaining(Guid.NewGuid()));

app.MapPost("/api/inventory", (InventoryController inventoryController) => inventoryController.AddStock(new RequestInventoryDto()
{//TODO: this one is workable
    DistributionCenterId = Guid.NewGuid(),
    ProductId = Guid.NewGuid(),
    Quantity = 3
}));

app.MapPost("/api/orders", (OrdersController ordersController) => ordersController.CreateOrder(new RequestOrderDto()
{
    CustomerName = "ANDREI",
    DeliveryAddress = "Pražská 636/38b, 642 00 Brno",
    Status = OrderStatus.Created
}));

//TODO: to fix this
app.MapPut("/api/orders/{id}", (OrdersController ordersController) => ordersController.ChangeOrderStatus(Guid.NewGuid(), OrderStatus.Processing));

//[FromRoute] Guid id  //TODO: to see if i need to fix it to make it "[FromRoute] Guid id"
app.MapGet("/api/orders/{id}/status", (OrdersController ordersController) => ordersController.GetOrder(Guid.NewGuid()));


app.MapPost("/api/shipments", (ShipmentsController shipmentsController) => shipmentsController.CreateShipment(new RequestShipmentDto()
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