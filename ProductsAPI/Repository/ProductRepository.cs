using ProductsAPI.Context;
using ProductsAPI.Models;

namespace ProductsAPI.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {

        }

        public IEnumerable<Product> GetProductsOrderedByPrice()
        {
            return Get().OrderBy(p => p.Price).ToList();
        }
    }
}
