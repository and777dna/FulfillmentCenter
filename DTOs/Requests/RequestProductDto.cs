using FulfillmentCenter.Entities;

namespace FulfillmentCenter.DTOs.Requests;

public class RequestProductDto
{
    public string Name { get; set; }
    public string SKU { get; set; }
    public decimal Weight { get; set; }
}