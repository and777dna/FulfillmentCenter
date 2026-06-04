namespace FulfillmentCenter.DTOs.Requests;

public record OrderFilterParams
{
    public DateTime? fromDate { get; set; }
    public DateTime? toDate { get; set; }
};