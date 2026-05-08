using FulfillmentCenter.Data;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlProductRepository(FulfillmentCenDbContext context, ILogger<SqlProductRepository> logger)
    : IProductRepository
{
    int page = 2;
    int pageSize = 50;

    public async Task Create(Product product)
    {
        try
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to create a product.");
            throw;
        }
    }

    public async Task Delete(Guid id)
    {
        var productToDelete = await context.Products.FirstOrDefaultAsync(product => product.Id == id);
        if(productToDelete == null)
        {
            throw new ArgumentNullException(nameof(id), "no Product was found");
        }
        productToDelete.IsDeleted = true;
        //TODO: to return Result
        await context.SaveChangesAsync();
    }

    public async Task<List<Product>> Read()
    {
        Console.WriteLine(context.Database.GetConnectionString());
        List<Product> products;
        try
        {
            products = await context.Products.Skip((page - 1) * pageSize)
                .Take(pageSize).OrderBy(p => p.Id).ToListAsync();
            products = await context.Products.OrderBy(p => p.Id).ToListAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to read products");
            throw;
        }
        return products;
    }
}