using System.ComponentModel.DataAnnotations;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;
using FulfillmentCenter.Services.Implementations;
using Moq;
using Guid = System.Guid;

namespace FulfillmentCenter.FulfillmentCenter.Tests.Services;

public class ProductsServicesTests
{
    [Fact]
    public async Task GetProducts_ShouldReturnOkWithMappedDtos()
    {
        var repositoryMock = new Mock<IProductRepository>();
        
        var products = new List<Product>
        {
            new Product
            {
                Name = "Phone",
                SKU = "PH123",
                Weight = 200
            },
            new Product
            {
                Name = "Laptop",
                SKU = "LP456",
                Weight = 1500
            }
        };
        
        repositoryMock
            .Setup(service => service.ReadAsync())
            .ReturnsAsync(products);
        
        var service = new ProductService(repositoryMock.Object);
        
        var result = await service.GetProducts(1, 20);
        
        // Assert
        Assert.Equal(2, result.Count);

        Assert.Equal("Phone", result[0].Name);

        repositoryMock.Verify(r => r.ReadAsync(), Times.Once);
    }
    
    [Fact]
    public async Task CreateProduct_WhenSkuIsUnique_ShouldCreateProduct()
    {
        var repositoryMock = new Mock<IProductRepository>();
        var requestDto = new RequestProductDto { Name = "Laptop", SKU = "LP456", Weight = 1500 };

        repositoryMock
            .Setup(r => r.ReadAsync())
            .ReturnsAsync(new List<Product>());
        
        var service = new ProductService(repositoryMock.Object);
        
        await service.CreateProduct(requestDto);

        repositoryMock.Verify(r => r.ReadAsync(), Times.Once);
        
        repositoryMock.Verify(r => r.CreateAsync(It.Is<Product>(p =>
            p.Name == requestDto.Name &&
            p.SKU == requestDto.SKU &&
            p.Weight == requestDto.Weight &&
            p.Id != Guid.Empty
        )), Times.Once);
    }

    /*Если в Read() уже есть продукт с тем же SKU -
     ожидается InvalidOperationException с ожидаемым текстом.*/
    [Fact]
    public async Task CreateProduct_WhenSkuAlreadyExists_ShouldThrowInvalidOperationException()
    {
        var repositoryMock = new Mock<IProductRepository>();
        var products = new List<Product>
        {
            new Product
            {
                Name = "Phone",
                SKU = "PH123",
                Weight = 200
            },
            new Product
            {
                Name = "Laptop",
                SKU = "LP456",
                Weight = 1500
            }
        };
        var requestDto = new RequestProductDto { Name = "Laptop", SKU = "LP456", Weight = 1500 };
        repositoryMock.Setup(r => r.ReadAsync()).ReturnsAsync(products);
        var service = new ProductService(repositoryMock.Object);
        var exception = await Assert.ThrowsAsync<InvalidOperationException> (
            () => service.CreateProduct(requestDto));
        Assert.Equal("Запись с таким SKU уже существует в базе данных.",
            exception.Message);
        
        repositoryMock.Verify(r => r.ReadAsync(), Times.Once);
        repositoryMock.Verify(
            r => r.CreateAsync(It.IsAny<Product>()),
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

        var service = new ProductService(repositoryMock.Object);

        var result = await service.FindProduct(productId);

        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        Assert.Equal("Laptop", result.Name);
        Assert.Equal("LP456", result.SKU);
        
        repositoryMock.Verify(r => r.ReadAsync(), Times.Once);
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
        repositoryMock.Setup(r => r.ReadAsync()).ReturnsAsync(products);
        var service = new ProductService(repositoryMock.Object);

        await Assert.ThrowsAsync<ValidationException>(() => service.FindProduct(Guid.NewGuid()));
        repositoryMock.Verify(r => r.ReadAsync(), Times.Once);
    }
}