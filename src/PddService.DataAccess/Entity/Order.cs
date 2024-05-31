using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PddService.DataAccess.Entity
{
    [DataContract]
    public class Order
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
        [DataMember]
        public int MallId { get; set; }
        [ForeignKey("MallId")]
        
        public Mall Mall { get; set; }
        #endregion


        #region 订单店铺信息
        /// <summary>
        /// 多多店铺Id
        /// </summary>
        [Required, MaxLength(20)]
        
        public string OwnerId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Required, MaxLength(50)]
        
        public string OrderSn { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        [Required]
        
        public int OrderNum { get; set; }

        [Required]
        
        public double OrderMoney { get; set; }
        /// <summary>
        /// 订单创建时间
        /// </summary>
        [Required]
        
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 成交时间
        /// </summary>
        [Required]
        
        public DateTime ConfirmTime { get; set; }
        #endregion

        #region 收件人信息

        /// <summary>
        /// 国家或地区
        /// </summary>
        [Required, MaxLength(20)]
        
        public string Country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        [Required, MaxLength(20)]
        
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [Required, MaxLength(20)]
        
        public string City { get; set; }
        /// <summary>
        /// 区，乡镇
        /// </summary>
        [Required, MaxLength(20)]
        
        public string Town { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [Required, MaxLength(200)]
        
        public string Address { get; set; }
        /// <summary>
        /// 收件人姓名
        /// </summary>
        [Required, MaxLength(50)]
        
        public string ReceiverName { get; set; }
        /// <summary>
        /// 收件人电话，仅订单状态=待发货状态下返回明文，其他状态下返回脱敏手机号，例如“1387677****”
        /// </summary>
        [Required, MaxLength(50)]
        
        public string ReceiverPhone { get; set; }


        /// <summary>
        /// 订单备注
        /// </summary>
        [MaxLength(500)]
        
        public string Remark { get; set; }
        /// <summary>
        /// 买家留言信息
        /// </summary>
        [MaxLength(500)]
        
        public string BuyerMemo { get; set; }
        #endregion
        /// <summary>
        /// 快递单号
        /// </summary>
        [MaxLength(20)]
        
        public string TrackingNumber { get; set; }

        [Column(name: "express_id")]
        
        public int? ExpressId { get; set; }
        [ForeignKey("ExpressId")]
        
        public Express Express { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        
        public DateTime? ShippingTime { get; set; }

        #region 订单状态
        /// <summary>
        /// 订单发货状态，1：待发货，2：已发货待签收，3：已签收 0：异常
        /// </summary>
        [Required]
        
        public int OrderStatus { get; set; }
        /// <summary>
        /// 订单售后状态，1：无售后或售后关闭，2：售后处理中，3：退款中，4：退款成功，0：异常
        /// </summary>
        
        public int RefundStatus { get; set; }

        /// <summary>
        /// 退款时间
        /// </summary>
        
        public  DateTime? RefundTime { get; set; }

        /// <summary>
        /// 后状态 0：无售后 2：买家申请退款，待商家处理 3：退货退款，待商家处理 4：商家同意退款，退款中 5：平台同意退款，退款中 6：驳回退款， 待买家处理 7：已同意退货退款,待用户发货 8：平台处理中 9：平台拒 绝退款，退款关闭 10：退款成功 11：买家撤销 12：买家逾期未处 理，退款失败 13：买家逾期，超过有效期 14 : 换货补寄待商家处理 15:换货补寄待用户处理 16:换货补寄成功 17:换货补寄失败 18:换货补寄待用户确认完成
        /// </summary>
        
        public int AfterSalesStatus { get; set; }
        #endregion

        
        public List<OrderDetail> OrderDetail { get; set; }


        [MaxLength(500)]
        
        public string WaybillErrorText { get; set; }
        
        public string PrintData { get; set; }

        #region 自身状态
        /// <summary>
        /// 打印状态
        /// </summary>
        [Required]
        
        public bool PrintStatus { get; set; }
        /// <summary>
        /// 发货状态
        /// </summary>
        [Required]
        
        public bool WaybillStatus { get; set; }
        
        public bool SyncStatus { get; set; }
        
        public DateTime? SyncTime { get; set; }

        
        public SelfOrderStatus SelfOrderStatus { get; set; }
        #endregion

    }
}
