using ProductsAPI.Models;

namespace ProductsAPI.Repository;

public interface ICategoryRepository : IRepository<Category>
{
    IEnumerable<Category> GetCategoriesOrderedByName();
    IEnumerable<Category> GetCategoriesWithProducts();
}
