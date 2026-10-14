using ProductManager.Api.DTOs.Categories;
using ProductManager.Api.Models;
using ProductManager.Api.Repositories.Interfaces;
using ProductManager.Api.Services.Interfaces;

namespace ProductManager.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(
        ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories =
            await _categoryRepository.GetAllAsync();

        return categories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        });
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category =
            await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            return null;
        }

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<CategoryDto> CreateAsync(
        CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name
        };

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateCategoryDto dto)
    {
        var category =
            await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            return false;
        }

        category.Name = dto.Name;

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category =
            await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            return false;
        }

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();

        return true;
    }
}