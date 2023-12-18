using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Context;
using ProductsAPI.DTOs;
using ProductsAPI.Models;
using ProductsAPI.Pagination;
using ProductsAPI.Repository;
using System.Text.Json;

namespace ProductsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public CategoriesController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    #region READ
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories([FromQuery] PaginationParameters paginationParameters)
    {
        PagedList<Category> category = await _uof.CategoryRepository.GetCategories(paginationParameters);

        object metadada = new
        {
            category.TotalCount,
            category.PageSize,
            category.CurrentPage,
            category.TotalPages,
            category.HasNext,
            category.HasPrevious
        };

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadada));
        var categoryDTO = _mapper.Map<List<CategoryDTO>>(category);
        return categoryDTO;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
    {
        var category = await _uof.CategoryRepository.GetById(c => c.CategoryId == id);

        if (category == null) return NotFound();

        var categoryDTO = _mapper.Map<CategoryDTO>(category);

        return Ok(categoryDTO);
    }

    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesWithProducts()
    {
        var categories = await _uof.CategoryRepository.GetCategoriesWithProducts();
        var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories);
        return categoriesDTO;
    }

    [HttpGet("orderByCategoryName")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesByCategoryName()
    {
        var category = await _uof.CategoryRepository.GetCategoriesOrderedByName();
        var categoryDTO = _mapper.Map<List<CategoryDTO>>(category);
        return categoryDTO;
    }
    #endregion READ

    #region CREATE
    [HttpPost]
    public async Task<ActionResult> PostCategory(CategoryDTO categoryDTO)
    {
        if (categoryDTO == null) return BadRequest();

        var category = _mapper.Map<Category>(categoryDTO);

        _uof.CategoryRepository.Add(category);
        await _uof.Commit();

        var newCategoryDTO = _mapper.Map<CategoryDTO>(category);

        return CreatedAtAction(nameof(GetCategoryById), new { id = newCategoryDTO.CategoryId }, newCategoryDTO);
    }
    #endregion CREATE

    #region UPDATE
    [HttpPut("{id:int}")]
    public async Task<ActionResult> PutCategory(CategoryDTO categoryDTO, int id)
    {
        if (categoryDTO.CategoryId != id) return BadRequest();

        var category = _mapper.Map<Category>(categoryDTO);

        _uof.CategoryRepository.Update(category);
        await _uof.Commit();

        return NoContent();
    }
    #endregion UPDATE

    #region DELETE
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var category = await _uof.CategoryRepository.GetById(c => c.CategoryId == id);

        if (category == null) return NotFound();

        _uof.CategoryRepository.Delete(category);
        await _uof.Commit();

        return NoContent();
    }
    #endregion DELETE
}