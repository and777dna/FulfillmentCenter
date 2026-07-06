using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Entities.Operation.Interfaces;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;
using FulfillmentCenter.Services.MapperDto.Interfaces;

namespace FulfillmentCenter.Services.Implementations;

public class InventoryService(
    IInventoryRepository inventoryRepository, 
    IMapper<Inventory, ResponseInventoryDto> inventoryMapper) : IInventoryService
{
    private IInventoryRepository _inventoryRepository = inventoryRepository;
    
    public async Task AddStock(RequestInventoryDto inventoryDto, Guid fulfillmentCenterId)
    {
        var productOnFulfillmentCenter =
            await ProductExistsOnCenter(fulfillmentCenterId,
                inventoryDto.ProductId); //TODO: to add then number of products if exists


        var inventory = new Inventory
        {
            ProductId = inventoryDto.ProductId,
            Quantity = inventoryDto.Quantity,
            DistributionCenterId = fulfillmentCenterId,
        };
        if (productOnFulfillmentCenter)
        {
            await _inventoryRepository.UpdateInventoryAsync(inventory);
        }
        else
        {
            inventory.Id = Guid.NewGuid();
            //TODO: to check if SKU is unique, because SKU is the PK
            await _inventoryRepository.AddAsync(inventory);
        }
    }

    private async Task<bool> ProductExistsOnCenter(Guid fulfillmentCenterId, Guid productId)
    {
        var inventories = await _inventoryRepository.GetAllAsync();
        var productOnFulfilCen = inventories.FirstOrDefault(inventory => inventory.DistributionCenterId == fulfillmentCenterId && inventory.ProductId == productId);
        return productOnFulfilCen != null;
    }
    
    public async Task<PagedResult<ResponseInventoryDto>> RemainingsOnTheFulfillmentCenter(Guid centerId, QueryParams queryParams)
    {//TODO: to filter by ID first, only afterwards to apply pagination
        var inventories = await _inventoryRepository.ReadAsync(queryParams);
        var findInventoriesFromCenter = inventories.FindAll(inventory => inventory.DistributionCenterId == centerId);
        var findInventoriesFromCenterDto = inventoryMapper.ToDto(findInventoriesFromCenter);
        var pagedResult = inventoryMapper.ToPagedResult(queryParams.Page, queryParams.PageSize, findInventoriesFromCenterDto);
            
        return pagedResult;
    }

    public async Task UpdateInventoryProduct(Guid productId, IOperation<Inventory> operation, Guid centerId)
    {//TODO: to implement IRepository to fix this duplicity
        var remainingsOnTheFulfillmentCenter = await RemainingsOnTheFulfillmentCenter(centerId, new QueryParams { PageSize = 50, Page = 1 });

        var currentInventory = remainingsOnTheFulfillmentCenter.Items
            .FirstOrDefault(inventory => inventory.ProductId == productId);
        if (currentInventory == null)
        {
            throw new ArgumentNullException(nameof(currentInventory),
                "inventory with such productId doesnt exist on the given distribution center");
        }

        var inventory = new Inventory
        {
            ProductId = currentInventory.ProductId,
            Quantity = currentInventory.Quantity,
            DistributionCenterId = centerId
        };
        var quantityBefore = inventory.Quantity;

        operation.Apply(inventory);

        var delta = inventory.Quantity - quantityBefore;
        await _inventoryRepository.UpdateInventoryQuantityAsync(new UpdateInventoryDto
        {
            ProductId = productId,
            Quantity = delta
        });
    }
}