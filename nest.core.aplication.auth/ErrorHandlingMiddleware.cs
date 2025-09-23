using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using nest.core.dominio.Excepciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace nest.core.aplication.auth
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (IdentityException iex)
            {
                _logger.LogError(iex, "IdentityException exception: {TraceId}", context.TraceIdentifier);
                await WriteResponseAsync(context, HttpStatusCode.InternalServerError, new
                {
                    type = $"https://httpstatuses.io/{(int)HttpStatusCode.BadRequest}",
                    title = "Internal Server Error",
                    status = (int)HttpStatusCode.BadRequest,
                    errors = iex.Errors.Select(e => new { field = e.Code, message = e.Description }),
                    traceId = context.TraceIdentifier
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {TraceId}", context.TraceIdentifier);
                await WriteResponseAsync(context, HttpStatusCode.InternalServerError, new
                {
                    type = $"https://httpstatuses.io/{(int)HttpStatusCode.InternalServerError}",
                    title = "Internal Server Error",
                    status = (int)HttpStatusCode.InternalServerError,
                    detail = GetMessage(ex),
                    traceId = context.TraceIdentifier
                });
            }
        }

        private static async Task WriteResponseAsync(HttpContext context, HttpStatusCode statusCode, object problem)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }

        private string GetMessage(Exception exception)
        {
            string message = string.Empty;
            if (exception.InnerException != null && !string.IsNullOrWhiteSpace(exception.InnerException.Message))
                message = exception.InnerException.Message;
            if (!string.IsNullOrWhiteSpace(exception.Message))
                message += (message.Length > 0 ? " - " : string.Empty) + exception.Message;
            return message;
        }
    }
}
