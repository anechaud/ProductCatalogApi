using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Data.Entity.Core;
using System.Net;
using System.Threading.Tasks;

namespace ProductCatalouge.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                SetTraceId(context);
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex);
            }
        }

        private void SetTraceId(HttpContext context)
        {
            context.Request.Headers.TryGetValue("TraceId", out StringValues traceId);
            if (string.IsNullOrEmpty(Convert.ToString(traceId)))
                context.Request.Headers.Add("TraceId", Guid.NewGuid().ToString());
        }

        private Task HandleExceptionMessageAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Request.Headers.TryGetValue("TraceId", out StringValues traceId);
            traceId = string.IsNullOrEmpty(Convert.ToString(traceId)) ? Guid.NewGuid().ToString() : traceId.ToString();
            int statusCode;
            string errorMessage;
            switch (ex)
            {
                case SqlException _:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = $"TraceId: {traceId}, Oracle Exception Occured: {ex.Message}, Please check logs for more detail";
                    break;
                case ObjectNotFoundException _:
                    statusCode = (int)HttpStatusCode.NotFound;
                    errorMessage = $"TraceId: {traceId},  Object not found Exception Occured: {ex.Message}, Please check logs for more detail";
                    break;
                case TimeoutException _:
                    statusCode = (int)HttpStatusCode.RequestTimeout;
                    errorMessage = $"TraceId: {traceId},  Timeout Exception Occured: {ex.Message}, Please check logs for more detail";
                    break;
                case BadRequestException _:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorMessage = $"TraceId: {traceId},  Bad Request Exception Occured: {ex.Message}, Please check logs for more detail";
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = $"TraceId: {traceId},  Message: Something went wrong Please check logs for more detail; Error: {ex.Message}";
                    break;
            }
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = errorMessage
            });
            _logger.LogError($"Error occured : {errorMessage}");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
