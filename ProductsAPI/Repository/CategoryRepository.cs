using Microsoft.EntityFrameworkCore;
using ProductsAPI.Context;
using ProductsAPI.Models;
using ProductsAPI.Pagination;

namespace ProductsAPI.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(EFContext context) : base(context)
    {

    }

    public Task<PagedList<Category>> GetCategories(PaginationParameters paginationParameters)
    {
        return PagedList<Category>.ToPagedList(Get().OrderBy(c => c.CategoryId),
                                                                paginationParameters.PageNumber,
                                                                paginationParameters.PageSize);
    }

    public async Task<IEnumerable<Category>> GetCategoriesOrderedByName()
    {
        return await Get().OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetCategoriesWithProducts()
    {
        return await Get().Include(c => c.Products).ToListAsync();
    }
}
