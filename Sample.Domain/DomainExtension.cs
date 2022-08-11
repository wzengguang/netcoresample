using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Sample.Domain.Entities;
using Sample.Domain.Accounts;

namespace Sample.Domain
{
    public class DomainExtension
    {
        public static void ConfigureDomain(IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DB"),
                x => x.MigrationsAssembly("Sample.Migrations")));

            var types = typeof(DomainExtension).Assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsClass)
                {
                    var typeInterface = type.GetInterfaces().FirstOrDefault(a => a.Name == "I" + type.Name);
                    if (typeInterface != null)
                    {
                        services.AddScoped(typeInterface, type);
                    }
                }
            }

            services.AddIdentity<User, Role>(option =>
            {
                option.SignIn.RequireConfirmedAccount = false;
                option.Password.RequireDigit = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<AppDbContext>();

        }
    }
}
