using Sample.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Migrations
{
    public class MigrationContextFactory : IDesignTimeDbContextFactory<MigrationContext>
    {
        public MigrationContext CreateDbContext(string[] args)
        {
            WebApplicationBuilder appBuilder = WebApplication.CreateBuilder(args);

            var connectString = appBuilder.Configuration.GetConnectionString("DB");

            var builder = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectString);

            return new MigrationContext(builder.Options);
        }
    }

    public class MigrationContext : AppDbContext
    {
        public MigrationContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
