using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BaseProject.Api.Infrastructure.Middlewares
{
    public class ExceptionHandlerMiddleware : DelegatingHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException ex)
            {
                logger.LogError(ex, ex.Message);
                var response = context.Response;
                response.StatusCode = ex.StatusCode;
                response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { message = ex?.Message });
                await response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                var response = context.Response;
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { message = ex?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
