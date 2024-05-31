using System;
using System.Collections.Generic;
using System.Text;

namespace PddOpenSDK.Attribute
{
    internal class PddRequestMethodAttribute : System.Attribute
    {
        public string TypeName { get; }
        public PddRequestMethodAttribute(string typeName)
        {
            TypeName = typeName;
        }
    }
}
