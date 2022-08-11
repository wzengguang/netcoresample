using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Sample.Domain
{
    public interface IRepository<TEntity> : IRepository where TEntity : class, IEntityBase
    {
        AppDbContext DbContext { get; }

        DbSet<TEntity> DbSet { get; }

        Task<EntityEntry<TEntity>> AddAsync(TEntity entity);

        void Delete(TEntity entity);
        void DeleteById(long id);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllListAsync();
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<int> SaveChangeAsync();
        IQueryable<TEntity> GetPages(int pageIndex, int pageSize);
        IQueryable<TEntity> GetPages(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindAsync(long id);
        Task<TEntity> FindAsync(string id);


        Task AddRangeAsync(ICollection<TEntity> entity);
        Task<TEntity> FindAsync(ICollection<long> ids);
        IQueryable<TEntity> FindAllAsync(ICollection<long> ids);
        Task<TEntity> LastOrDefaultAsync();
        Task<int> CountAsync();
    }

    public interface IRepository
    {

    }
}
