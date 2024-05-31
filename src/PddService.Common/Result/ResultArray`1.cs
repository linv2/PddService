using System;
using System.Collections.Generic;
using System.Text;

namespace PddService.Common.Result
{
    public class ResultArray<T> : Result<IEnumerable<T>>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }
    }
}