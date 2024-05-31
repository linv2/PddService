using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PddService.Common;
using PddService.Common.Controller;
using PddService.Common.Result;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Controllers
{
    public class TaskController : BaseController<TaskController>
    {
        public TaskController(ILogger<TaskController> logger, PddDbContext dbContext, SiteConfig siteConfig) : base(logger, dbContext, siteConfig)
        {
        }

        /// <summary>
        /// 查询全部异步任务
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页的数量</param>
        /// <param name="taskKey">任务Id</param>
        /// <param name="taskStatus">任务状态</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<AsyncTask>>), 200)]
        public IActionResult List(int pageIndex = 1, int pageSize = 10, string taskKey = null, int taskStatus = -1)
        {
            pageIndex--;
            pageIndex = pageIndex < 0 ? 0 : pageIndex;
            var query = DbContext.AsyncTask
                .AsNoTracking()
                .Where(x => x.UserId == UserInfo.Id);
            if (!string.IsNullOrEmpty(taskKey))
            {
                query = query.Where(x => x.TaskKey == taskKey);
            }
            if (taskStatus > -1)
            {
                query = query.Where(x => x.TaskStatus == taskStatus);
            }
            var data = query.OrderByDescending(x => x.Id).Skip(pageIndex * pageSize).Take(pageSize);
            return SuccessList(data, query.Count());
        }

        /// <summary>
        /// 查询任务的执行状态
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<IDictionary>), 200)]
        public IActionResult Sync([FromBody]string[] keys)
        {
            var query = DbContext.AsyncTask
               .AsNoTracking()
               .Where(x => x.UserId == UserInfo.Id && keys.Contains(x.TaskKey))
               .ToDictionary(x => x.TaskKey);
            return Success(query);
        }
    }
}
