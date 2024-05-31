using System.Collections.Generic;
using Newtonsoft.Json;
namespace PddOpenSDK.Models.Response.Ddk
{
    public partial class SearchDdkGoodsResponseModel : PddResponseModel
    {
        /// <summary>
        /// response
        /// </summary>
        [JsonProperty("goods_search_response")]
        public GoodsSearchResponseResponseModel GoodsSearchResponse { get; set; }
        public partial class GoodsSearchResponseResponseModel : PddResponseModel
        {
            /// <summary>
            /// 商品列表
            /// </summary>
            [JsonProperty("goods_list")]
            public List<GoodsListResponseModel> GoodsList { get; set; }
            /// <summary>
            /// 返回商品总数
            /// </summary>
            [JsonProperty("total_count")]
            public string TotalCount { get; set; }
            /// <summary>
            /// 搜索id
            /// </summary>
            [JsonProperty("search_id")]
            public string SearchId { get; set; }
            public partial class GoodsListResponseModel : PddResponseModel
            {
                /// <summary>
                /// 是否有店铺券
                /// </summary>
                [JsonProperty("has_mall_coupon")]
                public bool? HasMallCoupon { get; set; }
                /// <summary>
                /// 店铺券id
                /// </summary>
                [JsonProperty("mall_coupon_id")]
                public long? MallCouponId { get; set; }
                /// <summary>
                /// 店铺券折扣
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
                /// 店铺券开始使用时间
                /// </summary>
                [JsonProperty("mall_coupon_start_time")]
                public long? MallCouponStartTime { get; set; }
                /// <summary>
                /// 店铺券结束使用时间
                /// </summary>
                [JsonProperty("mall_coupon_end_time")]
                public long? MallCouponEndTime { get; set; }
                /// <summary>
                /// 创建时间（unix时间戳）
                /// </summary>
                [JsonProperty("create_at")]
                public long? CreateAt { get; set; }
                /// <summary>
                /// 商品id
                /// </summary>
                [JsonProperty("goods_id")]
                public long? GoodsId { get; set; }
                /// <summary>
                /// 商品名称
                /// </summary>
                [JsonProperty("goods_name")]
                public string GoodsName { get; set; }
                /// <summary>
                /// 商品描述
                /// </summary>
                [JsonProperty("goods_desc")]
                public string GoodsDesc { get; set; }
                /// <summary>
                /// 商品缩略图
                /// </summary>
                [JsonProperty("goods_thumbnail_url")]
                public string GoodsThumbnailUrl { get; set; }
                /// <summary>
                /// 商品主图
                /// </summary>
                [JsonProperty("goods_image_url")]
                public string GoodsImageUrl { get; set; }
                /// <summary>
                /// 商品轮播图
                /// </summary>
                [JsonProperty("goods_gallery_urls")]
                public List<string> GoodsGalleryUrls { get; set; }
                /// <summary>
                /// 最小拼团价（单位为分）
                /// </summary>
                [JsonProperty("min_group_price")]
                public long? MinGroupPrice { get; set; }
                /// <summary>
                /// 最小单买价格（单位为分）
                /// </summary>
                [JsonProperty("min_normal_price")]
                public long? MinNormalPrice { get; set; }
                /// <summary>
                /// 店铺名字
                /// </summary>
                [JsonProperty("mall_name")]
                public string MallName { get; set; }
                /// <summary>
                /// 店铺类型，1-个人，2-企业，3-旗舰店，4-专卖店，5-专营店，6-普通店
                /// </summary>
                [JsonProperty("merchant_type")]
                public int? MerchantType { get; set; }
                /// <summary>
                /// 商品类目ID，使用pdd.goods.cats.get接口获取
                /// </summary>
                [JsonProperty("category_id")]
                public long? CategoryId { get; set; }
                /// <summary>
                /// 商品类目名
                /// </summary>
                [JsonProperty("category_name")]
                public string CategoryName { get; set; }
                /// <summary>
                /// 商品标签ID，使用pdd.goods.opts.get接口获取
                /// </summary>
                [JsonProperty("opt_id")]
                public long? OptId { get; set; }
                /// <summary>
                /// 商品标签名
                /// </summary>
                [JsonProperty("opt_name")]
                public string OptName { get; set; }
                /// <summary>
                /// 商品标签id
                /// </summary>
                [JsonProperty("opt_ids")]
                public List<long?> OptIds { get; set; }
                /// <summary>
                /// 商品类目id
                /// </summary>
                [JsonProperty("cat_ids")]
                public List<long?> CatIds { get; set; }
                /// <summary>
                /// 该商品所在店铺是否参与全店推广，0：否，1：是
                /// </summary>
                [JsonProperty("mall_cps")]
                public int? MallCps { get; set; }
                /// <summary>
                /// 商品是否有优惠券 true-有，false-没有
                /// </summary>
                [JsonProperty("has_coupon")]
                public bool? HasCoupon { get; set; }
                /// <summary>
                /// 优惠券门槛价格，单位为分
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
                /// 商品评价数量
                /// </summary>
                [JsonProperty("goods_eval_count")]
                public long? GoodsEvalCount { get; set; }
                /// <summary>
                /// 已售卖件数
                /// </summary>
                [JsonProperty("sales_tip")]
                public string SalesTip { get; set; }
                /// <summary>
                /// 活动类型，0-无活动;1-秒杀;3-限量折扣;12-限时折扣;13-大促活动;14-名品折扣;15-品牌清仓;16-食品超市;17-一元幸运团;18-爱逛街;19-时尚穿搭;20-男人帮;21-9块9;22-竞价活动;23-榜单活动;24-幸运半价购;25-定金预售;26-幸运人气购;27-特色主题活动;28-断码清仓;29-一元话费;30-电器城;31-每日好店;32-品牌卡;101-大促搜索池;102-大促品类分会场;
                /// </summary>
                [JsonProperty("activity_type")]
                public int? ActivityType { get; set; }
                /// <summary>
                /// 服务标签: 4-送货入户并安装,5-送货入户,6-电子发票,9-坏果包赔,11-闪电退款,12-24小时发货,13-48小时发货,17-顺丰包邮,18-只换不修,19-全国联保,20-分期付款,24-极速退款,25-品质保障,26-缺重包退,27-当日发货,28-可定制化,29-预约配送,1000001-正品发票,1000002-送货入户并安装
                /// </summary>
                [JsonProperty("service_tags")]
                public List<long?> ServiceTags { get; set; }
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
                /// 推广计划类型 3:定向 4:招商
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
                /// <summary>
                /// 搜索id
                /// </summary>
                [JsonProperty("search_id")]
                public string SearchId { get; set; }
                /// <summary>
                /// 商品活动标记数组，例：[4,7]，4-秒杀 7-百亿补贴等
                /// </summary>
                [JsonProperty("activity_tags")]
                public int? ActivityTags { get; set; }

            }

        }

    }

}
