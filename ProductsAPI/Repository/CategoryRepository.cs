using Microsoft.EntityFrameworkCore;
using ProductsAPI.Context;
using ProductsAPI.Models;

namespace ProductsAPI.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {

    }

    public IEnumerable<Category> GetCategoriesOrderedByName()
    {
        return Get().OrderBy(c => c.Name).ToList();
    }

    public IEnumerable<Category> GetCategoriesWithProducts()
    {
        return Get().Include(c => c.Products).ToList();
    }
}
