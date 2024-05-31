using CaiNiaoSDK;
using Microsoft.Extensions.Logging;
using PddOpenSDK;
using PddOpenSDK.Models.Request.Order;
using PddService.Common;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Code.Manage.Order
{
    public class OrderSyncManage
    {
        private ILogger<OrderSyncManage> logger;
        private PddDbContext dbContext;
        private PddClient pddClient;
        private ITokenContainer tokenContainer;
        private SiteConfig siteConfig;
        public OrderSyncManage(ILogger<OrderSyncManage> logger, PddDbContext dbContext, PddClient pddClient, ITokenContainer tokenContainer, SiteConfig siteConfig)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.pddClient = pddClient;
            this.tokenContainer = tokenContainer;
            this.siteConfig = siteConfig;
        }
        public CollectModel Sync(int mallId, DateTime? startTime = null, DateTime? endTime = null, CollectModel collectModel = null)
        {

            logger.LogInformation("订单同步开始执行");
            if (collectModel == null)
            {
                collectModel = new CollectModel();
            }
            try
            {
                var mallInfo = tokenContainer.GetMall(mallId);
                if (!startTime.HasValue)
                {
                    startTime = mallInfo.LastOrderTime ?? DateTime.Now.AddMinutes(-30);
                }
                if (!endTime.HasValue)
                {
                    endTime = DateTime.Now;
                }
                var request = new GetOrderListRequestModel()
                {
                    OrderStatus = 5,
                    RefundStatus = 5,
                    StartConfirmAt = startTime.Value.GetTimestamp(),
                    EndConfirmAt = endTime.Value.GetTimestamp(),
                    Page = 1,
                    PageSize = 100,
                    UseHasNext = true
                };
                var response = pddClient.Request(request, mallInfo.AccessToken);
                collectModel.Total += response.OrderListGetResponse.OrderList.Count;
                logger.LogInformation($"MallId={mallId}，共获取到{response.OrderListGetResponse.OrderList.Count}条订单，下一页{(response.OrderListGetResponse.HasNext ?? false ? "还有" : "无")}数据");
                foreach (var pddOrderInfo in response.OrderListGetResponse.OrderList)
                {
                    try
                    {
                        if (dbContext.Order.Any(x => x.OrderSn == pddOrderInfo.OrderSn && x.MallId == mallId))
                        {

                            logger.LogInformation($"店MallId={mallId}，订单号={pddOrderInfo.OrderSn}的订单已存在，忽略");
                            continue;
                        }
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
                                GoodsCount = orderItem.GoodsCount ?? 1,
                                GoodsSpec = orderItem.GoodsSpec,
                                SkuId = orderItem.SkuId,
                                GoodsImg = orderItem.GoodsImg
                            };
                            details.Add(orderDetail);

                        }
                        var order = new DataAccess.Entity.Order()
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
                            RefundStatus = pddOrderInfo.RefundStatus ?? 0,
                            AfterSalesStatus = pddOrderInfo.AfterSalesStatus ?? 0,
                            OrderMoney = details.Select(x => x.GoodsPrice * x.GoodsCount).Sum(),
                        };
                        using (var transaction = dbContext.Database.BeginTransaction())
                        {
                            try
                            {
                                dbContext.Order.Add(order);
                                dbContext.SaveChanges();
                                details.ForEach(detail =>
                                {
                                    detail.OrderId = order.Id;
                                    dbContext.OrderDetail.Add(detail);
                                });
                                dbContext.SaveChanges();
                                transaction.Commit();
                                collectModel.Success();
                            }
                            catch
                            {
                                transaction.Rollback();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex,$"MallId={mallId}，订单号={pddOrderInfo.OrderSn}的订单同步失败");
                        collectModel.Fail();
                    }
                }
                logger.LogInformation($"MallId={mallId}，订单同步当前页面执行结束，下一页：{((response.OrderListGetResponse.HasNext ?? false) ? "有" : "无")}数据");
                if (response.OrderListGetResponse.HasNext ?? false)
                {
                    var maxTime = response.OrderListGetResponse.OrderList.Max(x => Convert.ToDateTime(x.ConfirmTime));
                    Sync(mallId, maxTime, endTime, collectModel);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "订单同步异常");
            }
            logger.LogInformation($"MallId={mallId}，订单同步执行结束：{collectModel.ToString()}");
            return collectModel;
        }
    }
}

