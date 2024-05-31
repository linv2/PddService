using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.DTO
{
    /// <summary>
    /// 订单信息
    /// </summary>
    public class OrderInfoDTO
    {
        /// <summary>
        /// <![CDATA[订单渠道平台，请按实际订单所属平台传入（详见https://support-cnkuaidi.taobao.com/doc.htm#?docId=105085&docType=1）]]>
        /// </summary>
        [JsonProperty("orderChannelsType")]
        public string OrderChannelsType { get; set; }

        /// <summary>
        /// 订单号列表（上限100个）
        /// </summary>
        [JsonProperty("tradeOrderList")]
        public List<string> TradeOrderList { get; set; }
    }
}
