using PddOpenSDK.Attribute;
using PddOpenSDK.Models.Response.Refund;

namespace PddOpenSDK.Models.Request.Refund
{
    [PddRequestMethod("pdd.refund.address.list.get")]
    public partial class GetRefundAddressListRequestModel : PddRequestModel<GetRefundAddressListResponseModel>
    {

    }

}
