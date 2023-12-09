using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Context;
using ProductsAPI.Models;
using ProductsAPI.Repository;

namespace ProductsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork _uof;

    public CategoriesController(IUnitOfWork uof)
    {
        _uof = uof;
    }

    #region READ
    [HttpGet]
    public ActionResult<IEnumerable<Category>> GetCategories()
    {
        return _uof.CategoryRepository.Get().ToList();
    }

    [HttpGet("{id:int}")]
    public ActionResult<Category> GetCategoryById(int id)
    {
        var category = _uof.CategoryRepository.GetById(c => c.CategoryId == id);

        if (category == null) return NotFound();

        return Ok(category);
    }

    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
    {
        return _uof.CategoryRepository.GetCategoriesWithProducts().ToList();
    }

    [HttpGet("orderByCategoryName")]
    public ActionResult<IEnumerable<Category>> GetCategoriesByCategoryName()
    {
        return _uof.CategoryRepository.GetCategoriesOrderedByName().ToList();
    }
    #endregion READ

    #region CREATE
    [HttpPost]
    public ActionResult PostCategory(Category category)
    {
        if (category == null) return BadRequest();

        _uof.CategoryRepository.Add(category);
        _uof.Commit();

        return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryId }, category);
    }
    #endregion CREATE

    #region UPDATE
    [HttpPut("{id:int}")]
    public ActionResult PutCategory(Category category, int id)
    {
        if (category.CategoryId != id) return BadRequest();

        _uof.CategoryRepository.Update(category);
        _uof.Commit();

        return NoContent();
    }
    #endregion UPDATE

    #region DELETE
    [HttpDelete("{id:int}")]
    public ActionResult DeleteCategory(int id)
    {
        var category = _uof.CategoryRepository.GetById(c => c.CategoryId == id);

        if (category == null) return NotFound();

        _uof.CategoryRepository.Delete(category);
        _uof.Commit();

        return NoContent();
    }
    #endregion DELETE
}