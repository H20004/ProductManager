using ProductManager.Api.DTOs.Categories;

namespace ProductManager.Api.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();

    Task<CategoryDto?> GetByIdAsync(int id);

    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);

    Task<bool> UpdateAsync(int id, UpdateCategoryDto dto);

    Task<bool> DeleteAsync(int id);
}