using FulfillmentCenter;
using FulfillmentCenter.Controllers;
using FulfillmentCenter.Data;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Implementations;
using FulfillmentCenter.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.Run();


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