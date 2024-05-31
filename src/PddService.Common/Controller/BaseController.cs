using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PddService.DataAccess;
using PddService.DataAccess.Entity;

namespace PddService.Common.Controller
{
    [Route("[controller]/[action]")]
    [ApiController]
    public abstract class BaseController<T> : ControllerBase where T : BaseController<T>
    {
        public ILogger<T> Logger { get; }
        public PddDbContext DbContext { get; }
        public SiteConfig SiteConfig { get; set; }
        protected BaseController(ILogger<T> logger,PddDbContext dbContext, SiteConfig siteConfig)
        {
            Logger = Logger;
            DbContext = dbContext;
            SiteConfig = siteConfig;
        }

        protected IActionResult OkResult => Ok(Common.Result.Result.Ok);

        protected IActionResult SuccessMessage(string message, int code = 200)
        {
            return Ok(Common.Result.Result.Success(message,code));
        }

        protected IActionResult Success<TParam>(TParam data)
        {
            return Ok(Common.Result.Result.Success(data));
        }

        protected IActionResult Fail(string message, int code = 400)
        {
            return Ok(Common.Result.Result.Fail(message, code));
        }

        protected IActionResult SuccessList<TParam>(IEnumerable<TParam> enumerable, int total = 0)
        {
            return Ok(Common.Result.Result.Success(enumerable, total));
        }

        public User UserInfo => (User) HttpContext.Items["user"];
    }
}
