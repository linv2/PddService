using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PddService.Common.Attribute;

namespace PddService.Common.Filters
{
    public class RequestLoggerFilter : IActionFilter
    {
        private readonly ILogger<RequestLoggerFilter> logger;
        private readonly IRequestTrace requestTrace;

        public RequestLoggerFilter(ILogger<RequestLoggerFilter> logger, IRequestTrace requestTrace)
        {
            this.logger = logger;
            this.requestTrace = requestTrace;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            requestTrace.Trace();
            var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            var noLogger = controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(NoLoggerAttribute), false)
                .Any();
            context.HttpContext.Items["noLogger"] = noLogger;
            if (noLogger)
            {
                return;
            }

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            context.HttpContext.Items["stopWatch"] = stopWatch;
            var builder = new StringBuilder();

            builder.AppendLine()
                .AppendFormat("{0} {1}", context.HttpContext.Request.Method,
                context.HttpContext.Request.GetEncodedPathAndQuery())
                .AppendLine();
            foreach (var header in context.HttpContext.Request.Headers)
            {
                builder
                    .AppendFormat("[header] {0}:{1}", header.Key, string.Join(";", header.Value))
                .AppendLine();
            }
            logger.LogDebug(builder.ToString());
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var stopWatch = context.HttpContext.Items["stopWatch"] as Stopwatch;
            stopWatch?.Stop();
            var noLogger = (bool)context.HttpContext.Items["noLogger"];

            if (!noLogger && context.HttpContext.Response != null)
            {
                if (stopWatch != null)
                {
                    logger.LogDebug("request time：{0}ms", stopWatch.ElapsedMilliseconds);
                }

            }
            requestTrace.Clear();
        }
    }
}