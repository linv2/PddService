using System;
using System.Collections.Generic;
using System.Text;

namespace PddService.Common.Exceptions
{
    public class ApiException : Exception
    {
        public Result.Result Result { get; private set; }

        public ApiException(string message, int code = 400) : base(message)
        {
            Result = new Result.Result()
            {
                Code = code,
                Message = message
            };
        }
    }
}