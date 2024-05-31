using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PddOpenSDK.Subscribe.Order
{
    public class TraceISVNotifyModel
    {
        /// <summary>
        /// 快递公司ID
        /// </summary>
        [JsonProperty("shippingId")]
        public long ShippingId { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        [JsonProperty("trackingNumber")]
        public string TrackingNumber { get; set; }

        /// <summary>
        /// 状态发生的时间
        /// </summary>
        [JsonProperty("status_time")]
        public DateTime StatusTime { get; set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        [JsonProperty("status_desc")]
        public string StatusDesc { get; set; }

        /// <summary>
        /// 节点说明 ，指明当前节点揽收、派送，签收
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        /// <summary>
        /// 节点说明 ，指明当前节点揽收、派送，签收
        /// </summary>
        [JsonProperty("node_description")]
        public string NodeDescription { get; set; }

        /// <summary>
        /// 扫描时间
        /// </summary>
        [JsonProperty("time")]
        public DateTime Time { get; set; }

        /// <summary>
        /// 轨迹详细信息
        /// </summary>
        [JsonProperty("desc")]
        public string Desc { get; set; }
    }
}
