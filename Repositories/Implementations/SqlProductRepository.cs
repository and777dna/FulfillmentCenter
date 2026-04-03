using FulfillmentCenter.Data;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlProductRepository : IProductRepository
{
    private FulfillmentCenDbContext _context;

    public SqlProductRepository(FulfillmentCenDbContext context)
    {
        _context = context;
    }

    public async Task Create(Product product)
    {
        try
        {
            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async void Delete(Guid id)
    {
        var productToDelete = await _context.Product.FirstOrDefaultAsync(product => product.Id == id);
        _context.Product.Remove(productToDelete);
        //TODO: to return Result
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> Read()
    {
        List<Product> products;
        try
        {
            products = await _context.Product.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return products;
    }
}