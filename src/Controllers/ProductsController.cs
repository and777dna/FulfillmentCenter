using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] decimal? fromWeight, [FromQuery] decimal? toWeight, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var productsQueryParams = new QueryParams
        {
            FromWeight = fromWeight,
            ToWeight = toWeight,
            
            Page = page,
            PageSize = pageSize
        };
        
        var pagedResult = await productService.GetProducts(productsQueryParams);
        return Ok(pagedResult);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] RequestProductDto productDto)
    {
        await productService.CreateProduct(productDto);
        return CreatedAtAction(nameof(AddProduct), new {productDto.SKU});
    }

}