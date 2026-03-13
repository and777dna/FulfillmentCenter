using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Services.Implementations;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductsController(IProductService productService) : ControllerBase
{
    private IProductService _productService = productService;
    
    
    [HttpGet]
    public IActionResult GetProducts()
    {
        List<Product> products = _productService.GetProducts().Result;
        List<ResponseProductDto> productsDtos = products.Select(product => new ResponseProductDto
        {
            Name = product.Name,
            SKU = product.SKU,
            Weight = product.Weight
        }).ToList();
        return Ok(productsDtos);
    }

    [HttpPost]
    public IActionResult AddProduct([FromBody] RequestProductDto productDto)
    {
        _productService.CreateProduct(productDto);
        return Ok("New product has been added.");
    }

}