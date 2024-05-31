using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Code.DynamicExpression.Models
{
    public class Condition
    {
        public Rule[] Rules { get; set; }
        public Operator Op { get; set; } = Operator.And;
    }
}
