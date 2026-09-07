using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Implementations;
using FulfillmentCenter.Services.MapperDto.Interfaces;
using Moq;
using Guid = System.Guid;

namespace FulfillmentCenter.Tests.Services;

public class ProductsServicesTests
{
    [Fact]
    public async Task GetProducts_ListOfProducts_ShouldReturnMappedDtos()
    {
        var repositoryMock = new Mock<IRepository<Product>>();
        var mapperMock = new Mock<IMapper<Product, ResponseProductDto>>();
        
        var products = new List<Product>
        {
            new Product { Name = "Phone", SKU = "PH123", Weight = 200 },
            new Product { Name = "Laptop", SKU = "LP456", Weight = 1500 }
        };
        var productDtos = new List<ResponseProductDto>
        {
            new ResponseProductDto { Name = "Phone", SKU = "PH123", Weight = 200 },
            new ResponseProductDto { Name = "Laptop", SKU = "LP456", Weight = 1500 }
        };
        var expectedPagedResult = new PagedResult<ResponseProductDto>
        {
            Items = productDtos,
            Page = 1,
            PageSize = 10,
            TotalCount = 2
        };
        QueryParams productsQueryParams = new QueryParams() { Page = 1, PageSize = 10, };
        repositoryMock
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(products);
        
        mapperMock.Setup(mapper => mapper.ToDto(products)).Returns(productDtos);
        mapperMock.Setup(m => m.ToPagedResult(productsQueryParams.Page, 
                productsQueryParams.PageSize, 
                productDtos))
            .Returns(expectedPagedResult);
        
        var service = new ProductService(repositoryMock.Object, mapperMock.Object);
        
        var result = await service.GetProducts(productsQueryParams);
        
        Assert.NotNull(result);
        Assert.Equal(expectedPagedResult.TotalCount, result.TotalCount);
        Assert.Equal("Phone", result.Items[0].Name);

        repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        mapperMock.Verify(m => m.ToDto(products), Times.Once);
        mapperMock.Verify(m => m.ToPagedResult(productsQueryParams.Page, 
            productsQueryParams.PageSize, 
            productDtos), Times.Once);
    }
    
    [Fact]
    public async Task CreateProduct_WhenSkuIsUnique_ShouldCreateProduct()
    {
        var repositoryMock = new Mock<IRepository<Product>>();
        var mapperMock = new Mock<IMapper<Product, ResponseProductDto>>();
        var requestDto = new RequestProductDto { Name = "Laptop", SKU = "LP456", Weight = 1500 };

        repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Product>());
        
        var service = new ProductService(repositoryMock.Object, mapperMock.Object);
        
        await service.CreateProduct(requestDto);

        repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        
        repositoryMock.Verify(r => r.AddAsync(It.Is<Product>(p =>
            p.Name == requestDto.Name &&
            p.SKU == requestDto.SKU &&
            p.Weight == requestDto.Weight &&
            p.Id != Guid.Empty
        )), Times.Once);
    }
    
    [Fact]
    public async Task CreateProduct_WhenSkuAlreadyExists_ShouldThrowInvalidOperationException()
    {
        var repositoryMock = new Mock<IRepository<Product>>();
        var mapperMock = new Mock<IMapper<Product, ResponseProductDto>>();
        var products = new List<Product>
        {
            new Product { Name = "Phone", SKU = "PH123", Weight = 200 },
            new Product { Name = "Laptop", SKU = "LP456", Weight = 1500 }
        };
        var requestDto = new RequestProductDto { Name = "Laptop", SKU = "LP456", Weight = 1500 };
        repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
        var service = new ProductService(repositoryMock.Object, mapperMock.Object);
        var exception = await Assert.ThrowsAsync<InvalidOperationException> (
            () => service.CreateProduct(requestDto));
        Assert.Equal("Запись с таким SKU уже существует в базе данных.",
            exception.Message);
        
        repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Product>()),
            Times.Never);
    }

    [Fact]
    public async Task FindProduct_WhenProductExists_ShouldReturnProduct()
    {
        var repositoryMock = new Mock<IProductRepository>();
        var productId = Guid.NewGuid();
        var products = new List<Product>
        {
            new Product
            {
                Id = productId,
                Name = "Laptop",
                SKU = "LP456",
                Weight = 1500
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Phone",
                SKU = "PH123",
                Weight = 200
            }
        };

        repositoryMock.Setup(r => r.ReadAsync()).ReturnsAsync(products);

        /*//var service = new ProductService(repositoryMock.Object);

        //var result = await service.FindProduct(productId);

        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        Assert.Equal("Laptop", result.Name);
        Assert.Equal("LP456", result.SKU);
        
        repositoryMock.Verify(r => r.ReadAsync(), Times.Once);*/
    }

    [Fact]
    public async Task FindProduct_WhenProductDoesNotExist_ShouldThrowValidationException()
    {
        var repositoryMock = new Mock<IProductRepository>();
        var productId = Guid.NewGuid();
        var products = new List<Product>
        {
            new Product
            {
                Id = productId,
                Name = "Laptop",
                SKU = "LP456",
                Weight = 1500
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Phone",
                SKU = "PH123",
                Weight = 200
            }
        };
        /*repositoryMock.Setup(r => r.ReadAsync()).ReturnsAsync(products);
        var service = new ProductService(repositoryMock.Object);

        await Assert.ThrowsAsync<ValidationException>(() => service.FindProduct(Guid.NewGuid()));
        repositoryMock.Verify(r => r.ReadAsync(), Times.Once);*/
    }
}