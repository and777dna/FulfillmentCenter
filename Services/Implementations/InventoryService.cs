using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Interfaces;
using FulfillmentCenter.Services.MapperDto.Implementations;

namespace FulfillmentCenter.Services.Implementations;

public class InventoryService(
    IInventoryRepository inventoryRepository,
    IFulfillmentCenterRepository fulfillmentCenterRepositor,
    IFulfillmentCenterService fulfillmentCenterService,
    IProductService productService, InventoryMapper inventoryMapper) : IInventoryService
{
    private IInventoryRepository _inventoryRepository = inventoryRepository;


    private IFulfillmentCenterRepository _fulfillmentCenterRepositor = fulfillmentCenterRepositor;
    private IFulfillmentCenterService _fulfillmentCenterService = fulfillmentCenterService;
    private IProductService _productService = productService;
    

    public async Task AddStock(RequestInventoryDto inventoryDto, Guid fulfillmentCenterId)
    {
        //TODO: if "fulfillmentCenterId" exist -> should be BOOL THIS ONE to delete?????
        //var fulfillmentCenter = await FindProduct(fulfillmentCenterId, inventoryDto.ProductId);
        //TODO: if "productId" exist -> should be BOOL 
        var productOnFulfillmentCenter =
            await FindProduct(fulfillmentCenterId,
                inventoryDto.ProductId); //TODO: to add then number of products if exists


        Inventory inventory = new Inventory
        {
            Id = Guid.NewGuid(),
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
            //TODO: to check if SKU is unique, because SKU is the PK
            await _inventoryRepository.CreateAsync(inventory);
        }
    }

    private async Task<bool> FindProduct(Guid fulfillmentCenterId, Guid productId)
    {
        var inventories = await _inventoryRepository.ReadAsync();
        bool productWasFound = false;
        var productOnFulfilCen = inventories.FirstOrDefault(inventory =>
        {
            return inventory.DistributionCenterId == fulfillmentCenterId && inventory.ProductId == productId;
        });
        if (productOnFulfilCen == null)
        {
            return false;
        }

        return true;
    }
    
    public async Task<PagedResult<ResponseInventoryDto>> RemainingsOnTheFulfillmentCenter(Guid centerId, QueryParams queryParams)
    {
        var inventories = await _inventoryRepository.ReadAsync(queryParams);
        var findInventoriesFromCenter = inventories.FindAll(inventory => inventory.DistributionCenterId == centerId);
        if (findInventoriesFromCenter.Count > 0)
        {
            var findInventoriesFromCenterDto = inventoryMapper.ToDto(findInventoriesFromCenter);
            var pagedResult = inventoryMapper.ToPagedResult(queryParams.Page, queryParams.PageSize, findInventoriesFromCenterDto);
            
            return pagedResult;
        }

        throw new ValidationException();
    }

    public Dictionary<Guid, int> ReturnProductAmount(ICollection<Inventory> inventories)
    {
        Dictionary<Guid, int> openWith = new Dictionary<Guid, int>();
        foreach (var product in inventories)
        {
            openWith.Add(product.Id, product.Quantity);
        }

        return openWith;
    }

    public async Task UpdateInventoryProduct(Guid productId, int quantity, Guid centerId)
    {
        var updatedInventory = new UpdateInventoryDto
        {
            ProductId = productId,
            Quantity = quantity
        };
        var remainingsOnTheFulfillmentCenter = await RemainingsOnTheFulfillmentCenter(centerId, new QueryParams{ PageSize = 50, Page = 1});

        if (CheckSufficientAmountOfInventory(remainingsOnTheFulfillmentCenter, updatedInventory))
        {
            await _inventoryRepository.UpdateInventoryQuantityAsync(updatedInventory);
        }
        else
        {
            throw new InvalidOperationException("not enough product on the given distribution center for the inventory");
        }

        
    }

    public bool CheckSufficientAmountOfInventory(PagedResult<ResponseInventoryDto> remainingsOnTheFulfillmentCenter, UpdateInventoryDto itemsToUpdate)
    {
        var findInventoryProduct = remainingsOnTheFulfillmentCenter.Items.FirstOrDefault(inventory => inventory.ProductId == itemsToUpdate.ProductId);
        if (findInventoryProduct == null)
        {
            throw new ArgumentNullException(nameof(findInventoryProduct), "inventory with such productId doesnt exist on the given distribution center");
        }

        if (findInventoryProduct.Quantity - itemsToUpdate.Quantity > 0)
        {
            return true;
        }

        return false;
    }
}