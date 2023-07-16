namespace Reprise
{
    internal sealed class ExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _Logger;

        private readonly IExceptionLogger _ExceptionLogger;

        private readonly IErrorResponseFactory _ErrorResponseFactory;

        private readonly RequestDelegate _Next;

        public ExceptionHandler(ILogger<ExceptionHandler> logger, IServiceProvider serviceProvider, RequestDelegate next)
        {
            _Logger = logger;
            _ExceptionLogger = serviceProvider.GetInternalDependency<IExceptionLogger>();
            _ErrorResponseFactory = serviceProvider.GetInternalDependency<IErrorResponseFactory>();
            _Next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _Next(httpContext);
            }
            catch (Exception ex)
            {
                try
                {
                    await Handle(ex, httpContext);
                }
                catch (Exception ex2)
                {
                    _Logger.LogExceptionInHandler(ex2);
                    if (!httpContext.Response.HasStarted)
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    }
                }
            }
        }

        private async Task Handle(Exception exception, HttpContext httpContext)
        {
            int statusCode;
            object? response;
            switch (exception)
            {
                case BadHttpRequestException badHttpRequestException:
                    statusCode = StatusCodes.Status400BadRequest;
                    var badHttpRequestErrorContext = new ErrorContext<BadHttpRequestException>(httpContext, badHttpRequestException);
                    _ExceptionLogger.Log(_Logger, badHttpRequestErrorContext);
                    response = _ErrorResponseFactory.Create(badHttpRequestErrorContext);
                    break;
                case ValidationException validationException:
                    statusCode = StatusCodes.Status400BadRequest;
                    var validationExceptionContext = new ErrorContext<ValidationException>(httpContext, validationException);
                    _ExceptionLogger.Log(_Logger, validationExceptionContext);
                    response = _ErrorResponseFactory.Create(validationExceptionContext);
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    var exceptionContext = new ErrorContext<Exception>(httpContext, exception);
                    _ExceptionLogger.Log(_Logger, exceptionContext);
                    response = _ErrorResponseFactory.Create(exceptionContext);
                    break;
            }
            await WriteResponse(exception, httpContext, statusCode, response);
        }

        private async Task WriteResponse(Exception exception, HttpContext httpContext, int statusCode, object? response)
        {
            if (httpContext.Response.HasStarted)
            {
                _Logger.LogResponseHasStarted(exception);
            }
            else
            {
                httpContext.Response.StatusCode = statusCode;
                if (response != null)
                {
                    await httpContext.Response.WriteAsJsonAsync(response);
                }
            }
        }
    }
}
