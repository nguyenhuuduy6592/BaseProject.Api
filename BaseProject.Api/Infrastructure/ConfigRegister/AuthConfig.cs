using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using BaseProject.Api.Infrastructure.OptionsDTO;

namespace BaseProject.Api.Infrastructure.ConfigRegister
{
    public static class AuthConfig
    {
        public static void AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("TokenOptions").Get<TokenOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(x =>
                    {
                        x.RequireHttpsMetadata = false;
                        x.SaveToken = true;
                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.Secret)),
                            ValidateIssuer = true,
                            ValidIssuer = settings.Issuer,
                            ValidateAudience = true,
                            ValidAudience = settings.Audience,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,
                            RequireExpirationTime = true
                        };
                    });
        }
    }
}
