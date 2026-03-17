namespace FulfillmentCenter.DTOs.Requests;

public record RequestProductDto
{
    public string Name { get; set; }
    public string SKU { get; set; }
    public decimal Weight { get; set; }
}