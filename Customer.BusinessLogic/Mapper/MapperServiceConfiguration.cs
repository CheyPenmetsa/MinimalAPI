using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Customer.BusinessLogic.Mapper
{
    public static class MapperServiceConfiguration
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }
    }
}
