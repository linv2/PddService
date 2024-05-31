using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PddService.Common.Exceptions;

namespace PddService.Common.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ApiException exception)
            {
                context.Result = new OkObjectResult(exception.Result);
            }
            else if (context.Exception is TokenExpireExption tokenExpireExption)
            {
                context.Result = new OkObjectResult(Result.Result.Fail(tokenExpireExption.Message));
            }
            else
            {
                _logger.LogError(context.Exception, "拦截器拦截到未处理异常");
                var result = Result.Result.Fail(context.Exception.Message);
                context.Result = new OkObjectResult(result);
            }
        }
    }
}