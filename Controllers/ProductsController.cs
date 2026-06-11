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
    public async Task<IActionResult> GetProducts([FromQuery] int page, [FromQuery] int pageSize)
    {
        
        
        //TODO: to make DTO here from repoistory
        List<Product> products = await _productService.GetProducts(page,pageSize);
        /*List<ResponseProductDto> productsDtos = products.Select(product => new ResponseProductDto
        {
            Name = product.Name,
            SKU = product.SKU,
            Weight = product.Weight
        }).ToList();*/
        PagedResult<Product> pagedResult = new PagedResult<Product>()
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = products.Count,
            Items = products,
            TotalPages = (int)Math.Ceiling(
                (double)products.Count / pageSize)
        };
        return Ok(pagedResult);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] RequestProductDto productDto)
    {
        await _productService.CreateProduct(productDto);
        return CreatedAtAction(nameof(AddProduct), new {productDto.SKU});
    }

}