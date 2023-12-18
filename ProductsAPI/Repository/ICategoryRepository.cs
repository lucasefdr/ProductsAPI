using ProductsAPI.Models;
using ProductsAPI.Pagination;

namespace ProductsAPI.Repository;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetCategoriesOrderedByName();
    Task<IEnumerable<Category>> GetCategoriesWithProducts();
    Task<PagedList<Category>> GetCategories(PaginationParameters paginationParameters);
}
