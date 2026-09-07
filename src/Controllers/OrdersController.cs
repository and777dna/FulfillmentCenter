using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities.Operation.Implementations;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Services.Interfaces;
using FulfillmentCenter.Services.UpdateOrderStatus;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[ApiController]
[Route("/api/orders")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private OrderHandlerFactory _orderHandlerFactory = new OrderHandlerFactory();
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey,
        [FromBody] RequestOrderDto? orderDto)
    {
        if(idempotencyKey == null)return BadRequest("missing Idempotency-Key");
        if (orderDto != null)
        {
            await orderService.CreateOrder(orderDto, idempotencyKey, orderDto.orderItemDto);
            return Ok();
        }

        return BadRequest();
    }
    
    [HttpPut("{id}/status")]
    public async Task<IActionResult> ChangeOrderStatus([FromRoute] Guid id,[FromQuery] OrderStatus status)
    {
        if (!Enum.IsDefined(typeof(OrderStatus), status))
        {
            return BadRequest("Invalid order status");
        }

        var service = _orderHandlerFactory.GetHandler(status);
        await service.HandleAsync(id);
        
        return NoContent();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder([FromRoute] Guid id)
    {
        var orderDto = await orderService.GetOrderById(id);

        return Ok(
            orderDto
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var orderQueryParamsParams = new QueryParams
        {
            FromDate = fromDate,
            ToDate = toDate,
            
            Page = page,
            PageSize = pageSize
        };
        
        PagedResult<ResponseOrderDto> orders = await orderService.GetOrders(orderQueryParamsParams);
        return Ok(orders);
    }
    
    [HttpPost("add/{orderId}/items")]
    public async Task<IActionResult> AddOrderItem([FromRoute] Guid orderId, [FromBody] RequestOrderItemDto? orderItemDto, [FromRoute] Guid centerId)
    {
        if (orderItemDto == null) throw new ArgumentNullException(nameof(orderItemDto), "OrderItemDto is null");

        orderItemDto.Operation = new AddOrderItemOperation(orderItemDto.Quantity);
        await orderService.UpdateOrder(orderId, orderItemDto, centerId);

        return Ok();
    }
    
    [HttpPost("delete/{orderId}/items")]
    public async Task<IActionResult> DeleteOrderItem([FromRoute] Guid orderId, [FromBody] RequestOrderItemDto? orderItemDto, [FromRoute] Guid centerId)
    {
        if (orderItemDto == null) throw new ArgumentNullException(nameof(orderItemDto), "OrderItemDto is null");

        orderItemDto.Operation = new DeleteOrderItemOperation(orderItemDto.Quantity);
        await orderService.UpdateOrder(orderId, orderItemDto, centerId);

        return Ok();
    }
     
}