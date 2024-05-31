using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Code.PddSubscribeHandle
{
    public interface ISubscribeHandle
    {

        string MessageTypeName { get; }
        bool MessageHandle(string messageBody);
    }
}
