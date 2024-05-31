using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PddService.DataAccess.Entity
{
    /// <summary>
    /// 地址
    /// </summary>
    [DataContract]
    public class Address
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [Required]
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [Required]
        public string City { get; set; }
        /// <summary>
        /// 区，乡镇
        /// </summary>
        [Required]
        public string Town { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [Required, MaxLength(200)]
        public string Detail { get; set; }

        /// <summary>
        /// 名字
        /// </summary>

        [Required, MaxLength(20)]
        public string Name { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Required, MaxLength(20)]
        public string Mobile { get; set; }

        /// <summary>
        /// 是否默认地址
        /// </summary>
        [Required]
        public bool Default { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Required]
        public bool Delete { get; set; }
    }
}
