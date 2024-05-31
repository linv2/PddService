<template>
<el-form label-width="120px" style="margin:20px;width:60%;min-width:600px;">
    <el-form-item label="名称：">
        {{form.displayName||form.userName}}
        <el-button size="mini" class="button" @click="showEdit=true">修改显示名</el-button>
    </el-form-item>
    <el-form-item v-if="showEdit">
        <el-input v-model="displayName" maxlength="12" show-word-limit style="width:300px" placeholder="请输入您的名称"></el-input>
        <el-button type="primary" size="mini" class="button" :disabled="loading" @click="doSaveDisplayName">{{loading?'保存中.':'保存'}}</el-button>
        <el-button size="mini" @click="showEdit=false">取消</el-button>
    </el-form-item>
    <el-form-item label="账号：">
        {{form.userName}}
    </el-form-item>
    <el-form-item label="密码：">
        ********
        <el-button size="mini" class="button" @click="doUpdatePassword">修改密码</el-button>
    </el-form-item>
    <el-form-item label="最后登录时间：">
        {{form.lastLoginTime}}
    </el-form-item>
    <el-form-item label="最后登录地点：">
        {{form.lastLoginIP}}
    </el-form-item>
    <el-form-item label="总登录次数：">
        {{form.loginCount}}
    </el-form-item>
    <el-form-item label="菜鸟账号：">
        {{caiNiaoAccountStatus?'已绑定':'未绑定'}}
        <a :href="oauthUrl" target="_blank" class="el-button el-button--mini button">重新/更换绑定</a>
    </el-form-item>
    <el-form-item label="账号状态：">
        {{form.disable?'禁用':'正常'}}
    </el-form-item>

</el-form>
</template>

<style scoped>
.button {
    margin-left: 20px;
}
</style>

<script>
export default {
    data() {
        return {
            form: {

            },
            loading             : false,
            displayName         : '',
            showEdit            : false,
            caiNiaoAccountStatus: false,
            oauthUrl            : ''
        }
    },
    methods: {
        async doSaveDisplayName() {
            let $self         = this;
                $self.loading = true;
            $self.doPost("/user/updatedisplayname", {
                displayName: $self.displayName
            }, {
                success() {

                    $self.$store.state.userInfo.displayName = $self.displayName;
                    $self.form.displayName                  = $self.displayName;
                    $self.showEdit                          = false;
                    return true;
                },
                any() {
                    $self.loading = false;
                }
            })
        },
        doReBind() {

        },
        doUpdatePassword() {
            this.$router.push("/user/passowrd");
        },
        async loadInfo() {
            let userData = this.$store.state.userInfo;
            for (var key in userData) {
                this.$set(this.form, key, userData[key]);
            }
            this.displayName = this.form.displayName;
        },
        caiNiaoAccount() {
            let $self = this;
            this.doGet("/cainiao/caiNiaowaybillstatus", {}, {
                success(res) {
                    $self.caiNiaoAccountStatus = res.data.caiNiaoWaybill;
                }
            });
        },
    },
    mounted() {
        
        this.oauthUrl = `/api/cainiao/authorize?url=/api/cainiao/code&ext=/bind`
        this.loadInfo();
        this.caiNiaoAccount();
    }
}
</script>
