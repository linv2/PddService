using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PddService.Common.Exceptions;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PddService.Code
{
    public class TokenContainer : ITokenContainer
    {
        ILogger<TokenContainer> logger;
        PddDbContext dbContext;
        public TokenContainer(ILogger<TokenContainer> logger, PddDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }


        public Mall this[long id]
        {
            get
            {
                var model = dbContext.Mall.AsNoTracking().Include(x=>x.User).FirstOrDefault(x => x.OwnerId == id.ToString());
                return model;
            }
        }

        public Mall GetMall(int mallId)
        {
            var model = dbContext.Mall.AsNoTracking().Include(x => x.User).FirstOrDefault(x => x.Id == mallId);
            return model;
        }

    }
}
