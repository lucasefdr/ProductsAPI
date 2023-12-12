using ProductsAPI.Context;
using ProductsAPI.Models;
using ProductsAPI.Pagination;

namespace ProductsAPI.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {

        }

        public PagedList<Product>? GetProducts(PaginationParameters paginationParamenters)
        {
            //return Get()
            //    .OrderBy(p => p.Name)
            //    .Skip((paginationParamenters.PageNumber - 1) * productsParameters.PageSize)
            //    .Take(paginationParamenters.PageSize)
            //    .ToList();
            return PagedList<Product>.ToPagedList(Get()
                                                    .OrderBy(p => p.ProductId),
                                                    paginationParamenters.PageNumber,
                                                    paginationParamenters.PageSize);

        }

        public IEnumerable<Product> GetProductsOrderedByPrice() => Get().OrderBy(p => p.Price).ToList();
    }
}
