using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PddService.Common;
using PddService.Common.Controller;
using PddService.Common.Filters;
using PddService.Common.Result;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CaiNiaoSDK.Common;
using Z.EntityFramework.Plus;

namespace PddService.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ExpressController : BaseController<ExpressController>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        /// <param name="siteConfig"></param>
        public ExpressController(ILogger<ExpressController> logger, PddDbContext dbContext, SiteConfig siteConfig) : base(logger, dbContext, siteConfig)
        {
        }
        /// <summary>
        /// 获取快递公司列表
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页的数量</param>
        /// <param name="expressList"></param>
        /// <param name="name">快递公司名字</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<Express>>), 200)]
        public IActionResult List(int pageIndex = 1, int pageSize = 10, string expressList= null, string name = null)
        {


            pageIndex--;
            pageIndex = pageIndex < 0 ? 0 : pageIndex;
            var query = DbContext.Express
                .AsNoTracking();//.
                                // Where(x => x.ProductStatus == ProductStatus.Success);
                                //if (UserType == UserType.Agent) {

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            string[] _expressList= expressList?.Split(',')??new string[0];

            query = query.Where(x => _expressList.Contains(x.CaiNiaoCode));

            var data = query.OrderByDescending(x => x.Id).Skip(pageIndex * pageSize).Take(pageSize);
            return SuccessList(data, query.Count());
        }
        /// <summary>
        /// 添加一个打印模板
        /// </summary>
        /// <param name="printTemplate"></param>
        /// <returns></returns>
        [HttpPost, ModelValidation]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult AddTemplate([FromBody, Bind("Name", "ExpressId", "SourceUrl")]PrintTemplate printTemplate)
        {
            printTemplate.UserId = UserInfo.Id;
            printTemplate.Content = new WebClient().DownloadString(printTemplate.SourceUrl);
            printTemplate.CreatedTime = DateTime.Now;
            printTemplate.Size = Help.GetTemplateSize(printTemplate.Content);
            printTemplate.Delete = false;
            printTemplate.Default = !DbContext.PrintTemplate.Any(x => x.UserId == UserInfo.Id);
            DbContext.PrintTemplate.Add(printTemplate);
            DbContext.SaveChanges();
            return OkResult;
        }

        [HttpGet]
        public IActionResult Test(int id)
        {
           var info= DbContext.PrintTemplate.FirstOrDefault(x => x.Id == id);
           return Content(Help.GetTemplateContent(info.Content));
        }
        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页的数量</param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<Express>), 200)]
        public IActionResult TemplateList(int pageIndex = 1, int pageSize = 10, string name = null)
        {

            pageIndex--;
            pageIndex = pageIndex < 0 ? 0 : pageIndex;
            var query = DbContext.PrintTemplate
              .Include(x => x.Express).AsNoTracking();
            // Where(x => x.ProductStatus == ProductStatus.Success);
            //if (UserType == UserType.Agent) {
            query = query.Where(x => x.UserId == UserInfo.Id && !x.Delete);
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            var data = query.OrderByDescending(x => x.Id).Skip(pageIndex * pageSize).Take(pageSize);
            return SuccessList(data, query.Count());
        }
        /// <summary>
        /// 删除一个打印模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult DeleteTemplate(int id)
        {
            var printTemplate = DbContext.PrintTemplate.FirstOrDefault(x => x.UserId == UserInfo.Id && x.Id == id && !x.Delete);
            Assert.IsNull(printTemplate, "数据不存在");
            Assert.IsTrue(printTemplate.Default, "默认模板不允许删除");
            printTemplate.Delete = true;
            DbContext.SaveChanges();
            return OkResult;
        }
        /// <summary>
        /// 设置默认打印模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult SetDefault(int id)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {


                try
                {
                    var userId = UserInfo.Id;
                    DbContext.PrintTemplate.Where(x => x.UserId == userId && x.Default && !x.Delete).Update(x => new PrintTemplate()
                    {
                        Default = false
                    });
                    DbContext.PrintTemplate.Where(x => x.UserId == userId && !x.Delete && x.Id == id).Update(x => new PrintTemplate()
                    {
                        Default = true
                    });
                    transaction.Commit();
                    DbContext.SaveChanges();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return OkResult;
        }

        /// <summary>
        /// 查询物流轨迹
        /// </summary>
        /// <param name="waybillCode"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<ExpressFollow>>), 200)]
        public IActionResult Query(string waybillCode)
        {
            Assert.IsNullOrEmpty(waybillCode, "waybillCode不能为空");
            var result = DbContext.ExpressFollow.Where(x => x.TrackingNumber == waybillCode).OrderBy(x => x.StatusTime);
            return SuccessList(waybillCode);
        }

    }
}
