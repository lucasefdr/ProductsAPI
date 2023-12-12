using ProductsAPI.Models;
using ProductsAPI.Pagination;

namespace ProductsAPI.Repository;

public interface ICategoryRepository : IRepository<Category>
{
    IEnumerable<Category> GetCategoriesOrderedByName();
    IEnumerable<Category> GetCategoriesWithProducts();
    PagedList<Category> GetCategories(PaginationParameters paginationParameters);
}
