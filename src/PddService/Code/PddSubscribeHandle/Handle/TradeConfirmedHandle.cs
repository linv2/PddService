using Hangfire;
using Microsoft.Extensions.Logging;
using PddOpenSDK;
using PddOpenSDK.Models.Request.Order;
using PddOpenSDK.Subscribe.Order;
using PddService.Code.AsyncLogicHandler;
using PddService.Code.AsyncLogicHandler.Model;
using PddService.Code.Manage.Order;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PddOpenSDK.Models.Response.Order.GetOrderInformationResponseModel.OrderInfoGetResponseResponseModel;

namespace PddService.Code.PddSubscribeHandle.Handle
{
    public class TradeConfirmedHandle : AbsSubscribeHandle<PddTradeNotityModel>
    {
        private AsyncLogicManager asyncLogicManager;

        public TradeConfirmedHandle(ILogger<TradeConfirmedHandle> logger, PddClient pddClient, PddDbContext dbContext, ITokenContainer tokenContainer, AsyncLogicManager asyncLogicManager) : base(logger, dbContext, pddClient, tokenContainer)
        {
            this.asyncLogicManager = asyncLogicManager;
        }

        public override string MessageTypeName { get { return "pdd_trade_TradeConfirmed"; } }


        public override bool Execute(PddTradeNotityModel tradeNotityModel)
        {
            try
            {
                var mallInfo = TokenContainer[tradeNotityModel.MallId];
                if (base.UpdateByOrderSn(mallInfo, tradeNotityModel.OrderSn, out var orderId)&& mallInfo.AutoSendOut)
                {
                    var timeNow = DateTime.Now;
                    var format = timeNow.Year * 10000 + timeNow.Month * 100 + timeNow.Day;
                    if ((format >= mallInfo.AutoSendOutStartTime && format <= mallInfo.AutoSendOutEndTime) || mallInfo.AutoSendOutEndTime == 0)
                        asyncLogicManager.Push(AsyncHandlerConstant.WaybillOrder, new int[] { orderId });
                    else
                    {
                        Logger.LogInformation($"订单{tradeNotityModel.OrderSn}不再发货时间区间");
                    }

                }
                if (orderId > 0 && (mallInfo.User?.Notity ?? false))
                {
                    var userInfo = mallInfo?.User;
                    if (!string.IsNullOrEmpty(userInfo.NotityKey))
                    {
                        asyncLogicManager.Push(AsyncHandlerConstant.OrderPublish, new OrderPublishModel() { OrderId = orderId });
                    }
                    else
                    {
                        Logger.LogInformation($"UserName={mallInfo.User?.UserName} NotityKey=null，不进行推送");
                    }
                }
                else
                {
                    Logger.LogInformation($"UserName={mallInfo.User?.UserName}，未启用订单推送");
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, "未捕获的异常发生");
            }
            return true;
        }


    }
}
