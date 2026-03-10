using FulfillmentCenter;
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


var app = builder.Build();

var connectionString = builder.Configuration.GetConnectionString("ConnectionStrings");
builder.Services.AddDbContext<FulfillmentCenDbContext>(options => options.UseSqlServer(connectionString));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.Run();


builder.Services.AddSingleton<IInventoryService, InventoryService>();
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IShipmentService, ShipmentService>();

builder.Services.AddSingleton<IFulfillmentCenterRepository, SqlFulfillmentCenterRepository>();
builder.Services.AddSingleton<IInventoryRepository, SqlInventoryRepository>();
builder.Services.AddSingleton<IOrderItemRepository, SqlOrderItemRepository>();
builder.Services.AddSingleton<IOrderRepository, SqlOrderRepository>();
builder.Services.AddSingleton<IProductRepository, SqlProductRepository>();
builder.Services.AddSingleton<IShipmentRepository, SqlShipmentRepository>();

builder.Services.AddSingleton<InventoryController, InventoryController>();
builder.Services.AddSingleton<OrdersController, OrdersController>();
builder.Services.AddSingleton<ProductsController, ProductsController>();
builder.Services.AddSingleton<ShipmentsController, ShipmentsController>();

builder.Services.AddSingleton<DbContext, FulfillmentCenDbContext>();//TODO: to check if this is okay, because i need to register interface first
    

var container = new Container();

container.Register<IInventoryService, InventoryService>();
container.Register<IOrderService, OrderService>();
container.Register<IProductService, ProductService>();
container.Register<IShipmentService, ShipmentService>();

container.Register<IFulfillmentCenterRepository, SqlFulfillmentCenterRepository>();
container.Register<IInventoryRepository, SqlInventoryRepository>();
container.Register<IOrderItemRepository, SqlOrderItemRepository>();
container.Register<IOrderRepository, SqlOrderRepository>();
container.Register<IProductRepository, SqlProductRepository>();
container.Register<IShipmentRepository, SqlShipmentRepository>();

container.Register<InventoryController, InventoryController>();
container.Register<OrdersController, OrdersController>();
container.Register<ProductsController, ProductsController>();
container.Register<ShipmentsController, ShipmentsController>();

container.Register<FulfillmentCenDbContext, FulfillmentCenDbContext>();