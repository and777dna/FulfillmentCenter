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
        PagedResultProduct<Product> productsPagedResult = new PagedResultProduct<Product>()
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = products.Count,
            Products = products,
            TotalPages = (int)Math.Ceiling(
                (double)products.Count / pageSize)
        };
        return Ok(productsPagedResult);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] RequestProductDto productDto)
    {
        await _productService.CreateProduct(productDto);
        return CreatedAtAction(nameof(AddProduct), new {productDto.SKU});
    }

}