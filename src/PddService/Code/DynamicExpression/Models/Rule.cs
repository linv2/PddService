using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Code.DynamicExpression.Models
{
    public class Rule
    {
        public string Field { get; set; }
        public string Op { get; set; } = "=";
        public string Value { get; set; }
    }
}
