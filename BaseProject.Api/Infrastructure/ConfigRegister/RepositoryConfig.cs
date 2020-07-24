using Microsoft.Extensions.DependencyInjection;
using BaseProject.Api.Data.People;

namespace BaseProject.Api.Infrastructure.ConfigRegister
{
    public static class RepositoryConfig
    {
        public static void AddRepositoryConfig(this IServiceCollection services)
        {
            //Register Interface Mappings for Repositories
            services.AddScoped<IPersonRepository, PersonRepository>();
        }
    }
}
