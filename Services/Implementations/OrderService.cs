using System.Transactions;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;
using FulfillmentCenter.Services.MapperDto.Interfaces;
using FulfillmentCenter.Services.UpdateOrderStatus;
using FulfillmentCenter.Strategies.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class OrderService(
    IOrderRepository orderRepository,
    ICacheService cache,
    IShipmentAssignmentStrategy shipmentAssignmentStrategy,
    IMapper<Order, ResponseOrderDto> orderMapper,
    IUnitOfWork unitOfWork,
    InventoryService inventoryService)
    : IOrderService
{
    private Lazy<Task<List<Order>>> _orders;
    
    private OrderHandlerFactory _orderHandlerFactory = new OrderHandlerFactory();
    
    public async Task CreateOrder(RequestOrderDto orderDto, string idempotencyKey, RequestOrderItemDto orderItemDto)
    {
        if (cache.TryGet<Guid>(idempotencyKey, out var cachedOrderId))
        {
            return;
        }
        
        if (orderDto.Status != OrderStatus.Created)
        {
            throw new ArgumentException("first status of order should be Created");
        }
        Order order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = orderDto.CustomerId,
            DeliveryAddress = orderDto.DeliveryAddress,
            Status = OrderStatus.Created
            //TODO: to add shippment here, by finding it in db
        };

        var findCenterId = await shipmentAssignmentStrategy.SelectDistributionCenter(orderItemDto.ProductId, orderItemDto.Quantity);
        
        //TODO: to enable differenct level of isolation to make it workable
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        OrderItem orderItem = new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            PricePerUnit = orderItemDto.PricePerUnit,
            ProductId = orderItemDto.ProductId,
            Quantity = orderItemDto.Quantity
        };
        
        await orderRepository.AddAsync(order);
        //await orderItemService.AddOrderItemToOrder(orderItemDto, findCenterId);
        order.Items.Add(orderItem);
        await inventoryService.UpdateInventoryProduct(orderItemDto.ProductId, orderItemDto.Quantity, findCenterId);
        cache.Set(idempotencyKey, order.Id, TimeSpan.FromMinutes(10));
        
        scope.Complete();
    }

    public async Task CancelOrder(Guid orderId)
    {
        var orderToCancelStatus = (await GetOrderById(orderId)).Status;
            if (orderToCancelStatus == OrderStatus.Created || orderToCancelStatus == OrderStatus.Processing)
            {
                var service = _orderHandlerFactory.GetHandler(orderToCancelStatus);
                await service.HandleAsync(orderId);
            }
            //GetOrderById(orderId).Status = OrderStatus.Cancelled;//TODO: to change to this status
    }
    
    public async Task<ResponseOrderDto> GetOrderById(Guid orderId)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        var orderDto = orderMapper.ToDto(order);
        /*new ResponseOrderDto
           {
               CustomerId = order.CustomerId,
               DeliveryAddress = order.DeliveryAddress,
               CreatedAt = order.CreatedAt,
               Status = order.Status
           }*/
        return orderDto;
    }

    public async Task<PagedResult<ResponseOrderDto>> GetOrders(QueryParams queryParams)
    {
        var pagedOrders = await orderRepository.ReadAsync(queryParams);
        var orderDtos = orderMapper.ToDto(pagedOrders);
        var pagedResult = orderMapper.ToPagedResult(queryParams.Page, queryParams.PageSize, orderDtos); 
        return pagedResult;
    }

    public async Task AddOrderItemToOrder(Guid orderId, RequestOrderItemDto orderItemDto, Guid centerId)
    {
        OrderItem orderItem = new OrderItem
        {
            Id = Guid.NewGuid(),
            PricePerUnit = orderItemDto.PricePerUnit,
            ProductId = orderItemDto.ProductId,
            Quantity = orderItemDto.Quantity
        };
        
        var order = await orderRepository.GetOrderWithItemsAsync(orderId);
        order.Items.Add(orderItem);
        await inventoryService.UpdateInventoryProduct(orderItemDto.ProductId, orderItemDto.Quantity, centerId);
        await unitOfWork.SaveTransactionAsync();
    }
}