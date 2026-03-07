namespace FulfillmentCenter.DTOs.Responses;

public class ResponseProductDto
{
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;  // артикул
    public decimal Weight { get; set; }
}