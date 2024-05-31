using System;
using System.Collections.Generic;
using System.Text;

namespace PddService.Common
{
    public interface IRequestTrace
    {
        void Trace(string propertyName = "reqId");
        void Clear(string propertyName = "reqId");
    }
}
