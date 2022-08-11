using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Sample.Domain
{
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntityBase
    {
        public AppDbContext DbContext { get; }

        public DbSet<TEntity> DbSet { get; }

        public EFRepository(IServiceProvider serviceProvider)
        {
            this.DbContext = serviceProvider.GetService<AppDbContext>();
            this.DbSet = this.DbContext.Set<TEntity>();
        }


        public async Task<TEntity> FindAsync(long id)
        {
            return await this.DbSet.FindAsync(id);
        }

        public async Task<TEntity> FindAsync(ICollection<long> ids)
        {
            return await this.DbSet.FindAsync(ids);
        }

        public IQueryable<TEntity> FindAllAsync(ICollection<long> ids)
        {
            return this.DbSet.Where(a => ids.Contains(a.Id));
        }

        public async Task<TEntity> FindAsync(string id)
        {
            var longId = Convert.ToInt64(id);

            return await FindAsync(longId);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = DbSet.AsQueryable();
            if (propertySelectors != null)
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }
            return query;
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<TEntity> GetPages(int pageIndex, int pageSize)
        {
            return DbSet.AsQueryable().Skip(pageIndex * pageSize).Take(pageSize);
        }

        public IQueryable<TEntity> GetPages(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsQueryable().Where(predicate).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            return await DbSet.ToListAsync();
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.SingleOrDefault(predicate);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.SingleOrDefaultAsync(predicate);
        }

        public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            return await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(ICollection<TEntity> entity)
        {
            await DbSet.AddRangeAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void DeleteById(long id)
        {
            var entity = DbSet.FirstOrDefault(x => x.Id == id);
            DbSet.Remove(entity);
        }

        public async Task<int> SaveChangeAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public async Task<TEntity> LastOrDefaultAsync()
        {
            var entity = await DbSet.OrderBy(a => a.Id).LastOrDefaultAsync();
            return entity;
        }

        public async Task<int> CountAsync()
        {
           return await DbSet.CountAsync();
        }
    }
}
