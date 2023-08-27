using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyDairy.DAL.Constexts;
using MyDairy.DAL.IRepositories;
using MyDairy.Domain.Commons;
using MyDairy.Domain.Entities;
using System.Linq.Expressions;

namespace MyDairy.DAL.Repositories;

public class Repository<T> : IRepository<T> where T : Auditable
{
    private readonly MyDairyDbContext dbContext;
    private readonly DbSet<T> table;

    public Repository(MyDairyDbContext dbContext)
    {
        this.dbContext = dbContext;
        table = dbContext.Set<T>();
    }

    public async ValueTask<T> AddAsync(T entity)
    {
        await table.AddAsync(entity);

        return entity;
    }

    public async ValueTask<bool> DeleteAsync(Expression<Func<T, bool>> expression)
    {
        var entity = await this.SelectAsync(expression);

        if (entity is not null)
        {
            entity.IsDeleted = true;
            return true;
        }

        return false;
    }

    public async ValueTask<T> SelectAsync(Expression<Func<T, bool>> expression, string[] includes = null)
            => await this.SelectAll(expression, includes).FirstOrDefaultAsync(t => !t.IsDeleted);

    public IQueryable<T> SelectAll(Expression<Func<T, bool>> expression = null, string[] includes = null, bool isTracking = true)
    {
        var query = expression is null ? isTracking ? table : table.AsNoTracking()
            : isTracking ? table.Where(expression) : table.Where(expression).AsNoTracking();

        if (includes is not null)
            foreach (var include in includes)
                query = query.Include(include);
        
        return query.Where(i => !i.IsDeleted);
    }

    public async ValueTask<T> UpdateAsync(T entity)
    {
        entity.ModifiedAt = DateTime.UtcNow;
        EntityEntry<T> entry = this.dbContext.Update(entity);

        return entry.Entity;
    }

    public async ValueTask SaveAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}
