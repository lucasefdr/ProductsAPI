using Microsoft.EntityFrameworkCore;
using ProductsAPI.Context;
using System.Linq.Expressions;

namespace ProductsAPI.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    // Create a constructor that accepts an instance of AppDbContext
    protected AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    // Set<T>() returns a DbSet<T> instance for the given entity type
    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public IQueryable<T> Get()
    {
        // AsNoTracking() improves performance by telling EF Core not to track changes
        return _context.Set<T>().AsNoTracking();
    }

    public T GetById(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().SingleOrDefault(predicate);
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.Set<T>().Update(entity);
    }
}
