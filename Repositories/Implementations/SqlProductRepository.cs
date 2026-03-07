using FulfillmentCenter.Data;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlProductRepository : IProductRepository
{
    private FulfillmentCenDbContext _context;

    public SqlProductRepository()
    {
        _context = new FulfillmentCenDbContext();
    }

    public void Create(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var productToDelete = _context.Products.FirstOrDefault(product => product.Id == id);
        _context.Products.Remove(productToDelete);
        //TODO: to return Result
        _context.SaveChanges();
    }

    public List<Product> Read()
    {
        List<Product> products = _context.Products.ToList();
        return products;
    }

    public void UpdateProduct()
    {
        
    }
}