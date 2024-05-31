using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using PddService.Common.Exceptions;
using PddService.DataAccess;

namespace PddService.Common.Filters
{
    public class AuthorizeFilter : IActionFilter
    {
        private PddDbContext _dbContext;

        public AuthorizeFilter(PddDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            var allowAnonymous = controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                .Any();
            if (!allowAnonymous)
            {
                var userId = context.HttpContext.Session.GetInt32("userId");
                if ((userId ?? 0) < 1)
                {
                    context.Result = new OkObjectResult(Result.Result.Fail("当前身份未授权", 401));
                    return;
                }
                var user = _dbContext.User.FirstOrDefault(x => x.Id == userId.Value);
                context.HttpContext.Items["user"] = user;
            }
        }
    }
}
