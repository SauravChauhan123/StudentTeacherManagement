using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Infrastructure.CustomMiddleware
{
    /// <summary>
    /// Middleware for handling exceptions and returning a custom error response.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next request delegate in the middleware pipeline.</param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Middleware Invocation

        /// <summary>
        /// Invokes the middleware, handling exceptions that occur during request processing.
        /// </summary>
        /// <param name="httpContext">The HTTP context for the current request.</param>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        #endregion

        #region Exception Handling

        /// <summary>
        /// Handles exceptions by setting the response status code and writing a custom error message.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorDetails = new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error"
            };

            return context.Response.WriteAsync(errorDetails.ToString());
        }

        #endregion
    }
}
