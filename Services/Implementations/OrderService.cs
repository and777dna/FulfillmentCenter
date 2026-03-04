using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Repositories.Implementations;
using FulfillmentCenter.Services.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class OrderService : IOrder
{
    private SqlOrderRepository _sqlOrderRepository;

    public OrderService(SqlOrderRepository sqlOrderRepository)
    {
        _sqlOrderRepository = sqlOrderRepository;
    }
    
    public void CreateOrder(Order order)
    {
        if (GetOrderById(order.Id) != null)//TODO: to fix this "Expression is always true according to nullable reference types' annotations"
        {
            _sqlOrderRepository.Create(order);
        }
    }

    public void CancelOrder(Guid orderId)
    {
        if (_sqlOrderRepository.Orders != null)
        {
            if (GetOrderById(orderId).Status == OrderStatus.Created || GetOrderById(orderId).Status == OrderStatus.Processing)
            {
                _sqlOrderRepository.Delete(orderId);
            }

            //GetOrderById(orderId).Status = OrderStatus.Cancelled;//TODO: to change to this status
        }
    }
    
    public Order GetOrderById(Guid orderId)
    {
        var orders = _sqlOrderRepository.Orders;
        
        var findedBook = SearchById(orderId, orders);
        return findedBook;
    }
    
    public Order SearchById(Guid orderId, List<Order> orders)
    {
        var findedOrder = orders.Find(order => order.Id == orderId);
        return findedOrder;
    }
    
    public void UpdateOrderStatus(OrderStatus orderStatus, Guid Id)
    {
        _sqlOrderRepository.UpdateOrder(orderStatus, Id, (order, status) => { order.Status = status;});
    }

}