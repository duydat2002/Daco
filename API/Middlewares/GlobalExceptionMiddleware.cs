namespace Daco.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
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
            //catch (DomainException ex)
            //{
            //    context.Response.StatusCode = StatusCodes.Status400BadRequest;
            //    await WriteResponse(context, ex.Code, ex.Message);
            //}
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                context.Response.ContentType = "application/json";

                var response = new ResponseDTO
                {
                    IsSuccess = false,
                    Code = "SYSTEM_ERROR",
                    Message = "Internal server error",
                    Data = null
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
