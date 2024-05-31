using System.Collections.Generic;
using Newtonsoft.Json;
namespace PddOpenSDK.Models.Response.Ddk
{
    public partial class DetailDdkGoodsResponseModel : PddResponseModel
    {
        /// <summary>
        /// response
        /// </summary>
        [JsonProperty("goods_detail_response")]
        public GoodsDetailResponseResponseModel GoodsDetailResponse { get; set; }
        public partial class GoodsDetailResponseResponseModel : PddResponseModel
        {
            /// <summary>
            /// 多多进宝商品对象列表
            /// </summary>
            [JsonProperty("goods_details")]
            public List<GoodsDetailsResponseModel> GoodsDetails { get; set; }
            public partial class GoodsDetailsResponseModel : PddResponseModel
            {
                /// <summary>
                /// 店铺优惠券id
                /// </summary>
                [JsonProperty("mall_coupon_id")]
                public long? MallCouponId { get; set; }
                /// <summary>
                /// 店铺折扣
                /// </summary>
                [JsonProperty("mall_coupon_discount_pct")]
                public int? MallCouponDiscountPct { get; set; }
                /// <summary>
                /// 最小使用金额
                /// </summary>
                [JsonProperty("mall_coupon_min_order_amount")]
                public int? MallCouponMinOrderAmount { get; set; }
                /// <summary>
                /// 最大使用金额
                /// </summary>
                [JsonProperty("mall_coupon_max_discount_amount")]
                public int? MallCouponMaxDiscountAmount { get; set; }
                /// <summary>
                /// 店铺券总量
                /// </summary>
                [JsonProperty("mall_coupon_total_quantity")]
                public long? MallCouponTotalQuantity { get; set; }
                /// <summary>
                /// 店铺券余量
                /// </summary>
                [JsonProperty("mall_coupon_remain_quantity")]
                public long? MallCouponRemainQuantity { get; set; }
                /// <summary>
                /// 店铺券使用开始时间
                /// </summary>
                [JsonProperty("mall_coupon_start_time")]
                public long? MallCouponStartTime { get; set; }
                /// <summary>
                /// 店铺券使用结束时间
                /// </summary>
                [JsonProperty("mall_coupon_end_time")]
                public long? MallCouponEndTime { get; set; }
                /// <summary>
                /// 参与多多进宝的商品ID
                /// </summary>
                [JsonProperty("goods_id")]
                public long? GoodsId { get; set; }
                /// <summary>
                /// 参与多多进宝的商品标题
                /// </summary>
                [JsonProperty("goods_name")]
                public string GoodsName { get; set; }
                /// <summary>
                /// 参与多多进宝的商品描述
                /// </summary>
                [JsonProperty("goods_desc")]
                public string GoodsDesc { get; set; }
                /// <summary>
                /// 多多进宝商品主图
                /// </summary>
                [JsonProperty("goods_image_url")]
                public string GoodsImageUrl { get; set; }
                /// <summary>
                /// 商品轮播图
                /// </summary>
                [JsonProperty("goods_gallery_urls")]
                public List<string> GoodsGalleryUrls { get; set; }
                /// <summary>
                /// 最低价sku的拼团价，单位为分
                /// </summary>
                [JsonProperty("min_group_price")]
                public long? MinGroupPrice { get; set; }
                /// <summary>
                /// 最低价sku的单买价，单位为分
                /// </summary>
                [JsonProperty("min_normal_price")]
                public long? MinNormalPrice { get; set; }
                /// <summary>
                /// 店铺名称
                /// </summary>
                [JsonProperty("mall_name")]
                public string MallName { get; set; }
                /// <summary>
                /// 商品标签ID，使用pdd.goods.opt.get接口获取
                /// </summary>
                [JsonProperty("opt_id")]
                public long? OptId { get; set; }
                /// <summary>
                /// 商品标签名称
                /// </summary>
                [JsonProperty("opt_name")]
                public string OptName { get; set; }
                /// <summary>
                /// 商品标签ID
                /// </summary>
                [JsonProperty("opt_ids")]
                public int? OptIds { get; set; }
                /// <summary>
                /// 商品一~四级类目ID列表
                /// </summary>
                [JsonProperty("cat_ids")]
                public int? CatIds { get; set; }
                /// <summary>
                /// 优惠券门槛金额，单位为分
                /// </summary>
                [JsonProperty("coupon_min_order_amount")]
                public long? CouponMinOrderAmount { get; set; }
                /// <summary>
                /// 优惠券面额，单位为分
                /// </summary>
                [JsonProperty("coupon_discount")]
                public long? CouponDiscount { get; set; }
                /// <summary>
                /// 优惠券总数量
                /// </summary>
                [JsonProperty("coupon_total_quantity")]
                public long? CouponTotalQuantity { get; set; }
                /// <summary>
                /// 优惠券剩余数量
                /// </summary>
                [JsonProperty("coupon_remain_quantity")]
                public long? CouponRemainQuantity { get; set; }
                /// <summary>
                /// 优惠券生效时间，UNIX时间戳
                /// </summary>
                [JsonProperty("coupon_start_time")]
                public long? CouponStartTime { get; set; }
                /// <summary>
                /// 优惠券失效时间，UNIX时间戳
                /// </summary>
                [JsonProperty("coupon_end_time")]
                public long? CouponEndTime { get; set; }
                /// <summary>
                /// 佣金比例，千分比
                /// </summary>
                [JsonProperty("promotion_rate")]
                public long? PromotionRate { get; set; }
                /// <summary>
                /// 商品评价数
                /// </summary>
                [JsonProperty("goods_eval_count")]
                public long? GoodsEvalCount { get; set; }
                /// <summary>
                /// 商品类目ID，使用pdd.goods.cats.get接口获取
                /// </summary>
                [JsonProperty("cat_id")]
                public long? CatId { get; set; }
                /// <summary>
                /// 已售卖件数
                /// </summary>
                [JsonProperty("sales_tip")]
                public string SalesTip { get; set; }
                /// <summary>
                /// 商家id
                /// </summary>
                [JsonProperty("mall_id")]
                public long? MallId { get; set; }
                /// <summary>
                /// 服务标签: 4-送货入户并安装,5-送货入户,6-电子发票,9-坏果包赔,11-闪电退款,12-24小时发货,13-48小时发货,17-顺丰包邮,18-只换不修,19-全国联保,20-分期付款,24-极速退款,25-品质保障,26-缺重包退,27-当日发货,28-可定制化,29-预约配送,1000001-正品发票,1000002-送货入户并安装
                /// </summary>
                [JsonProperty("service_tags")]
                public int? ServiceTags { get; set; }
                /// <summary>
                /// 店铺收藏券id
                /// </summary>
                [JsonProperty("clt_cpn_batch_sn")]
                public string CltCpnBatchSn { get; set; }
                /// <summary>
                /// 店铺收藏券起始时间
                /// </summary>
                [JsonProperty("clt_cpn_start_time")]
                public long? CltCpnStartTime { get; set; }
                /// <summary>
                /// 店铺收藏券截止时间
                /// </summary>
                [JsonProperty("clt_cpn_end_time")]
                public long? CltCpnEndTime { get; set; }
                /// <summary>
                /// 店铺收藏券总量
                /// </summary>
                [JsonProperty("clt_cpn_quantity")]
                public long? CltCpnQuantity { get; set; }
                /// <summary>
                /// 店铺收藏券剩余量
                /// </summary>
                [JsonProperty("clt_cpn_remain_quantity")]
                public long? CltCpnRemainQuantity { get; set; }
                /// <summary>
                /// 店铺收藏券面额，单位为分
                /// </summary>
                [JsonProperty("clt_cpn_discount")]
                public long? CltCpnDiscount { get; set; }
                /// <summary>
                /// 店铺收藏券使用门槛价格，单位为分
                /// </summary>
                [JsonProperty("clt_cpn_min_amt")]
                public long? CltCpnMinAmt { get; set; }
                /// <summary>
                /// 描述分
                /// </summary>
                [JsonProperty("desc_txt")]
                public string DescTxt { get; set; }
                /// <summary>
                /// 服务分
                /// </summary>
                [JsonProperty("serv_txt")]
                public string ServTxt { get; set; }
                /// <summary>
                /// 物流分
                /// </summary>
                [JsonProperty("lgst_txt")]
                public string LgstTxt { get; set; }
                /// <summary>
                /// 推广计划类型
                /// </summary>
                [JsonProperty("plan_type")]
                public int? PlanType { get; set; }
                /// <summary>
                /// 招商团长id
                /// </summary>
                [JsonProperty("zs_duo_id")]
                public long? ZsDuoId { get; set; }
                /// <summary>
                /// 快手专享
                /// </summary>
                [JsonProperty("only_scene_auth")]
                public bool? OnlySceneAuth { get; set; }

            }

        }

    }

}
