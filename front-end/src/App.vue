<template>
<div id="app" :class="bodyClass">
    <transition name="fade" mode="out-in">
        <router-view v-if="loaded"></router-view>
    </transition>
</div>
</template>

<script>
export default {
    name: 'app',
    data() {
        return {
            loaded: false
        }
    },
    computed: {
        bodyClass() {
            return 'bodyClass-' + this.$route.path.substr(1);
        }
    },
    methods: {
        async loadUser() {
            let resp = await this.doGetSync("/user/info");
            if (resp.code == 200) {
                this.loaded                = true;
                this.$store.state.userInfo = resp.data;
                if (this.$route.path == '/login') {
                    this.$router.replace("/main");
                }
            } else {
                this.loaded = false;
            }
            if (this.$route.path == '/login') {
                this.loaded = true;
            }
        }
    },
    mounted() {
        // if (this.$route.path != '/login') {
        this.loadUser();
        // }
    },
}
</script>

<style lang="scss">
.bodyClass-login {
    background-color: #0091e6
}

body {
    margin : 0px;
    padding: 0px;
    /*background: url(assets/bg1.jpg) center !important;

background-size: cover;*/
    // background: #1F2D3D;
    font-family           : Helvetica Neue, Helvetica, PingFang SC, Hiragino Sans GB, Microsoft YaHei, SimSun, sans-serif;
    font-size             : 14px;
    -webkit-font-smoothing: antialiased;
}

#app {
    position: absolute;
    top     : 0px;
    bottom  : 0px;
    width   : 100%;
}

.el-submenu [class^=fa] {
    vertical-align: baseline;
    margin-right  : 10px;
}

.el-menu-item [class^=fa] {
    vertical-align: baseline;
    margin-right  : 10px;
}

.toolbar {
    background: #f2f2f2;
    padding   : 10px;
    //border:1px solid #dfe6ec;
    margin: 10px 0px;

    .el-form-item {
        margin-bottom: 10px;
    }
}

.fade-enter-active,
.fade-leave-active {
    transition: all .2s ease;
}

.fade-enter,
.fade-leave-active {
    opacity: 0;
}

.w50 {
    width: 50px;
}

.w60 {
    width: 60px;
}

.w70 {
    width: 70px;
}

.w80 {
    width: 80px;
}

.w90 {
    width: 90px;
}

.w120 {
    width: 120px;
}

.el-table .warning-row {
    background: oldlace;
}
</style>
