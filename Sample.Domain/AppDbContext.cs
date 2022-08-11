using Sample.Domain.Accounts;
using Sample.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Sample.Domain
{
    public class AppDbContext : IdentityDbContext<User, Role, long>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var foreignKeys = builder.Model.GetEntityTypes().SelectMany(a => a.GetForeignKeys()).Where(fk =>
              {
                  return fk.DeleteBehavior == DeleteBehavior.Cascade;
              });
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.Entity<Resources.Resource>().HasQueryFilter(e => !e.IsDeleted);
        }

        public DbSet<Resources.Resource> Resources { get; set; }
    }
}
