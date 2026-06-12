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
    private readonly IProductService _productService = productService;
    
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var productsQueryParams = new QueryParams
        {
            Page = page,
            PageSize = pageSize
        };
        
        //TODO: to make DTO here from repoistory
        var pagedResult = await _productService.GetProducts(productsQueryParams);
        return Ok(pagedResult);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] RequestProductDto productDto)
    {
        await _productService.CreateProduct(productDto);
        return CreatedAtAction(nameof(AddProduct), new {productDto.SKU});
    }

}