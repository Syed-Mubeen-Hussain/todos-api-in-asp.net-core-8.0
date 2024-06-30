
using Azure;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using TodosApi.Helper;

namespace TodosApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                var error = new ExceptionError()
                {
                    Title = ex.Message,
                    StatusCode = 500
                };
                var response = JsonConvert.SerializeObject(error);
                await context.Response.WriteAsync(response);
            }
        }
    }
}
