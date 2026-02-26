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