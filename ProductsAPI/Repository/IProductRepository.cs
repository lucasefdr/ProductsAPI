using ProductsAPI.Models;
using ProductsAPI.Pagination;

namespace ProductsAPI.Repository;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsOrderedByPrice();
    //IEnumerable<Product>? GetProducts(ProductsParameters paginationParamenters);
    Task<PagedList<Product>> GetProducts(PaginationParameters paginationParamenters);
}
