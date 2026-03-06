using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[Route("/api/products")]
public class ProductsController : Controller
{
    private ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet]
    public List<RequestProductDto> GetProducts()
    {
        List<Product> products = _productService.GetProducts();
        List<RequestProductDto> productsDtos = products.Select(product => new RequestProductDto
        {
            Name = product.Name,
            SKU = product.SKU,
            Weight = product.Weight
        }).ToList();
        return productsDtos;
    }

    [HttpPost]
    public void AddProduct(Product product)
    {
        _productService.CreateProduct(product);
    }

}