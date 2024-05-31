<template>
<div>
    <div class="el-alert el-alert--error is-light" v-show="this.showBar">
        <i class="el-alert__icon el-icon-error"></i>
        <div class="el-alert__content">
            <span class="el-alert__title">
                未绑定菜鸟账号，请先<a :href="oauthUrl" target="_blank">绑定</a>。
            </span>
        </div>
    </div>
    <div class="el-alert el-alert--success is-light" v-if="this.branchAccount&&this.branchAccount.quantity">
        <i class="el-alert__icon el-icon-info"></i>
        <div class="el-alert__content">
            <span class="el-alert__title">
                面单剩余数量: {{this.branchAccount.quantity}}
                <el-button icon="el-icon-refresh" size="mini" v-on:click="loadAccountQuantity(true)" circle></el-button>
            </span>
        </div>
    </div>
</div>
</template>

<style scoped>
.el-alert {
    margin: 5px;
}
</style>

<script>
export default {
    data() {
        return {
            showBar      : false,
            branchAccount: undefined,
            oauthUrl     : ''
        }
    },
    methods: {
        refreshStatus() {
            let $self = this;
            this.doGet("/cainiao/caiNiaowaybillstatus", {}, {
                success(res) {
                    $self.showBar = !res.data.caiNiaoWaybill;
                }
            });
        },
        loadAccountQuantity(tipRes) {
            let $self = this;
            if ($self.$route.path != 'login') {
                this.doGet("/cainiao/defaultquerywaybill", {}, {
                    success(res) {
                        if (res.code == 200) {
                            $self.branchAccount = res.data;
                            if (tipRes) {
                                $self.msgSuccess("面单余量已刷新")
                            }
                            return false;
                        }
                    },
                    fail() {
                        return true;
                    }
                });
            }
        }
    },
    mounted() {
        this.oauthUrl = `/api/cainiao/authorize?url=/api/cainiao/code&ext=/bind`
        this.refreshStatus();
        this.loadAccountQuantity();
        setInterval(this.loadAccountQuantity, 30000);
    }
}
</script>
