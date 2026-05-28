namespace FulfillmentCenter.DTOs.Responses;

public record PagedResultProduct<Product>
{
    public IEnumerable<Product> Products { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; } //=>
        //(int)Math.Ceiling((double)TotalCount / PageSize);
}