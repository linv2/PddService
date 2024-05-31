using Microsoft.Extensions.Logging;
using PddOpenSDK;
using PddOpenSDK.Subscribe.Order;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Code.PddSubscribeHandle.Handle
{
    /// <summary>
    /// 物流轨迹
    /// 针对使用拼多多电子面单或在拼多多平台发货的运单，增量推送轨迹。向拼多多电子面单服务取号或在拼多多平台发货时自动触发，无需订阅。
    /// </summary>
    public class TraceISVNotifyHandle : AbsSubscribeHandle<TraceISVNotifyModel>
    {
        public TraceISVNotifyHandle(ILogger<TraceISVNotifyHandle> logger, PddDbContext dbContext, PddClient pddClient, ITokenContainer tokenContainer) : base(logger, dbContext, pddClient, tokenContainer)
        {
        }


        public override string MessageTypeName => "trace_isv_notify";

        public override bool Execute(TraceISVNotifyModel notifyModel)
        {
            try
            {
                var expressFollow = new ExpressFollow()
                {
                    ShippingId = notifyModel.ShippingId,
                    TrackingNumber = notifyModel.TrackingNumber,
                    StatusTime = notifyModel.StatusTime,
                    StatusDesc = notifyModel.StatusDesc,
                    Action = notifyModel.Action,
                    NodeDescription = notifyModel.NodeDescription,
                    ScanTime = notifyModel.Time,
                    Desc = notifyModel.Desc
                };
                DbContext.ExpressFollow.Add(expressFollow);
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "保存物流轨迹异常");
            }
            return true;
        }
    }
}
