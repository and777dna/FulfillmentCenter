using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using FulfillmentCenter.Enums;

namespace FulfillmentCenter.DTOs.Requests;

public record RequestOrderDto
{
    [Required]
    public string CustomerName { get; set; }
    [Required]
    public string DeliveryAddress { get; set; }

    [Required] //public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateTime CreatedAt { get; set; } = DateTime.Today;
    //public DateTime CreatedAt { get; set; } = DateTime.Now;//DateTime.Parse("10-22-2015 12:10:15");//new DateTime(2015, 12, 25);//= DateTime.Now;
    [Required]
    public OrderStatus Status { get; set; }
}