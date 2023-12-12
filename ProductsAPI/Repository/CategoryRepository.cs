using Microsoft.EntityFrameworkCore;
using ProductsAPI.Context;
using ProductsAPI.Models;
using ProductsAPI.Pagination;

namespace ProductsAPI.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {

    }

    public PagedList<Category> GetCategories(PaginationParameters paginationParameters)
    {
        return PagedList<Category>.ToPagedList(Get().OrderBy(c => c.CategoryId),
                                                                paginationParameters.PageNumber,
                                                                paginationParameters.PageSize);
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
