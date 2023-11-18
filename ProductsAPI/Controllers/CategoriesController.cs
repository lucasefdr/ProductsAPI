using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Context;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            var categories = _context.Categories.ToList();

            if (categories is null) return NotFound("Categories not found.");

            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Category> GetCategoryById(int id)
        {
            var category = _context.Categories.Include(c => c.Products).FirstOrDefault(c => c.CategoryId == id);

            if (category == null) return NotFound();

            return Ok(category);
        }

        [HttpGet("products")]
        public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
        {
            var categoriesWithProducts = _context.Categories.Include(c => c.Products).ToList();

            if (categoriesWithProducts is null) return NotFound();

            return Ok(categoriesWithProducts);
        }

        [HttpPost]
        public ActionResult PostCategory(Category category)
        {
            if (category == null) return BadRequest();

            _context.Categories.Add(category);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id:int}")]
        public ActionResult PutCategory(Category category, int id)
        {
            if (category.CategoryId != id) return BadRequest();

            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return NoContent();
        }
    }
}