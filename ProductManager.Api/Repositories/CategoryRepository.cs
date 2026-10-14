using ProductManager.Api.Data;
using ProductManager.Api.Models;
using ProductManager.Api.Repositories.Interfaces;

namespace ProductManager.Api.Repositories;

public class CategoryRepository
    : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context)
        : base(context)
    {
    }
}