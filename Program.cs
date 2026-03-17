using FulfillmentCenter.Controllers;
using FulfillmentCenter.Data;
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
builder.Services.AddDbContext<FulfillmentCenDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddControllers();

builder.Services.AddTransient<IInventoryService, InventoryService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IProductService, ProductService>();

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

builder.Services.AddScoped<DbContext, FulfillmentCenDbContext>();//TODO: to check if this is okay, because i need to register interface first


var app = builder.Build();

//DbContextOptions

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/testing", () => "TESTING");
app.MapGet("/api/products", (IProductService productService) => productService.GetProducts());


/*app.MapGet("/api/orders/{id}", () =>
{
    
});*/


app.Run();