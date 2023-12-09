using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Context;
using ProductsAPI.Filters;
using ProductsAPI.Models;
using ProductsAPI.Services;

namespace ProductsAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public ProductsController(AppDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))] // Add logging services - builder.Services.AddScoped<ApiLoggingFilter>();
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
        {
            _logger.LogInformation("Getting products...");

            // async operation
            var products = await _context.Products.AsNoTracking().ToListAsync();

            if (products is null) return NotFound("Products not found.");

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        [ActionName(nameof(GetProductByIdAsync))]
        public async Task<ActionResult<Product>> GetProductByIdAsync([FromRoute] int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if (product is null) return NotFound("Product not found.");

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> PostProductAsync([FromBody] Product product)
        {
            if (product is null) return BadRequest();

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductByIdAsync), new { id = product.ProductId }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutProductAsync([FromRoute] int id, [FromBody] Product product)
        {
            if (id != product.ProductId) return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProductAsync([FromRoute] int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null) return NotFound("Product not found.");

            _context.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("fromServices")]
        public ActionResult<string> GetFromServices([FromQuery] string name, [FromServices] IFromService service)
        {
            return Ok(service.HelloWorld(name));
        }
    }
}
