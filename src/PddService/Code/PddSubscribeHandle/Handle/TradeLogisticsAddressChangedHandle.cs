using Microsoft.Extensions.Logging;
using PddOpenSDK;
using PddOpenSDK.Subscribe.Order;
using PddService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Code.PddSubscribeHandle.Handle
{
    /// <summary>
    /// 地址修改
    /// </summary>
    public class TradeLogisticsAddressChangedHandle : AbsSubscribeHandle<PddTradeNotityModel>
    {
        public TradeLogisticsAddressChangedHandle(ILogger<TradeLogisticsAddressChangedHandle> logger, PddDbContext dbContext, PddClient pddClient, ITokenContainer tokenContainer) : base(logger, dbContext, pddClient, tokenContainer)
        {
        }

        public override string MessageTypeName => "pdd_trade_TradeLogisticsAddressChanged";

        public override bool Execute(PddTradeNotityModel tradeNotityModel)
        {
            var mallInfo = TokenContainer[tradeNotityModel.MallId];
            return true;
        }
    }
}
