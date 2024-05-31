using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.DTO
{

    /// <summary>
    /// 发货人信息
    /// </summary>
    public class UserInfoDTO
    {
        /// <summary>
     /// 发货地址
     /// </summary>
        [JsonProperty("address")]
        public AddressDTO Address { get; set; }
        /// <summary>
        /// 手机号码（手机号和固定电话不能同时为空）
        /// </summary>
        [JsonProperty("mobile")]
        public String Mobile { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [JsonProperty("name")]
        public String Name { get; set; }
        /// <summary>
        /// 固定电话（手机号和固定电话不能同时为空）
        /// </summary>
        [JsonProperty("phone")]
        public String Phone { get; set; }
    }
}
