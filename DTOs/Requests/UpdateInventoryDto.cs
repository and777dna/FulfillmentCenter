namespace FulfillmentCenter.DTOs.Requests;

public record UpdateInventoryDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}