using Microsoft.Extensions.Logging;
using PddOpenSDK;
using PddOpenSDK.Exception;
using PddOpenSDK.Models.PmcUserPermit;
using PddOpenSDK.Models.Request.Mall;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace PddService.Code.Manage
{
    public class MallManage
    {
        private PddClient pddClient;
        private PddDbContext dbContext;
        private ILogger<MallManage> logger;
        private ITokenContainer tokenContainer;
        public MallManage(PddClient pddClient, PddDbContext dbContext, ILogger<MallManage> logger, ITokenContainer tokenContainer)
        {
            this.pddClient = pddClient;
            this.dbContext = dbContext;
            this.logger = logger;
            this.tokenContainer = tokenContainer;
        }
        public Mall UpdateMallMsgByAccessToken(int userId, string ownerId, string owerName, string accessToken, string refreshToken, int expiresIn = 0)
        {
            if (!dbContext.Mall.Any(x => x.UserId == userId && x.OwnerId == ownerId))
            {
                var mallResponse = pddClient.Request(new GetMallInfoRequestModel(), accessToken);
                var merchantType = string.Empty;
                switch (mallResponse.MallInfoGetResponse.MerchantType)
                {
                    case 1:
                        {
                            merchantType = "个人";
                        }
                        break;
                    case 2:
                        {
                            merchantType = "企业";
                        }
                        break;
                    case 3:
                        {
                            merchantType = "旗舰店";
                        }
                        break;
                    case 4:
                        {
                            merchantType = "专卖店";
                        }
                        break;
                    case 5:
                        {
                            merchantType = "专营店";
                        }
                        break;
                    case 6:
                        {
                            merchantType = "普通店";
                        }
                        break;
                    default:
                        {
                            merchantType = "未知";
                        }
                        break;
                }
                dbContext.Mall.Add(new Mall
                {
                    UserId = userId,
                    OwnerId = ownerId,
                    OwnerName = owerName,
                    MallName = mallResponse.MallInfoGetResponse.MallName,
                    MerchantType = merchantType,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpireTime = DateTime.Now.AddSeconds(expiresIn),
                    CreatedTime=DateTime.Now,
                    AutoSendOut=true
                });
                PermitUserMessagePush(accessToken);
            }
            else
            {
                dbContext.Mall.Where(x => x.UserId == userId && x.OwnerId == ownerId).Update(x => new Mall
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpireTime = DateTime.Now.AddSeconds(expiresIn)
                });
            }
            dbContext.SaveChanges();
            var mallInfo = dbContext.Mall.FirstOrDefault(x => x.UserId == userId && x.OwnerId == ownerId);
            return mallInfo;
        }

        public void UpdateAccessToken(string owerId, int userId = 0)
        {
            var query = dbContext.Mall.Where(x => x.OwnerId == owerId);
            if (userId > 0)
            {
                query = query.Where(x => x.UserId == userId);
            }
            var mallInfo = query.FirstOrDefault();
            if (mallInfo == null)
            {
                throw new NullReferenceException("没有找到该店铺信息");
            }
            if (mallInfo.ExpireTime.HasValue && mallInfo.ExpireTime.Value <= DateTime.Now)
            {
                throw new OAuthExpiresException(mallInfo.OwnerName, mallInfo.MallName, mallInfo.MerchantType);
            }
            var response = pddClient.RefreshToken(mallInfo.RefreshToken);
            UpdateMallMsgByAccessToken(mallInfo.UserId, mallInfo.OwnerId, mallInfo.OwnerName, response.AccessToken, response.RefreshToken, response.ExpiresIn);

        }
        public void PermitUserMessagePush(string accessToken)
        {
            try
            {
                var response = pddClient.Request(new PddPmcUserPermitRequest(), accessToken);
                logger.LogDebug($"开通用户消息服务：{(response.isSuccess ? "成功" : "失败")}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"开通消息服务异常");
            }

        }
    }
}
