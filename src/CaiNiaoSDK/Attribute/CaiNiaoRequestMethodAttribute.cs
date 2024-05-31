using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Attribute
{
    internal class CaiNiaoRequestMethodAttribute : System.Attribute
    {
        public string TypeName { get; }
        public CaiNiaoRequestMethodAttribute(string typeName)
        {
            TypeName = typeName;
        }
    }
}
