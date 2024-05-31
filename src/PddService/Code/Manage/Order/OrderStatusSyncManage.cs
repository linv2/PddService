using Microsoft.Extensions.Logging;
using PddOpenSDK;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace PddService.Code.Manage.Order
{
    public class OrderStatusSyncManage
    {
        private ILogger<OrderStatusSyncManage> logger;
        private PddClient pddClient;
        private PddDbContext dbContext;
        private ITokenContainer tokenContainer;
        public OrderStatusSyncManage(ILogger<OrderStatusSyncManage> logger, PddClient pddClient, PddDbContext dbContext, ITokenContainer tokenContainer)
        {
            this.logger = logger;
            this.pddClient = pddClient;
            this.dbContext = dbContext;
            this.tokenContainer = tokenContainer;
        }

        public void Sync(int userId, params int[] orderIds)
        {
            var orders = dbContext.Order.Where(x => x.UserId == userId && orderIds.Contains(x.Id)).ToArray();
            var orderLoopup = orders.ToLookup(x => x.MallId);
            foreach (var item in orderLoopup)
            {
                var index = 0;
                while (index < item.Count())
                {
                    var tempArray = item.Skip(index).Take(50);
                    Mall mallInfo = tokenContainer.GetMall(tempArray.FirstOrDefault()?.MallId ?? 0);
                    SyncOrder(mallInfo, tempArray);
                    index += 50;
                }
            }
        }
        private void SyncOrder(Mall mallInfo, IEnumerable<DataAccess.Entity.Order> orders)
        {
            var request = new PddOpenSDK.Models.Request.Order.GetOrderStatusRequestModel()
            {
                OrderSns = string.Join(',', orders.Select(x => x.OrderSn))
            };
            try
            {
                var response = pddClient.Request(request, mallInfo.AccessToken);
                foreach (var item in response.OrderStatusGetResponse.OrderStatusList)
                {
                    dbContext.Order.Where(x => x.MallId == mallInfo.Id && x.OrderSn == item.Ordersn).Update(x => new DataAccess.Entity.Order()
                    {
                        OrderStatus = item.OrderStatus ?? 0,
                        RefundStatus = item.RefundStatus ?? 0
                    });
                }
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "同步订单状态过程中发生异常");
            }
        }
    }
}
