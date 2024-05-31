using System;
using System.Collections.Generic;
using System.Text;
using PddOpenSDK.Models;

namespace PddOpenSDK.Exception
{
    public class RequestException : System.Exception
    {
        public ErrorResponse ErrorResponse { get; }

        public RequestException(ErrorResponse errorResponse):base(errorResponse.ErrorMsg)
        {
            ErrorResponse = errorResponse;
        }
    }
}
