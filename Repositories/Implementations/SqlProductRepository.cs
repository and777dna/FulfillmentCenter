using FulfillmentCenter.Data;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.FilterV2.Implementations;
using FulfillmentCenter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlProductRepository(FulfillmentCenDbContext context, ILogger<SqlProductRepository> logger)
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
        await context.SaveChangesAsync();
    }

    public async Task<List<Product>> ReadAsync()
    {
        Console.WriteLine(context.Database.GetConnectionString());
        List<Product> products;
        try
        {
            products = await context.Products.OrderBy(p => p.Id).ToListAsync();
            //products = await context.Products.OrderBy(p => p.Id).ToListAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "not possible to read products");
            throw;
        }
        return products;
    }
    public async Task<PagedResult<ResponseProductDto>> ReadAsync(QueryParams productsQueryParams)
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

        var responseProductDtos = products.Select(product =>
            new ResponseProductDto
            {
                Name = product.Name,
                SKU = product.SKU,
                Weight = product.Weight
            });

        PagedResult<ResponseProductDto> pagedResult = new PagedResult<ResponseProductDto>()
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = products.Count,
            TotalPages = products.Count / page,
            Items = responseProductDtos
        };
        return pagedResult;
    }
}