using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PddService.Common.Result
{
    public class Result
    {
        public static Result Ok => Success();
        /// <summary>
        /// <![CDATA[错误码]]>
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///<![CDATA[消息]]>
        /// </summary>
        public string Message { get; set; }

        public static Result Success(string message="操作成功",int code = 200)
        {
            return new Result()
            {
                Code=code,
                Message=message
            };
        }
        public static Result Fail(string message = "操作失败", int code = 400)
        {
            return new Result()
            {
                Code = code,
                Message = message
            };
        }
        public static Result<T> Success<T>(T data,string message = "操作成功", int code = 200)
        {
            return new Result<T>()
            {
                Data= data,
                Code = code,
                Message = message
            };
        }
        public static ResultArray<T> Success<T>(IEnumerable<T> enumerable, int total)
        {
            return new ResultArray<T>()
            {
                Data = enumerable,
                Total=total,
                Code = 200,
            };
        }
        [JsonIgnore]
        public IActionResult ActionResult
        {
            get
            {
                return new OkObjectResult(this);
            }
        }
    }
}
