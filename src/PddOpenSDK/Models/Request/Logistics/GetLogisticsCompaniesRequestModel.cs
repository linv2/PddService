using PddOpenSDK.Attribute;
using PddOpenSDK.Models.Response.Logistics;

namespace PddOpenSDK.Models.Request.Logistics
{
    [PddRequestMethod("pdd.logistics.companies.get")]
    public partial class GetLogisticsCompaniesRequestModel : PddRequestModel<GetLogisticsCompaniesResponseModel>
    {

    }

}
