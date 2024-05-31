using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace PddService.DataAccess.Entity
{
    [DataContract]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "用户名")]
        [Required(ErrorMessage = "{0}不能为空")]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0}不能为空")]
        [MaxLength(50)]
        [JsonIgnore]
        public string PassWord { get; set; }

        [Display(Name = "名称")]
        [Required(ErrorMessage = "{0}不能为空")]
        [MaxLength(12)]
        public string DisplayName { get; set; }

        [JsonIgnore]
        public string CaiNiaoAccessToken { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 最后登录 Ip
        /// </summary>
        [MaxLength(30)]
        public string LastLoginIP { get; set; }
        /// <summary>
        /// 总登录次数
        /// </summary>
        public long LoginCount { get; set; }

        /// <summary>
        /// 是否被禁用
        /// </summary>
        [Required]
        public bool Disable { get; set; }

        /// <summary>
        /// 是否通知
        /// </summary>
        [Required]
        public bool Notity { get; set; }
        /// <summary>
        /// 通知Url
        /// </summary>
        [MaxLength(200)]
        public string NotityUrl { get; set; }

        public string NotityKey { get; set; }
    }
}
