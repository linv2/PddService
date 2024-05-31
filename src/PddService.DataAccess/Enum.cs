using System;
using System.Collections.Generic;
using System.Text;

namespace PddService.DataAccess
{
    /// <summary>
    /// 平台自身的状态标识
    /// </summary>
    public enum SelfOrderStatus
    {
        /// <summary>
        /// 订单已经创建
        /// </summary>
        Created,
        /// <summary>
        /// 订单已发货，但是未同步到多多平台
        /// </summary>
        Waybill,
        /// <summary>
        /// 订单已经发货，并同步到多多平台
        /// </summary>
        WaybillNotityed,
        /// <summary>
        /// 订单有退款发生
        /// </summary>
        Refund,
        /// <summary>
        /// 订单已完成
        /// </summary>
        Complate
    }


}
