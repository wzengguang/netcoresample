using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Application
{
    public class ServiceExtension
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            var types = typeof(ServiceExtension).Assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsClass)
                {
                    var typeInterface = type.GetInterfaces().FirstOrDefault(a => a.Name == "I" + type.Name);
                    if (typeInterface != null)
                    {
                        services.AddScoped(typeInterface, type);
                        continue;
                    }
                }
            }

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(ServiceExtension).Assembly);
            });

            var mapper = new Mapper(configuration);

            services.AddSingleton<IMapper>(mapper);
        }
    }
}
