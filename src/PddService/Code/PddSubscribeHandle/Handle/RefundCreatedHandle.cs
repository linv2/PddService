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
    /// 退款创建消息
    /// </summary>
    public class RefundCreatedHandle : AbsSubscribeHandle<PddRefundCreatedModel>
    {
        public RefundCreatedHandle(ILogger<RefundCreatedHandle> logger, PddDbContext dbContext, PddClient pddClient, ITokenContainer tokenContainer) : base(logger, dbContext, pddClient, tokenContainer)
        {
        }

        public override string MessageTypeName => "pdd_refund_RefundCreated";

        public override bool Execute(PddRefundCreatedModel refundCreated)
        {
            try
            {
                var orderInfo = DbContext.Order.FirstOrDefault(x => x.OwnerId == refundCreated.MallId && x.OrderSn == refundCreated.Tid);
                if (orderInfo == null)
                {
                    Logger.LogWarning("退款订单不存在， 店铺ID={0} 订单号={1}", refundCreated.MallId, refundCreated.Tid);
                    return true;
                }
                var mallInfo = TokenContainer.GetMall(orderInfo.MallId);
                if (mallInfo == null)
                {
                    Logger.LogWarning("退款店铺不存在或者AccessToken过期， 店铺ID={0} 订单号={1}", refundCreated.MallId, refundCreated.Tid);
                    return true;
                }
                if (!DbContext.OrderRefund.Any(x => x.MallId == mallInfo.Id && x.RefundId == refundCreated.RefundId))
                {
                    using (var transaction = DbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            DbContext.OrderRefund.Add(new OrderRefund()
                            {
                                UserId = mallInfo.UserId,
                                MallId=mallInfo.Id,
                                OrderId=orderInfo.Id,
                                OrderSn=orderInfo.OrderSn,
                                OrderMoney=orderInfo.OrderMoney,
                                OrderNum=orderInfo.OrderNum,
                                RefundId= refundCreated.RefundId,
                                BillType= refundCreated.BillType,
                                Operation= refundCreated.Operation,
                                RefundFee= refundCreated.RefundFee,
                                CreatedTime=DateTime.Now,
                                Status=0,

                            });
                            DbContext.Order.Where(x => x.MallId == mallInfo.Id && x.OrderSn == refundCreated.Tid).Update(x => new Order()
                            {
                                RefundStatus = 3,
                                SelfOrderStatus = SelfOrderStatus.Refund,
                                RefundTime = DateTime.Now
                            });

                            DbContext.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, $"退款订单保存失败");
                            transaction.Rollback();
                        }
                    }
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
