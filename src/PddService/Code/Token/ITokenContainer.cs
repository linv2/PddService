using PddService.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Code
{
    public interface ITokenContainer
    {
        Mall this[long id] { get;}
        Mall GetMall(int mallId);
    }
}
