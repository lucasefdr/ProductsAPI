using Microsoft.EntityFrameworkCore;
using ProductsAPI.Context;
using ProductsAPI.Models;
using ProductsAPI.Pagination;

namespace ProductsAPI.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(EFContext context) : base(context)
        {

        }

        public async Task<PagedList<Product>> GetProducts(PaginationParameters paginationParamenters)
        {
            //return Get()
            //    .OrderBy(p => p.Name)
            //    .Skip((paginationParamenters.PageNumber - 1) * productsParameters.PageSize)
            //    .Take(paginationParamenters.PageSize)
            //    .ToList();
            return await PagedList<Product>.ToPagedList(Get()
                                                    .OrderBy(p => p.ProductId),
                                                    paginationParamenters.PageNumber,
                                                    paginationParamenters.PageSize);

        }

        public async Task<IEnumerable<Product>> GetProductsOrderedByPrice() => await Get().OrderBy(p => p.Price).ToListAsync();
    }
}
