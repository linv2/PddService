using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PddOpenSDK;
using PddOpenSDK.Exception;
using PddOpenSDK.Models.Request.Order;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static PddOpenSDK.Models.Response.Order.GetOrderInformationResponseModel.OrderInfoGetResponseResponseModel;

namespace PddService.Code.PddSubscribeHandle
{
    public abstract class AbsSubscribeHandle<TRequest> : ISubscribeHandle
    {
        protected PddClient PddClient { get; }
        protected ILogger Logger { get; }
        protected PddDbContext DbContext { get; }
        protected ITokenContainer TokenContainer { get; set; }
        protected AbsSubscribeHandle(ILogger logger, PddDbContext dbContext, PddClient pddClient, ITokenContainer tokenContainer)
        {
            Logger = logger;
            DbContext = dbContext;
            PddClient = pddClient;
            TokenContainer = tokenContainer;
        }
        public abstract string MessageTypeName { get; }
        public abstract bool Execute(TRequest request);

        public bool MessageHandle(string messageBody)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<TRequest>(messageBody);
                return Execute(request);
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"同步拼多多消息异常,消息内容={messageBody}");
                return false;
            }
        }

        protected SelfOrderStatus GetSelfOrderStatus(OrderInfoResponseModel orderInfoResponseModel)
        {
            if ((orderInfoResponseModel.AfterSalesStatus ?? 0) > 0)
            {
                return SelfOrderStatus.Refund;
            }
            if ((orderInfoResponseModel.OrderStatus ?? 0) > 0)
            {
                switch (orderInfoResponseModel.OrderStatus)
                {
                    case 1: return SelfOrderStatus.Created;
                    case 2: return SelfOrderStatus.WaybillNotityed;
                    case 3: return SelfOrderStatus.Complate;
                    default: return SelfOrderStatus.Complate;
                }
            }
            return SelfOrderStatus.Complate;
        }

        protected bool UpdateByOrderSn(Mall mallInfo, string orderSn, out int orderId)
        {
            orderId = 0;
            PddOpenSDK.Models.Response.Order.GetOrderInformationResponseModel orderResponse = null;
            var tryNumber = 0;
            do
            {
                try
                {
                    orderResponse = PddClient.Request(new GetOrderInformationRequestModel() { OrderSn = orderSn }, mallInfo.AccessToken);
                    tryNumber = 10;
                }
                catch (RequestException requestException)
                {
                    Logger.LogError(requestException, "请求订单失败，重新尝试");
                    tryNumber++;
                    Thread.Sleep(1500);
                }
            } while (tryNumber < 3);
            if (orderResponse?.OrderInfoGetResponse?.OrderInfo != null)
            {
                orderId = DbContext.Order.Where(x => x.MallId == mallInfo.Id && x.OrderSn == orderSn).Select(x => x.Id).FirstOrDefault();
                if (orderId == 0)
                {
                    SaveNewOrder(mallInfo, orderResponse?.OrderInfoGetResponse?.OrderInfo, out orderId);
                    return true;
                }
            }
            else
            {
                Logger.LogWarning($"OrderSn={orderSn}订单获取失败");
            }
            return false;

        }
        private void SaveNewOrder(Mall mallInfo, OrderInfoResponseModel pddOrderInfo, out int orderId)
        {
            var details = new List<OrderDetail>(pddOrderInfo.ItemList.Count);
            foreach (var orderItem in pddOrderInfo.ItemList)
            {

                var orderDetail = new OrderDetail
                {
                    UserId = mallInfo.UserId,
                    MallId = mallInfo.Id,
                    OrderSn = pddOrderInfo.OrderSn,
                    GoodsName = orderItem.GoodsName,
                    GoodsPrice = orderItem.GoodsPrice ?? 0,
                    GoodsSpec = orderItem.GoodsSpec,
                    GoodsCount = orderItem.GoodsCount ?? 1,
                    SkuId = Convert.ToString(orderItem.SkuId),
                    GoodsImg = orderItem.GoodsImg
                };
                details.Add(orderDetail);

            }
            var order = new Order()
            {
                UserId = mallInfo.UserId,
                MallId = mallInfo.Id,
                OwnerId = mallInfo.OwnerId,
                OrderNum = pddOrderInfo.ItemList.Count,
                OrderSn = pddOrderInfo.OrderSn,
                CreatedTime = Convert.ToDateTime(pddOrderInfo.CreatedTime),
                ConfirmTime = Convert.ToDateTime(pddOrderInfo.ConfirmTime),
                Country = pddOrderInfo.Country,
                Province = pddOrderInfo.Province,
                City = pddOrderInfo.City,
                Town = pddOrderInfo.Town,
                Address = pddOrderInfo.Address,
                ReceiverName = pddOrderInfo.ReceiverName,
                ReceiverPhone = pddOrderInfo.ReceiverPhone,
                Remark = pddOrderInfo.Remark,
                BuyerMemo = pddOrderInfo.BuyerMemo,
                OrderStatus = pddOrderInfo.OrderStatus ?? 0,
                AfterSalesStatus = pddOrderInfo.AfterSalesStatus ?? 0,
                OrderMoney = details.Select(x => x.GoodsPrice * x.GoodsCount).Sum(),
                SelfOrderStatus = GetSelfOrderStatus(pddOrderInfo)
            };
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    DbContext.Order.Add(order);
                    DbContext.SaveChanges();
                    orderId = order.Id;
                    details.ForEach(detail =>
                    {
                        detail.OrderId = order.Id;
                        DbContext.OrderDetail.Add(detail);
                    });
                    DbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    orderId = 0;
                    Logger.LogError(ex, "订单保存失败");
                    transaction.Rollback();
                }
            }
        }


    }
}
