using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace PddService.DataAccess.Entity
{
    [DataContract]
    public class PrintTemplate
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }
        [Required,MaxLength(50)]
        public string Name { get; set; }

        [Column(name: "express_id")]
        public int ExpressId { get; set; }
        [ForeignKey("ExpressId")]
        public Express Express { get; set; }

        [Required,MaxLength(200)]
        public string SourceUrl { get; set; }
        [Required]
        public string Content { get; set; }
        [MaxLength(200)]
        public string ContentUrl { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }
        
        /// <summary>
        /// 尺寸
        /// </summary>
        [Required,MaxLength(15)]
        public string Size { get; set; }

        [Required,JsonIgnore]
        public bool Delete { get; set; }

        [Required]
        public bool Default { get; set; }
    }
}
