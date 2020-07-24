using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BaseProject.Api.Infrastructure.ConfigRegister
{
    public static class HealthChecksConfig
    {
        public static void AddHealthChecksConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                    .AddSqlServer(
                                connectionString: configuration.GetConnectionString("SQLDB"),
                                healthQuery: "SELECT 1;",
                                name: "SQL",
                                failureStatus: HealthStatus.Degraded,
                                tags: new string[] { "db", "sql", "sqlserver" });

            services
                .AddHealthChecksUI()
                .AddInMemoryStorage();
        }

        public static void UseHealthChecksConfig(this IApplicationBuilder app)
        {
            //adding health check point used by the UI
            app.UseHealthChecks("/self-health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            //adding health check UI
            app.UseHealthChecksUI();

        }
    }
}
