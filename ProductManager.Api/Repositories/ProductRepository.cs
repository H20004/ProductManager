using Microsoft.EntityFrameworkCore;
using ProductManager.Api.Data;
using ProductManager.Api.Models;
using ProductManager.Api.Repositories.Interfaces;

namespace ProductManager.Api.Repositories;

public class ProductRepository
    : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdWithCategoryAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}