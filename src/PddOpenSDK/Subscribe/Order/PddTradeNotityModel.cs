using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PddOpenSDK.Subscribe.Order
{
    /// <summary>
    /// PddTradeNotityModel(交易成功消息)
    /// </summary>
    public class PddTradeNotityModel
    {
        /// <summary>
        /// 店铺id
        /// </summary>
        [JsonProperty("mall_id")]
        public long MallId { get; set; }
        /// <summary>
        /// 店铺id
        /// </summary>
        [JsonProperty("tid")]
        public string OrderSn { get; set; }
    }
}
