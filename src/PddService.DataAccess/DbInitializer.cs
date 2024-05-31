using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PddService.DataAccess
{
    public class DbInitializer
    {
        public static void Initializer(PddDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.User.Count()==0)
            {
                context.User.Add(new Entity.User { UserName = "admin", PassWord = "admin", DisplayName = "admin" });
                var strExpress = new[] { "百世汇通,HTKY,HTKY,HT,3",
                                        "圆通,YTO,YTO,YTO,85",
                                        "中通,ZTO,ZTO,ZTO,115",
                                        "优速,UC,UC,YS,117",
                                        "国通,GTO,GTO,GTO,124",
                                        "天天,TTKDEX,TTKDEX,TT,119",
                                        "申通,STO,STO,STO,1",
                                        "韵达,YUNDA,YUNDA,YUNDA,121" };
                foreach (var express in strExpress)
                {
                    var strTemp = express.Split(',');
                    context.Express.Add(new Entity.Express() { Name = strTemp[0], CaiNiaoResCode = strTemp[1], CaiNiaoCode = strTemp[2], PddCode = strTemp[3], PddLogisticsId = Convert.ToInt32(strTemp[4]) });
                }
                context.SaveChanges();
            }

        }
    }
}
