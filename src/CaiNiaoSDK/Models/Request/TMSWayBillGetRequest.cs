using CaiNiaoSDK.Attribute;
using CaiNiaoSDK.Models.DTO;
using CaiNiaoSDK.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.Request
{
    /// <summary>
    /// 电子面单云打印取号接口
    /// <para>接口文档https://global.link.cainiao.com/#/homepage/api/link/merchant_electronic_sheet/TMS_WAYBILL_GET?_k=7a0klb</para>
    /// </summary>
    [CaiNiaoRequestMethod("TMS_WAYBILL_GET")]
    public class TMSWaybillGetRequest:CaiNiaoRequestModel<TMSWaybillGetResponse>
    {
        [JsonProperty("cpCode")]
        public string CpCode { get; set; }
        /// <summary>
        /// 发货人信息
        /// </summary>
        [JsonProperty("sender")]
        public UserInfoDTO Sender { get; set; }


        /// <summary>
        /// 	        请求面单列表（上限10个）
        /// </summary>
        [JsonProperty("tradeOrderInfoDtos")]
        public List<TradeOrderInfoDTO> TradeOrderInfoDtos { get; set; }


        /// <summary>
        /// 特殊业务使用，如需使用请联系对接人员。
        /// </summary>
        [JsonProperty("storeCode")]
        public String StoreCode { get; set; } = "无";

        /// <summary>
        /// 特殊业务使用，如需使用请联系对接人员。
        /// </summary>
        [JsonProperty("resourceCode")]
        public String ResourceCode { get; set; } = "无";
        /// <summary>
        /// 特殊业务使用，如需使用请联系对接人员。
        /// </summary>
        [JsonProperty("dmsSorting")]
        public bool DmsSorting { get; set; } 

        /// <summary>
        /// 云打印数据是否加密（true-加密;false-不加密）
        /// </summary>
        [JsonProperty("needEncrypt")]
        public bool NeedEncrypt { get; set; }
    }
}
