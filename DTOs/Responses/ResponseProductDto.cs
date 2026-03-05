namespace FulfillmentCenter.DTOs.Responses;

public class ResponseProductDto
{
    //TODO review: Unlike in the request DTO, Name and SKU here have no = string.Empty default and are not marked string?. This will produce compiler warnings (non-nullable reference type uninitialized) and could result in null values being serialized.
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string SKU { get; set; }
    //TODO review: Weight is missing from the response
}