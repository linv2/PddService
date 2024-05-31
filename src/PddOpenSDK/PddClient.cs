using System;
using PddOpenSDK.Models;
using PddOpenSDK.Models.Request;
using PddOpenSDK.Models.Response;

namespace PddOpenSDK
{
    public abstract class PddClient
    {
        public  string ClientId { get; protected set; }
        public string ClientSecret { get; protected set; }


        public abstract TResponse Request< TResponse>(PddRequestModel<TResponse> request,string accessToken=null)
            where TResponse :PddResponseModel;

        public abstract AccessTokenResponseModel AccessToken(string code, string redirect_uri, string state=null);

        public abstract AccessTokenResponseModel RefreshToken(string refresh_token);
    }
}
