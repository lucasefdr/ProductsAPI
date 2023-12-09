using ProductsAPI.Models;

namespace ProductsAPI.Repository;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetProductsOrderedByPrice();
}
