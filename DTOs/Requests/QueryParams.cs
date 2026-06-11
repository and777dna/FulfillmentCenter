namespace FulfillmentCenter.DTOs.Requests;

public record QueryParams
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public int Page { get; set; }
    public int PageSize { get; set; }
};