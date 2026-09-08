using FulfillmentCenter.Data;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Filter;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlProductRepository(FulfillmentCenterDbContext context, ILogger<SqlProductRepository> logger)
    : IProductRepository
{
    public async Task CreateAsync(Product product)
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

    public async Task DeleteAsync(Guid id)
    {
        var productToDelete = await context.Products.FirstOrDefaultAsync(product => product.Id == id);
        if(productToDelete == null)
        {
            throw new ArgumentNullException(nameof(id), "no Product was found");
        }
        productToDelete.IsDeleted = true;
        //TODO: to return Result
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to delete a product.");
            throw;
        }
    }

    public async Task<List<Product>> ReadAsync()
    {
        Console.WriteLine(context.Database.GetConnectionString());
        List<Product> products;
        try
        {
            products = await context.Products.OrderBy(p => p.Id).ToListAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to read products");
            throw;
        }
        return products;
    }
    public async Task<List<Product>> ReadAsync(QueryParams productsQueryParams)
    {
        int page = productsQueryParams.Page;
        int pageSize = productsQueryParams.PageSize;
        
        var filter = new ProductFilterBuilder(context.Products)
            .FilterWeight(productsQueryParams.FromWeight, productsQueryParams.ToWeight).Build();
        
        Console.WriteLine(context.Database.GetConnectionString());
        List<Product> products;
        try
        {
            products = await filter.Skip((page - 1) * pageSize)
                .Take(pageSize).OrderBy(p => p.Id).ToListAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to read products");
            throw;
        }
        return products;
    }
}