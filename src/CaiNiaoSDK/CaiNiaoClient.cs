using System;
using CaiNiaoSDK.Models;
using CaiNiaoSDK.Models.Response;

namespace CaiNiaoSDK
{
    public abstract class CaiNiaoClient
    {
        public  string AppKey { get; protected set; }
        public string AppSecret { get; protected set; }


        public abstract TResponse Request<TResponse>(CaiNiaoRequestModel<TResponse> request, string accessToken = null)
            where TResponse : CaiNiaoResponseModel, new();

        public abstract AccessTokenResponse AccessToken(string code);
    }
}
