using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.DTO
{
    public class TradeOrderInfoDTO
    {
        /// <summary>
        /// <![CDATA[物流服务值（详见https://support-cnkuaidi.taobao.com/doc.htm#?docId=106156&docType=1，如无特殊服务请置空）]]>
        /// </summary>
        [JsonProperty("logisticsServices")]
        public String LogisticsServices { get; set; }
        /// <summary>
        /// 请求ID（请保证一次批量请求中值不重复，调用方可按该值从返回数据中获取相应结果；只限传入数字、字母和下划线，建议用数字序号下标代替）
        /// </summary>
        [JsonProperty("objectId")]
        public String ObjectId { get; set; }
        /// <summary>
        /// 订单信息
        /// </summary>
        [JsonProperty("orderInfo")]
        public OrderInfoDTO OrderInfo { get; set; }
        /// <summary>
        /// 包裹信息
        /// </summary>
        [JsonProperty("packageInfo")]
        public PackageInfoDTO PackageInfo { get; set; }
        /// <summary>
        /// 收件人信息
        /// </summary>
        [JsonProperty("recipient")]
        public UserInfoDTO Recipient { get; set; }
        /// <summary>
        /// 云打印标准模板URL（组装云打印结果使用，值格式http://cloudprint.cainiao.com/template/standard/${模板ID}）
        /// </summary>
        [JsonProperty("templateUrl")]
        public String TemplateUrl { get; set; }
        /// <summary>
        ///使用者ID（使用电子面单账号的实际商家ID，如存在一个电子面单账号多个店铺使用时，请传入店铺的商家ID
        /// </summary>
        [JsonProperty("userId")]
        public long UserId { get; set; }
    }
}
