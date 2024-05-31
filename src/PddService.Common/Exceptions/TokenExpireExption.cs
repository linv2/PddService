using System;
using System.Collections.Generic;
using System.Text;

namespace PddService.Common.Exceptions
{
   public class TokenExpireExption:Exception
    {
        public TokenExpireExption() : base("access_token已过期")
        {

        }
    }
}
