using ProductManager.Api.DTOs.Products;
using ProductManager.Api.Models;
using ProductManager.Api.Repositories.Interfaces;
using ProductManager.Api.Services.Interfaces;

namespace ProductManager.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products =
            await _productRepository.GetAllWithCategoryAsync();

        return products.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty
        });
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product =
            await _productRepository.GetByIdWithCategoryAsync(id);

        if (product == null)
        {
            return null;
        }

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty
        };
    }

    public async Task<ProductDto> CreateAsync(
        CreateProductDto dto)
    {
        var category =
            await _categoryRepository.GetByIdAsync(dto.CategoryId);

        if (category == null)
        {
            throw new ArgumentException(
                "Category does not exist.");
        }

        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId
        };

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            CategoryName = category.Name
        };
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateProductDto dto)
    {
        var product =
            await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return false;
        }

        var category =
            await _categoryRepository.GetByIdAsync(dto.CategoryId);

        if (category == null)
        {
            throw new ArgumentException(
                "Category does not exist.");
        }

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;
        product.CategoryId = dto.CategoryId;

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product =
            await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return false;
        }

        _productRepository.Delete(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }
}