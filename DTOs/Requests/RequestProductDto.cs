namespace FulfillmentCenter.DTOs.Requests;

public class RequestProductDto
{
    //TODO review: No data validation attributes ([Required], [MaxLength], [Range(min, max)] etc.). Name and SKU could be empty strings. Weight could be negative
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;  // артикул
    public decimal Weight { get; set; }
}