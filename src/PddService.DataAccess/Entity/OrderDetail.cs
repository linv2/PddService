using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using Z.BulkOperations;

namespace PddService.DataAccess.Entity
{
    [DataContract]
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        [Column(name: "user_id")]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }

        [Column(name: "mall_id")]
        public int MallId { get; set; }
        [ForeignKey("MallId")]
        public Mall Mall { get; set; }

        [Column(name: "order_id")]
        public int OrderId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Required, MaxLength(50)]
        public string OrderSn { get; set; }
        [Required, MaxLength(200)]
        public string GoodsName { get; set; }
        [MaxLength(300)]
        public string GoodsImg { get; set; }
        [Required]
        public double GoodsPrice { get; set; }
        [Required]
        public int GoodsCount { get; set; }
        [MaxLength(500)]
        public string GoodsSpec { get; set; }
        [MaxLength(50)]
        public string SkuId { get; set; }
    }
}
