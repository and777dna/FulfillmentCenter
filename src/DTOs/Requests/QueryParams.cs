namespace FulfillmentCenter.DTOs.Requests;

public record QueryParams
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    
    public decimal? FromWeight { get; set; }
    public decimal? ToWeight { get; set; }

    public int Page { get; set; }
    public int PageSize { get; set; }
};