using FulfillmentCenter.Data;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlProductRepository : IProductRepository
{
    private readonly FulfillmentCenDbContext _context;

    public SqlProductRepository(FulfillmentCenDbContext context)
    {
        _context = context;
    }

    public async Task Create(Product product)
    {
        try
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Delete(Guid id)
    {
        var productToDelete = await _context.Products.FirstOrDefaultAsync(product => product.Id == id);
        if(productToDelete == null)
        {
            throw new ArgumentNullException(nameof(id), "no Product was found");
        }
        _context.Products.Remove(productToDelete);
        //TODO: to return Result
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> Read()
    {
        List<Product> products;
        try
        {
            products = await _context.Products.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return products;
    }
}