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
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _context.Products.ToList();

            if (products is null) return NotFound("Products not found.");

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (product is null) return NotFound("Product not found.");

            return Ok(product);
        }

        [HttpPost]
        public ActionResult PostProduct(Product product)
        {
            if (product is null) return BadRequest();

            _context.Products.Add(product);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        [HttpPut("{id:int}")]
        public ActionResult PutProduct(int id, Product product)
        {
            if (id != product.ProductId) return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            //var product = _context.Products.Find(id);

            if (product is null) return NotFound("Product not found.");

            _context.Remove(product);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
