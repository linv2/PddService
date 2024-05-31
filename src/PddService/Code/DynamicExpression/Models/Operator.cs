using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Code.DynamicExpression.Models
{
    public enum Operator
    {
        And,
        Or,
        Equal,
        NotEqual,
        Greate,
        Less,
        Greaterorequal,
        Lessorequal,
        Like,
        In,
        NotIn
    }
}
