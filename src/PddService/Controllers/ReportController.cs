using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PddService.Common;
using PddService.Common.Controller;
using PddService.DataAccess;
using Serenity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportController : BaseController<ReportController>
    {
        private IDistributedCache distributedCache;
        public ReportController(ILogger<ReportController> logger, PddDbContext dbContext, SiteConfig siteConfig, IDistributedCache distributedCache) : base(logger, dbContext, siteConfig)
        {
            this.distributedCache = distributedCache;
        }
        /// <summary>
        /// 获取店铺总数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMallCount()
        {
            var list = DbContext.Mall.Where(x => x.UserId == UserInfo.Id).AsNoTracking().Select(x => new
            {
                Expired = x.ExpireTime > DateTime.Now
            }).GroupBy(x => x.Expired).Select(x => new
            {
                Type = x.Key,
                Count = x.Count()
            }).ToList();
            return Success(list);
        }
        /// <summary>
        /// 查询日期的订单总数(根据成交时间查询)
        /// </summary>
        /// <param name="queryTime"></param>
        /// <param name="waybillStatus">发货状态[当值为-1则不进行查询，当值为0查询未发货，当值为1查询已发货]</param>
        /// <param name="printStatus">打印状态[当值为-1则不进行查询，当值为0查询未打单，当值为1查询已打单]</param>
        /// <param name="cacheTime">缓存时间，单位秒</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOrderCount(DateTime? queryTime, int waybillStatus = -1, int printStatus = -1, int cacheTime = 300)
        {
            var startTime = (queryTime?.Date) ?? DateTime.Today;
            var endTime = startTime.AddDays(1);
            var cacheKey = $"report:order:user{UserInfo.Id}:{startTime.Date.ToString("yyyyMMdd")}_{waybillStatus}_{printStatus}";

            var buffer = await distributedCache.GetAsync(cacheKey);
            var result = 0;
            if (buffer?.Any() ?? false)
            {
                result = BitConverter.ToInt32(buffer);
            }
            else
            {
                var query = DbContext.Order.AsNoTracking().Where(x => x.UserId == UserInfo.Id && x.ConfirmTime >= startTime && x.ConfirmTime < endTime);
                if (waybillStatus > -1)
                {
                    query = waybillStatus == 1 ? query.Where(x => x.WaybillStatus) : query.Where(x => !x.WaybillStatus);
                }
                if (printStatus > -1)
                {
                    query = waybillStatus == 1 ? query.Where(x => x.PrintStatus) : query.Where(x => !x.PrintStatus);
                }
                result = query.Count();
                await distributedCache.SetAsync(cacheKey, BitConverter.GetBytes(result), new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheTime)));
            }
            return Success(result);
        }
        /// <summary>
        /// 查询日期区间的订单总数
        /// </summary>
        /// <param name="days">要查询的天数</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMonthOrderCount(int days = 30)
        {
            var endTime = DateTime.Today;
            var startTime = endTime.AddDays(0 - days);
            var cacheKey = $"report:orderday:user{UserInfo.Id}:{startTime.Date.ToString("yyyyMMdd")}";
            var result = await distributedCache.GetObjectAsync<SortedDictionary<string, int>>(cacheKey);
            if (result == null)
            {
             var   resultDict = DbContext.Order
                      .AsNoTracking()
                      .Where(x => x.UserId == UserInfo.Id && x.ConfirmTime >= startTime && x.ConfirmTime < endTime)
                      .GroupBy(x => new
                      {
                          x.ConfirmTime.Year,
                          x.ConfirmTime.Month,
                          x.ConfirmTime.Day
                      })
                      .Select(x =>
                      new
                      {
                          x.Key.Year,
                          x.Key.Month,
                          x.Key.Day,
                          Count = x.Count()
                      }).ToList().ToDictionary(x => $"{x.Year}-{(x.Month > 10 ? x.Month.ToString() : ("0" + x.Month))}-{(x.Day > 10 ? x.Day.ToString() : ("0" + x.Day))}", x => x.Count);
                while (startTime < endTime)
                {
                    var key = startTime.ToString("yyyy-MM-dd");
                    if (!resultDict.ContainsKey(key))
                    {
                        resultDict[key] = 0;
                    }
                    startTime = startTime.AddDays(1);
                }
                result = new SortedDictionary<string, int>(resultDict);

                await distributedCache.SetObjectAsync(cacheKey, result,new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Today.AddDays(1)));
            }
            return Success(result);
        }

    }
}
