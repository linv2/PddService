using System;
using System.Collections.Generic;
using System.Text;

namespace PddService.Common.Result
{
    public class Result<T> : Result
    {
        /// <summary>
        /// <![CDATA[数据]]>
        /// </summary>
        public virtual T Data { get; set; }
    }
}