using Xunit;
using NSubstitute;
using ProductManager.Api.DTOs.Products;
using ProductManager.Api.Models;
using ProductManager.Api.Repositories.Interfaces;
using ProductManager.Api.Services;

namespace ProductManager.Tests;

public class ProductServiceTests
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _categoryRepository = Substitute.For<ICategoryRepository>();

        _service = new ProductService(
            _productRepository,
            _categoryRepository);
    }

    [Fact]
    public async Task GetAllAsync_WhenProductsExist_ReturnsProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Keyboard",
                Price = 799,
                StockQuantity = 10,
                CategoryId = 2,
                Category = new Category
                {
                    Id = 2,
                    Name = "Electronics"
                }
            }
        };

        _productRepository
            .GetAllWithCategoryAsync()
            .Returns(products);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Single(result);

        var product = result.First();

        Assert.Equal("Keyboard", product.Name);
        Assert.Equal(799, product.Price);
        Assert.Equal("Electronics", product.CategoryName);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ReturnsProduct()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Mouse",
            Price = 499,
            StockQuantity = 5,
            CategoryId = 2,
            Category = new Category
            {
                Id = 2,
                Name = "Electronics"
            }
        };

        _productRepository
            .GetByIdWithCategoryAsync(1)
            .Returns(product);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Mouse", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductDoesNotExist_ReturnsNull()
    {
        // Arrange
        _productRepository
            .GetByIdWithCategoryAsync(999)
            .Returns((Product?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_WithValidCategory_CreatesProduct()
    {
        // Arrange
        var category = new Category
        {
            Id = 2,
            Name = "Electronics"
        };

        var dto = new CreateProductDto
        {
            Name = "Laptop",
            Price = 9999,
            StockQuantity = 3,
            CategoryId = 2
        };

        _categoryRepository
            .GetByIdAsync(2)
            .Returns(category);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.Equal("Laptop", result.Name);
        Assert.Equal(9999, result.Price);
        Assert.Equal(2, result.CategoryId);

        await _productRepository
            .Received(1)
            .AddAsync(Arg.Any<Product>());

        await _productRepository
            .Received(1)
            .SaveChangesAsync();
    }

    [Fact]
    public async Task CreateAsync_WithInvalidCategory_ThrowsArgumentException()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Laptop",
            Price = 9999,
            StockQuantity = 3,
            CategoryId = 999
        };

        _categoryRepository
            .GetByIdAsync(999)
            .Returns((Category?)null);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateAsync(dto));

        // Assert
        Assert.Equal(
            "Category does not exist.",
            exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenProductDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var dto = new UpdateProductDto
        {
            Name = "Updated Product",
            Price = 1000,
            StockQuantity = 4,
            CategoryId = 2
        };

        _productRepository
            .GetByIdAsync(999)
            .Returns((Product?)null);

        // Act
        var result = await _service.UpdateAsync(999, dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductExists_DeletesProduct()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Keyboard",
            Price = 799,
            StockQuantity = 10,
            CategoryId = 2
        };

        _productRepository
            .GetByIdAsync(1)
            .Returns(product);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);

        _productRepository
            .Received(1)
            .Delete(product);

        await _productRepository
            .Received(1)
            .SaveChangesAsync();
    }
}