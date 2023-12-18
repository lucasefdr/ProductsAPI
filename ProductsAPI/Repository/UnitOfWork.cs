using ProductsAPI.Context;

namespace ProductsAPI.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly EFContext _context;

    public UnitOfWork(EFContext context)
    {
        _context = context;
    }

    // Implementing the repositories
    public IProductRepository ProductRepository => _productRepository ?? new ProductRepository(_context);
    public ICategoryRepository CategoryRepository => _categoryRepository ?? new CategoryRepository(_context);

    // Save pending changes to the data store
    public async Task Commit() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
