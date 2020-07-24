using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BaseProject.Api.Data;

namespace BaseProject.Api.Infrastructure.ConfigRegister
{
    public static class DbContextConfig
    {
        public static void AddDbContextConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("SQLDB")));
        }
    }
}
