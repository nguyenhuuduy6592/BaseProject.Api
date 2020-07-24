using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProject.Api.Infrastructure.ConfigRegister
{
    public static class ModelValidatorsConfig
    {
        public static void AddFluentValidatorsConfig(this IServiceCollection services)
        {
            //Register DTO Validators
            //services.AddTransient<IValidator<CreatePersonRequest>, CreatePersonRequestValidator>();

            //Disable Automatic Model State Validation built-in to ASP.NET Core
            services.Configure<ApiBehaviorOptions>(opt => { opt.SuppressModelStateInvalidFilter = true; });
        }
    }
}
