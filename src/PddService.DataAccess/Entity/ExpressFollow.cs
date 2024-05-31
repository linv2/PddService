using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace PddService.DataAccess.Entity
{
    /// <summary>
    /// 快递跟踪
    /// </summary>
    [DataContract]
    public class ExpressFollow
    {
        [Key]
        public int Id { get; set; }


       
        /// <summary>
        /// 快递公司ID
        /// </summary>
        [Required]
        public long ShippingId { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        [Required,MaxLength(20)]
        public string TrackingNumber { get; set; }

        /// <summary>
        /// 状态发生的时间
        /// </summary>
        [Required]
        public DateTime StatusTime { get; set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        [Required, MaxLength(200)]
        public string StatusDesc { get; set; }

        /// <summary>
        /// 节点说明 ，指明当前节点揽收、派送，签收
        /// </summary>
        [Required, MaxLength(50)]
        public string Action { get; set; }

        /// <summary>
        /// 节点说明 ，指明当前节点揽收、派送，签收
        /// </summary>
        [Required, MaxLength(50)]
        public string NodeDescription { get; set; }

        /// <summary>
        /// 扫描时间
        /// </summary>
        [Required]
        public DateTime ScanTime { get; set; }

        /// <summary>
        /// 轨迹详细信息
        /// </summary>
        [Required,MaxLength(500)]
        public string Desc { get; set; }
    }
}
