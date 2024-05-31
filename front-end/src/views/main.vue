<template>
<div>
    <el-row :gutter="20" class="row">
        <el-col :span="3">
            <el-card class="box-card">
                <div slot="header" class="clearfix">
                    <span>有效店铺数</span>
                </div>
                <div class="content">
                    {{mall.valid}}
                </div>
            </el-card>
        </el-col>
        <el-col :span="3">
            <el-card class="box-card">
                <div slot="header" class="clearfix">
                    <span>无效店铺数</span>
                </div>
                <div class="content">
                    {{mall.invalid}}
                </div>
            </el-card>
        </el-col>
        <el-col :span="3">
            <el-card class="box-card">
                <div slot="header" class="clearfix">
                    <span>当日订单总数</span>
                </div>
                <div class="content">

                    {{order.all}}
                </div>
            </el-card>
        </el-col>
        <el-col :span="3">
            <el-card class="box-card">
                <div slot="header" class="clearfix">
                    <span>当日订单未发货数</span>
                </div>
                <div class="content">

                    {{order.noWaybill}}
                </div>
            </el-card>
        </el-col>

        <el-col :span="3">
            <el-card class="box-card">
                <div slot="header" class="clearfix">
                    <span>当日订单已发货数</span>
                </div>
                <div class="content">

                    {{order.waybill}}
                </div>
            </el-card>
        </el-col>
        <el-col :span="3">
            <el-card class="box-card">
                <div slot="header" class="clearfix">
                    <span>当日订单未打印数</span>
                </div>
                <div class="content">

                    {{order.noPrint}}
                </div>
            </el-card>
        </el-col>
        <el-col :span="3">
            <el-card class="box-card">
                <div slot="header" class="clearfix">当日订单已打印数
                </div>
                <div class="content">
                    {{order.printed}}
                </div>
            </el-card>
        </el-col>
    </el-row>
    <el-row class="row">
        <el-card class="box-card" :span="20">
            <div slot="header" class="clearfix">月份订单统计
            </div>
            <div ref="order" class="chart">
            </div>
        </el-card>
    </el-row>
</div>
</template>

<style scoped>
div.content {
    width     : 100%;
    text-align: center;
    height    : 50px;
    font-size : 30px;
}

.row {
    margin-top: 20px;
}

.chart {
    height: 200px;
}
</style>

<script>
export default {
    data() {
        return {
            mall: {
                invalid: '-',
                valid  : '-'
            },
            order: {
                all      : '-',
                noWaybill: '-',
                waybill  : '-',
                noPrint  : '-',
                printed  : '-'
            }
        };
    },
    methods: {
        async loadMall() {
            let resp            = await this.doGetSync("/report/getmallcount");
            let linq            = this.$linq.from(resp.data);
                this.mall.valid = linq.firstOrDefault(x => x.type, {
                count: 0
            }).count;
            this.mall.invalid = linq.firstOrDefault(x => !x.type, {
                count: 0
            }).count;
        },
        async loadOrder(key, waybillStatus, printStatus) {
            let resp = await this.doGetSync("/report/getordercount", {
                waybillStatus,
                printStatus
            });
            this.$set(this.order, key, resp.data);
        },
        async renderOrder() {
            let $self = this;
            $self.doGet("/report/getmonthordercount", {
                days: 60
            }, {
                success(resp) {
                    if (!($self.$refs.order) || $self.$refs.order.$el) {
                        return;
                    }
                    let linq       = $self.$linq.from(resp.data);
                    let axisData   = linq.select(x => x.key).toArray();
                    let seriesData = linq.select(x => x.value).toArray();
                    var myChart    = $self.$echarts.init($self.$refs.order || $self.$refs.order.$el);
                    // 绘制图表
                    myChart.setOption({
                        xAxis: {
                            data: axisData
                        },
                        yAxis : {},
                        series: [{
                            name: '数量',
                            type: 'line',
                            data: seriesData
                        }]
                    });
                }
            });

        }
    },
    async mounted() {
        this.loadMall();
        this.loadOrder('all', -1, -1);
        this.loadOrder('noWaybill', 0, -1);
        this.loadOrder('waybill', 1, -1);
        this.loadOrder('noPrint', -1, 0);
        this.loadOrder('printed', -1, 1);
        this.renderOrder();
    }
}
</script>
