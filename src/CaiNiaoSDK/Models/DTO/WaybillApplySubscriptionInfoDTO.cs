using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.DTO
{
    /// <summary>
    /// CP网点信息及对应的商家的发货信息
    /// </summary>
    public class WaybillApplySubscriptionInfoDTO
    {
        /// <summary>
        /// CP网点信息及对应的商家的发货信息
        /// </summary>
        [JsonProperty("branchAccountCols")]
        public List<WaybillBranchAccountDTO> BranchAccountCols { get; set; }
        /// <summary>
        /// 物流服务商ID 
        /// </summary>
        [JsonProperty("cpCode")]
        public string cpCode { get; set; }
        /// <summary>
        /// 1是直营，2是加盟
        /// </summary>
        [JsonProperty("cpType")]
        public string cpType { get; set; }
    }
    public class WaybillBranchAccountDTO    
    {
        /// <summary>
        /// 已用面单数量
        /// </summary>
        [JsonProperty("allocatedQuantity")]
        public long AllocatedQuantity { get; set; }
        /// <summary>
        /// 网点Code
        /// </summary>
        [JsonProperty("branchCode")]
        public string BranchCode { get; set; }
        /// <summary>
        /// 网点名称
        /// </summary>
        [JsonProperty("branchName")]
        public string BranchName { get; set; }
        /// <summary>
        /// 网点状态    
        /// </summary>
        [JsonProperty("branchStatus")]
        public int BranchStatus { get; set; }
        /// <summary>
        ///  取消的面单总数
        /// </summary>
        [JsonProperty("cancelQuantity")]
        public long CancelQuantity { get; set; }
        /// <summary>
        /// 已经打印的面单总数
        /// </summary>
        [JsonProperty("printQuantity")]
        public long PrintQuantity { get; set; }
        /// <summary>
        /// 电子面单余额数量    
        /// </summary>
        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        /// <summary>
        /// 当前网点下的发货地址
        /// </summary>
        [JsonProperty("shippAddressCols")]
        public List<AddressDTO> shippAddressCols { get; set; }

        /// <summary>
        /// 可用的服务信息列表
        /// </summary>
        [JsonProperty("serviceInfoCols")]
        public List<ServiceInfoDTO> serviceInfoCols { get; set; }
    }
    public class ServiceInfoDTO
    {

        /// <summary>
        /// 服务名称
        /// </summary>
        [JsonProperty("serviceName")]
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务编码
        /// </summary>
        [JsonProperty("serviceCode")]
        public string ServiceCode { get; set; }
    }
}
