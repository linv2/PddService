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
    /// 异步任务
    /// </summary>
    public class AsyncTask
    {
        [Key]
        public int Id { get; set; }

        #region 外键
        [Column(name: "user_id")]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }

        #endregion

        /// <summary>
        /// 任务Key
        /// </summary>
        [Required,MaxLength(100)]
        public string TaskKey { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        [Required]
        public int TaskType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        [Required]
        public int TaskStatus { get; set; }
    
        /// <summary>
        /// 任务参数
        /// </summary>
        [JsonIgnore]
        public string TaskParam { get; set; }
        /// <summary>
        /// 任务参数Hash值
        /// </summary>
        public string TaskParamHash { get; set; }

        /// <summary>
        /// 任务完成
        /// </summary>
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 任务参数
        /// </summary>
        [MaxLength(2000)]
        public string CompleteParam { get; set; }
    }
}
