using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.DTO
{
    public class PackageInfoDTO
    {

        /// <summary>
        /// 包裹id，用于拆合单场景（只能传入数字、字母和下划线；批量请求时值不得重复，大小写敏感，即123A,123a 不可当做不同ID，否则存在一定可能取号失败）
        /// </summary>
        [JsonProperty("id")]
        public String Id { get; set; }
        /// <summary>
        /// 商品信息,数量限制为100
        /// </summary>
        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        /// <summary>
        /// 体积, 单位 ml
        /// </summary>
        [JsonProperty("volume")]
        public long? Volume { get; set; }
        /// <summary>
        /// 重量,单位 g
        /// </summary>
        [JsonProperty("weight")]
        public long? Weight { get; set; }
    }
}
