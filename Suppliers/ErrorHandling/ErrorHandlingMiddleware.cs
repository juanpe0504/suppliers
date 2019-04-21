namespace Suppliers.API.ErrorHandling
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Suppliers.Domain.Exceptions;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Threading.Tasks;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;

            if (ex is NotFoundException) code = HttpStatusCode.NotFound;
            else if (ex is ValidationException) code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new ErrorResult { ErrorMessage = ex.Message, Code = (int)code });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
