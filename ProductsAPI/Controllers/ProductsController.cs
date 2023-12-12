using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.DTOs;
using ProductsAPI.Models;
using ProductsAPI.Pagination;
using ProductsAPI.Repository;
using System.Text.Json;

namespace ProductsAPI.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public ProductsController(IUnitOfWork uof, ILogger<ProductsController> logger, IMapper mapper)
    {
        _uof = uof;
        _logger = logger;
        _mapper = mapper;
    }

    #region READ
    [HttpGet]
    //[ServiceFilter(typeof(ApiLoggingFilter))] // Add logging services - builder.Services.AddScoped<ApiLoggingFilter>();
    public ActionResult<IEnumerable<ProductDTO>> GetProducts([FromQuery] PaginationParameters paginationParameters)
    {
        //_logger.LogInformation("Getting products...");
        var products = _uof.ProductRepository.GetProducts(paginationParameters);

        var metadada = new
        {
            products.TotalCount,
            products.PageSize,
            products.CurrentPage,
            products.TotalPages,
            products.HasNext,
            products.HasPrevious
        };

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadada));

        var productsDTO = _mapper.Map<List<ProductDTO>>(products);

        return productsDTO;
    }

    [HttpGet("{id:int}")]
    [ActionName(nameof(GetProductById))]
    public ActionResult<ProductDTO> GetProductById([FromRoute] int id)
    {
        var product = _uof.ProductRepository.GetById(p => p.ProductId == id);

        if (product is null) return NotFound("Product not found.");

        var productDTO = _mapper.Map<ProductDTO>(product);

        return Ok(productDTO);
    }

    [HttpGet("orderByPrice")]
    public ActionResult<IEnumerable<ProductDTO>> GetProductsOrderedByPrice()
    {
        var products = _uof.ProductRepository.GetProductsOrderedByPrice()?.ToList();
        var productsDTO = _mapper.Map<List<ProductDTO>>(products);

        return productsDTO;
    }
    #endregion READ

    #region CREATE
    [HttpPost]
    public ActionResult PostProduct([FromBody] ProductDTO productDTO)
    {
        if (productDTO is null) return BadRequest();

        var product = _mapper.Map<Product>(productDTO);

        _uof.ProductRepository.Add(product);
        _uof.Commit();

        var newProductDTO = _mapper.Map<ProductDTO>(product);

        return CreatedAtAction(nameof(GetProductById), new { id = newProductDTO.ProductId }, newProductDTO);
    }
    #endregion CREATE

    #region UPDATE
    [HttpPut("{id:int}")]
    public ActionResult PutProduct([FromRoute] int id, [FromBody] ProductDTO productDTO)
    {
        if (id != productDTO.ProductId) return BadRequest();

        var product = _mapper.Map<Product>(productDTO);

        _uof.ProductRepository.Update(product);
        _uof.Commit();

        return NoContent();
    }
    #endregion UPDATE

    #region DELETE
    [HttpDelete("{id:int}")]
    public ActionResult DeleteProduct([FromRoute] int id)
    {
        var product = _uof.ProductRepository.GetById(p => p.ProductId == id);

        if (product is null) return NotFound("Product not found.");

        _uof.ProductRepository.Delete(product);
        _uof.Commit();

        return NoContent();
    }
    #endregion DELETE

    /*[HttpGet("fromServices")]
    public ActionResult<string> GetFromServices([FromQuery] string name, [FromServices] IFromService service)
    {
        return Ok(service.HelloWorld(name));
    }*/
}
