using AutoMapper;
using AutoWrapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BaseProject.Api.Infrastructure.ConfigRegister;
using FluentValidation.AspNetCore;

namespace BaseProject.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerConfig();

            services.AddCorsConfig();

            services.AddFluentValidatorsConfig();

            services.AddRepositoryConfig();

            services.AddAuthenticationConfig(Configuration);

            services.AddHealthChecksConfig(Configuration);

            services.AddDbContextConfig(Configuration);

            services.AddAutoMapper(typeof(AutoMapperProfileConfig));

            services.AddControllers()
                .AddFluentValidation(fv => { fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCutomMiddlewareConfig();

            app.UseSwaggerConfig();

            app.UseHealthChecksConfig();

            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { IsDebug = true, ShowStatusCode = true, IsApiOnly = false });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksUI();
            });
        }
    }
}
