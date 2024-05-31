using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace PddService.DataAccess.Entity
{
    [DataContract]
    public class Express
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 快递公司名称
        /// </summary>
        [Required, MaxLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// 物流公司代码
        /// </summary>
        [Required, MaxLength(20)]
        public string PddCode { get; set; }


        /// <summary>
        /// 多多的公司ID
        /// </summary>
        [Required]
        public int PddLogisticsId { get; set; }
        /// <summary>
        /// 资源编码（发货使用）
        /// </summary>
        [Required, MaxLength(20)]
        public string CaiNiaoCode { get; set; }

        /// <summary>
        /// 公司编码（取号使用）
        /// </summary>
        [Required,MaxLength(20)]
        public string CaiNiaoResCode { get; set; }
    }
}
