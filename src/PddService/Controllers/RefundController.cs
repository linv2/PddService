using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PddService.Common;
using PddService.Common.Controller;
using PddService.Common.Result;
using PddService.DataAccess;
using PddService.DataAccess.Entity;

namespace PddService.Controllers
{
    /// <summary>
    /// 退款
    /// </summary>
    public class RefundController : BaseController<RefundController>
    {
        public RefundController(ILogger<RefundController> logger, PddDbContext dbContext, SiteConfig siteConfig) : base(logger, dbContext, siteConfig)
        {
        }

        /// <summary>
        /// 查询退款订单
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页的数量</param>
        /// <param name="orderSn">订单号</param>
        /// <param name="name">收货人名字</param>
        /// <param name="mobile">收货人电话</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<OrderRefund>>), 200)]
        public IActionResult List(int pageIndex = 1, int pageSize = 10,string orderSn=null, string name = null, string mobile = null, DateTime? startTime = null, DateTime? endTime = null)
        {
            pageIndex--;
            pageIndex = pageIndex < 0 ? 0 : pageIndex;
            var query = DbContext.OrderRefund
                .AsNoTracking();
            if (!string.IsNullOrEmpty(orderSn))
            {
                query = query.Where(x => x.OrderSn == orderSn);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Order.ReceiverName == name);
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                query = query.Where(x => x.Order.ReceiverPhone == mobile);
            }
            if (startTime.HasValue)
            {
                query = query.Where(x => x.Modified >= startTime);
            }
            if (endTime.HasValue)
            {
                query = query.Where(x => x.Modified < endTime);
            }
            var data = query.OrderByDescending(x => x.Id).Skip(pageIndex * pageSize).Take(pageSize);
            return SuccessList(data, query.Count());
        }
    }
}