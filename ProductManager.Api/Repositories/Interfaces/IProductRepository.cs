using ProductManager.Api.Models;

namespace ProductManager.Api.Repositories.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetAllWithCategoryAsync();

    Task<Product?> GetByIdWithCategoryAsync(int id);
}