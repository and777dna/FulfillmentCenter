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
    public List<ResponseProductDto> GetProducts()
    {
        List<Product> products = _productService.GetProducts();
        List<ResponseProductDto> productsDtos = products.Select(product => new ResponseProductDto
        {
            Name = product.Name,
            SKU = product.SKU,
            Weight = product.Weight
        }).ToList();
        return productsDtos;
    }

    [HttpPost]
    public void AddProduct(RequestProductDto productDto)
    {
        _productService.CreateProduct(productDto);
    }

}