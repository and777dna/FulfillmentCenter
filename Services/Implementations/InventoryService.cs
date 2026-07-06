using System.ComponentModel.DataAnnotations;
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
    IFulfillmentCenterRepository fulfillmentCenterRepositor,
    IFulfillmentCenterService fulfillmentCenterService,
    IProductService productService, IMapper<Inventory, ResponseInventoryDto> inventoryMapper) : IInventoryService
{
    private IInventoryRepository _inventoryRepository = inventoryRepository;


    private IFulfillmentCenterRepository _fulfillmentCenterRepositor = fulfillmentCenterRepositor;
    private IFulfillmentCenterService _fulfillmentCenterService = fulfillmentCenterService;
    private IProductService _productService = productService;
    

    public async Task AddStock(RequestInventoryDto inventoryDto, Guid fulfillmentCenterId)
    {
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