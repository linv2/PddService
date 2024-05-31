using System;
using System.Collections.Generic;
using System.Text;

namespace PddService.Common
{
    public class CollectModel
    {
        private string successName;
        private string failName;
        private int success = 0;
        private int fail = 0;
        public int Total { get; set; }
        public CollectModel(int total=0,string successName="成功",string failName="失败")
        {
            this.Total = total;
            this.successName = successName;
            this.failName = failName;
        }
        public void Success()
        {
            success++;
        }
        public void Fail()
        {
            fail++;
        }
        public override string ToString()
        {
            return $"共{Total}条数据，执行{successName}{success}条，{failName}{fail}条";
        }
    }
}
