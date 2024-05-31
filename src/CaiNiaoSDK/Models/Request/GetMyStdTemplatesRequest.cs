using CaiNiaoSDK.Attribute;
using CaiNiaoSDK.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiNiaoSDK.Models.Request
{
    [CaiNiaoRequestMethod("cainiao.cloudprint.mystdtemplates.get")]
    public class GetMyStdTemplatesRequest:CaiNiaoRequestModel<GetMyStdTemplatesResponse>
    {
    }
}
