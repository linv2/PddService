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
    /// 
    /// </summary>
    [DataContract]
    public class Mall
    {
        [Key]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }
        /// <summary>
        /// 多多店铺Id
        /// </summary>
        [Required, MaxLength(20)]
        public string OwnerId { get; set; }
        /// <summary>
        /// 多多账号名
        /// </summary>
        [Required, MaxLength(50)]
        public string OwnerName { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        [Required, MaxLength(50)]
        public string MallName { get; set; }

        /// <summary>
        /// 店铺类型,1:个人 2:企业 3:旗舰店 4:专卖店 5:专营店 6:普通店
        /// </summary>
        [Required, MaxLength(20)]
        public string MerchantType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [MaxLength(50)]
        public string AccessToken { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpireTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [MaxLength(50)]
        [JsonIgnore]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 发货地址
        /// </summary>
        public int? AddressId { get; set; }
        [ForeignKey("AddressId")]
        [JsonIgnore]
        public Address Address { get; set; }
        /// <summary>
        /// 打印快递单配置
        /// </summary>
        public int? PrintTemplateId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("PrintTemplateId")]
        [JsonIgnore]
        public PrintTemplate PrintTemplate { get; set; }


        /// <summary>
        /// 自动发货
        /// </summary>
        [Required]
        public bool AutoSendOut { get; set; }


        /// <summary>
        /// 自动通知
        /// </summary>
        [Required]
        public bool AutoNotity { get; set; }

        [Required]
        public int AutoSendOutStartTime { get; set; }
        [Required]
        public int AutoSendOutEndTime { get; set; }

        /// <summary>
        /// 未发货自动同意退款
        /// </summary>
        [Required]
        public bool AutoCancel { get; set; }



        public DateTime? LastSyncTime { get; set; }
        public DateTime? LastOrderTime { get; set; }
    }
}
