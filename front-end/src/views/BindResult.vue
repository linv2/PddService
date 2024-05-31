<template>
<el-row class="oauth_container">
    <div v-if="ownerName">
        店铺《{{ownerName}}》已完成授权，请刷新页面！

    </div>
    <div class="error" v-if="errorMsg">
        <div>授权过程出现异常：</div>
        <div>{{errorMsg}}</div>
        <div>请联系管理人员！！！</div>
    </div>
    <div v-if="bind">
        菜鸟账号已成功绑定!
    </div>
    <el-row class="footer">
        <el-button type="danger" v-on:click="onClose">关闭页面</el-button>
    </el-row>
</el-row>
</template>

<style>
.oauth_container {
    /*box-shadow: 0 0px 8px 0 rgba(0, 0, 0, 0.06), 0 1px 0px 0 rgba(0, 0, 0, 0.02);*/
    -webkit-border-radius: 5px;
    border-radius        : 5px;
    -moz-border-radius   : 5px;
    background-clip      : padding-box;
    margin               : 180px auto;
    width                : 50%;
    height               : 50%;
    padding              : 35px 35px 15px 35px;
    background           : #fff;
    border               : 1px solid #eaeaea;
    box-shadow           : 0 0 25px #cac6c6;
    font-size            : 25px;

}

.error div {
    line-height: 45px;
    color      : red;
}

.footer {
    display: flex;
    margin : 30%;
}

.footer button {
    flex: 1;
}
</style>

<script>
import util from '../common/js/util'
export default {
    data() {
        return {
            errorMsg   : '',
            ownerName  : '',
            bindCaiNiao: ''
        }
    },
    methods: {
        onClose() {
            window.open("about:blank", "_self").close()
        }
    },
    mounted() {
       
        this.errorMsg    = util.getQueryStringByName("error");
        this.ownerName   = decodeURIComponent(util.getQueryStringByName("ownerName"));
        this.bindCaiNiao = util.getQueryStringByName("bind");
    },
}
</script>
