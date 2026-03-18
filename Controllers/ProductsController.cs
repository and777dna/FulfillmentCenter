using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;
    
    
    [HttpGet]
    public async Task<List<ResponseProductDto>> GetProducts()
    {
        List<Product> products = await _productService.GetProducts();
        List<ResponseProductDto> productsDtos = products.Select(product => new ResponseProductDto
        {
            Name = product.Name,
            SKU = product.SKU,
            Weight = product.Weight
        }).ToList();
        return productsDtos;
    }

    [HttpPost]
    public IActionResult AddProduct([FromBody] RequestProductDto productDto)
    {
        _productService.CreateProduct(productDto);
        return CreatedAtAction(nameof(AddProduct), new {productDto.SKU});
    }

}