using Microsoft.EntityFrameworkCore;
using MyRestApi.Data;
using MyRestApi.Models;
using MyRestApi.DTOs;

namespace MyRestApi.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateAsync(int id, ProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.Category = dto.Category;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Product>> SearchByNameAsync(string keyword)
        {
            return await _context.Products
                .Where(p => p.Name.Contains(keyword))
                .ToListAsync();
        }

        public async Task<List<Product>> GetByCategoryAsync(string category)
        {
            return await _context.Products
                .Where(p => p.Category == category)
                .ToListAsync();
        }

        public async Task<List<Product>> GetSortedByPriceAsync(bool descending)
        {
            return descending
                ? await _context.Products.OrderByDescending(p => p.Price).ToListAsync()
                : await _context.Products.OrderBy(p => p.Price).ToListAsync();
        }

        public async Task<bool> UpdateStockAsync(int id, int newStock)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            product.Stock = newStock;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Products.CountAsync();
        }
    }
}
