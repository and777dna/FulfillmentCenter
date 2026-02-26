namespace FulfillmentCenter.DTOs.Requests;

public class RequestProductDto
{
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;  // артикул
    public decimal Weight { get; set; }
}