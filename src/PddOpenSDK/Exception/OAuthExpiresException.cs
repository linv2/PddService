using System;
using System.Collections.Generic;
using System.Text;

namespace PddOpenSDK.Exception
{
   public class OAuthExpiresException:System.Exception
    {
        public string MerchantType { get;private set; }
        public string MallName { get; private set; }
        public string OwnerName { get; private set; }
        public OAuthExpiresException(string ownerName,string mallName,string merchantType) :base($"{mallName}授权已过期,请重新授权")
        {
            OwnerName = ownerName;
            MallName = mallName;
            MerchantType = merchantType;
        }
    }
}
