using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Code.DynamicExpression.Models
{
    public class PaginationInfo
    {
        public string Name { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public Condition Condition { get; set; }
        public Sort Sort { get; set; } = new Sort()
        {
            Field = "Id",
            Asc = false
        };
        public IEnumerable<Condition> Groups { get; set; }
    }
}
