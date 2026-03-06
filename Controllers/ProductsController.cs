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
    public List<Product> GetProducts()
    {
        List<Product> products = _productService.GetProducts();
        return products;
    }

    [HttpPost]
    public void AddProduct(Product product)
    {
        _productService.CreateProduct(product);
    }

}