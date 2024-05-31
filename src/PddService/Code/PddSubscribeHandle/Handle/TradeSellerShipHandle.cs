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
    /// 发货
    /// </summary>
    public class TradeSellerShipHandle : AbsSubscribeHandle<PddTradeNotityModel>
    {

        public TradeSellerShipHandle(ILogger<TradeSellerShipHandle> logger, PddClient pddClient, PddDbContext dbContext, ITokenContainer tokenContainer) : base(logger, dbContext, pddClient, tokenContainer)
        {
        }

        public override string MessageTypeName => "pdd_trade_TradeSellerShip";

        public override bool Execute(PddTradeNotityModel tradeNotityModel)
        {
            try
            {
                var mallInfo = TokenContainer[tradeNotityModel.MallId];
                DbContext.Order.Where(x => x.MallId == mallInfo.Id && x.OrderSn == tradeNotityModel.OrderSn).Update(x => new Order()
                {
                    OrderStatus = 2,
                    SelfOrderStatus = SelfOrderStatus.WaybillNotityed,
                    ShippingTime = DateTime.Now
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
