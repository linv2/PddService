using Microsoft.Extensions.Logging;
using PddOpenSDK;
using PddOpenSDK.Subscribe.Order;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace PddService.Code.PddSubscribeHandle.Handle
{
    /// <summary>
    /// 交易成功
    /// </summary>
    public class TradeSuccessHandle : AbsSubscribeHandle<PddTradeNotityModel>
    {

        public TradeSuccessHandle(ILogger<TradeSuccessHandle> logger, PddClient pddClient, PddDbContext dbContext, ITokenContainer tokenContainer) : base(logger, dbContext, pddClient, tokenContainer)
        {
        }
        public override string MessageTypeName => "pdd_trade_TradeSuccess";


        public override bool Execute(PddTradeNotityModel tradeNotityModel)
        {
            try
            {
                var mallInfo = TokenContainer[tradeNotityModel.MallId];
                DbContext.Order.Where(x => x.MallId == mallInfo.Id && x.OrderSn == tradeNotityModel.OrderSn).Update(x => new Order()
                {
                    OrderStatus = 3,
                    SelfOrderStatus = SelfOrderStatus.Complate
                });
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "未捕获的异常发生");
            }
            return true;
        }
    }
}
