using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PddOpenSDK.Subscribe.Order
{
    public class PddRefundCreatedModel
    {
        /// <summary>
        /// 店铺id
        /// </summary>
        [JsonProperty("mall_id")]
        public string MallId { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        [JsonProperty("refund_fee")]
        public long RefundFee { get; set; }

        /// <summary>
        /// 售后类型: 1-仅退款；2-退货退款；3-换货；4-补寄；5-维修
        /// </summary>
        [JsonProperty("bill_type")]
        public int BillType { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty("modified")]
        public long Modified { get; set; }

        /// <summary>
        /// 售后单id
        /// </summary>
        [JsonProperty("refund_id")]
        public long RefundId { get; set; }

        /// <summary>
        /// 售后操作:1000-消费者申请；1001-平台客服新建；1002-平台客服开启；1003-系统创建
        /// </summary>
        [JsonProperty("operation")]
        public int Operation { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [JsonProperty("tid")]
        public string Tid { get; set; }
    }
}
