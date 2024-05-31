using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PddOpenSDK;
using PddOpenSDK.Models.Request.Logistics;
using PddOpenSDK.Models.Request.Order;
using PddService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace PddService.Code.AsyncLogicHandler.Handlers
{
    public class AsyncNotityPlatformHandler : AbsAsyncLoginHandlerBase<int[]>
    {
        public AsyncNotityPlatformHandler(ILogger<AsyncNotityPlatformHandler> logger, PddDbContext dbContext, PddClient pddClient, ITokenContainer tokenContainer) : base(logger, dbContext, pddClient, tokenContainer)
        {
        }

        public override string MessageTypeName => AsyncHandlerConstant.NotityPlatform.ToString();

        public override Task Execute(int[] orderIds)
        {
            var orders = DbContext
                .Order
                .AsNoTracking()
                .Include(x => x.Express)
                .Where(x => orderIds.Contains(x.Id) && x.WaybillStatus&&!x.SyncStatus)
                .ToArray();
            var orderLoopup = orders.ToLookup(x => x.MallId);
            var list = new List<int>();
            foreach (var item in orderLoopup)
            {
                var mallInfo = DbContext.Mall.AsNoTracking().Include(x => x.User).FirstOrDefault(x => x.Id == item.Key);
                foreach (var el in item)
                {
                    try
                    {
                        var request = new SendLogisticsOnlineRequestModel()
                        {
                            OrderSn = el.OrderSn,
                            LogisticsId = el.Express.PddLogisticsId,
                            TrackingNumber = el.TrackingNumber
                        };
                        var pddResponse = PddClient.Request(request, mallInfo.AccessToken);
                        if (pddResponse?.LogisticsOnlineSendResponse?.IsSuccess ?? false)
                        {
                            list.Add(el.Id);
                        }
                        else
                        {
                            Logger.LogError($"订单{el.OrderSn}通知平台发货失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "通知平台发货状态异常");
                    }
                }
            }
            SaveNotityStatus(list);
            return Task.CompletedTask;
        }
        private void SaveNotityStatus(List<int> list)
        {
            try
            {
                DbContext.Order.Where(x => list.Contains(x.Id)).Update(x => new DataAccess.Entity.Order()
                {
                    SyncStatus = true,
                    SyncTime = DateTime.Now
                });
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"订单已经成功发货，同步字段更新异常 order_id={string.Join(',', list)}");
            }
        }
    }
}
