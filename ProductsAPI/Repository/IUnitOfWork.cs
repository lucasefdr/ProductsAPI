namespace ProductsAPI.Repository;

public interface IUnitOfWork
{
    // UnitOfWork will be responsible for creating the repositories
    IProductRepository ProductRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    Task Commit();
}
