using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace PddService.DataAccess.Entity
{
    /// <summary>
    /// 退款
    /// </summary>
    [DataContract]
    public class OrderRefund
    {
        [Key]
        public int Id { get; set; }


        #region 外键
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
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        #endregion

        /// <summary>
        /// 订单号
        /// </summary>
        [Required, MaxLength(20)]
        public string OrderSn { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        [Required]
        public double OrderMoney { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        public int OrderNum { get; set; }
        /// <summary>
        /// 售后单id
        /// </summary>
        [Required]
        public long RefundId { get; set; }

        /// <summary>
        /// 售后类型: 1-仅退款；2-退货退款；3-换货；4-补寄；5-维修
        /// </summary>
        [Required]
        public int BillType { get; set; }
        /// <summary>
        /// 售后操作:1000-消费者申请；1001-平台客服新建；1002-平台客服开启；1003-系统创建
        /// </summary>
        [Required]
        public int Operation { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        [Required]
        public long RefundFee { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Required]
        public DateTime Modified { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 处理状态
        /// 0未处理
        /// 1同意退货
        /// 2拒绝退货
        /// 99完成退款
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 快递拦截状态
        /// </summary>
        [Required]
        public bool ExpressStatus { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? HandleTime { get; set; }

    }
}
