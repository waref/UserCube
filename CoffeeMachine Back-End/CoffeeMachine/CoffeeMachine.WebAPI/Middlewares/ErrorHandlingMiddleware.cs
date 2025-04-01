using CoffeeMachine.Core.ConstantStrings;

namespace CoffeeMachine.WebAPI.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConsStringLogMessages.MiddlewareExceptionMessage);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(ConsStringLogMessages.MiddlewareGeneralErrorMessage);
            }
        }
    }
}
