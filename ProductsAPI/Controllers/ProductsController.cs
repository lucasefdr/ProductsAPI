using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Context;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
        {
            // async operation
            var products = await _context.Products.AsNoTracking().ToListAsync();

            if (products is null) return NotFound("Products not found.");

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if (product is null) return NotFound("Product not found.");

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> PostProductAsync(Product product)
        {
            if (product is null) return BadRequest();

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductByIdAsync), new { id = product.ProductId }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutProductAsync(int id, Product product)
        {
            if (id != product.ProductId) return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null) return NotFound("Product not found.");

            _context.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
