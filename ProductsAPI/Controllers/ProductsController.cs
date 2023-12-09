using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Models;
using ProductsAPI.Repository;

namespace ProductsAPI.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly ILogger _logger;

    public ProductsController(IUnitOfWork uof, ILogger<ProductsController> logger)
    {
        _uof = uof;
        _logger = logger;
    }

    #region READ
    [HttpGet]
    //[ServiceFilter(typeof(ApiLoggingFilter))] // Add logging services - builder.Services.AddScoped<ApiLoggingFilter>();
    public ActionResult<IEnumerable<Product>> GetProducts()
    {
        //_logger.LogInformation("Getting products...");
        return _uof.ProductRepository.Get().ToList();
    }

    [HttpGet("{id:int}")]
    [ActionName(nameof(GetProductById))]
    public ActionResult<Product> GetProductById([FromRoute] int id)
    {
        var product = _uof.ProductRepository.GetById(p => p.ProductId == id);

        if (product is null) return NotFound("Product not found.");

        return Ok(product);
    }

    [HttpGet("orderByPrice")]
    public ActionResult<IEnumerable<Product>> GetProductsOrderedByPrice()
    {
        return _uof.ProductRepository.GetProductsOrderedByPrice().ToList();
    }
    #endregion READ

    #region CREATE
    [HttpPost]
    public ActionResult PostProduct([FromBody] Product product)
    {
        if (product is null) return BadRequest();

        _uof.ProductRepository.Add(product);
        _uof.Commit();

        return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
    }
    #endregion CREATE

    #region UPDATE
    [HttpPut("{id:int}")]
    public ActionResult PutProduct([FromRoute] int id, [FromBody] Product product)
    {
        if (id != product.ProductId) return BadRequest();

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
