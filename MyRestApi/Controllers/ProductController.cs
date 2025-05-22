using Microsoft.AspNetCore.Mvc;
using MyRestApi.DTOs;
using MyRestApi.Models;
using MyRestApi.Services;

namespace MyRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                Category = dto.Category
            };

            var created = await _service.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
        {
            var result = await _service.UpdateAsync(id, productDto);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var results = await _service.SearchByNameAsync(keyword);
            return Ok(results);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var products = await _service.GetByCategoryAsync(category);
            return Ok(products);
        }

        [HttpGet("sorted")]
        public async Task<IActionResult> GetSorted([FromQuery] string order = "asc")
        {
            var sorted = await _service.GetSortedByPriceAsync(order.ToLower() == "desc");
            return Ok(sorted);
        }

        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int newStock)
        {
            var result = await _service.UpdateStockAsync(id, newStock);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetProductCount()
        {
            var count = await _service.CountAsync();
            return Ok(new { count });
        }
        
    }
}
