using ProductsAPI.Models;
using ProductsAPI.Pagination;

namespace ProductsAPI.Repository;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product>? GetProductsOrderedByPrice();
    //IEnumerable<Product>? GetProducts(ProductsParameters paginationParamenters);
    PagedList<Product>? GetProducts(PaginationParameters paginationParamenters);
}
