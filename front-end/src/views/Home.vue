<template>
<el-row class="container">
    <el-col :span="24" class="header">
        <el-col :span="2" class="logo">{{sysName}}</el-col>
        <el-col :span="18">
            <el-menu class="nav" :default-active="$route.path" mode="horizontal" unique-opened router>
                <template v-for="item in $router.options.routes">
                    <el-menu-item v-if="item.leaf&&item.children.length>0" :key="JSON.stringify(item)" :index="item.children[0].path">
                        <i :class="item.iconCls"></i>
                        {{item.children[0].name}}
                    </el-menu-item>
                </template>
            </el-menu>
        </el-col>
        <el-col :span="4" class="userinfo">
            <el-dropdown trigger="hover">
                <span class="el-dropdown-link userinfo-inner">
                    {{this.$store.state.userInfo.displayName}}
                    <img :src="require('../assets/anonymous.jpg')" />
                </span>
                <el-dropdown-menu slot="dropdown">
                    <el-dropdown-item @click.native="logout">退出登录</el-dropdown-item>
                </el-dropdown-menu>
            </el-dropdown>
        </el-col>
    </el-col>
    <el-row class="main  el-col-24">
        <el-col :span="24">
            <printComponent ref="print"></printComponent>
            <cainiaoAccoutBar></cainiaoAccoutBar>
            <exportExcel ref="exportExcel"></exportExcel>
            <section class="content-container">
                <div class="grid-content bg-purple-light">
                    <el-col :span="24" class="breadcrumb-container">
                        <strong class="title">{{$route.name}}</strong>
                        <el-breadcrumb separator="/" class="breadcrumb-inner">
                            <el-breadcrumb-item v-for="item in $route.matched" :key="item.path">{{ item.name }}</el-breadcrumb-item>
                        </el-breadcrumb>
                    </el-col>
                    <el-col :span="24" class="content-wrapper">
                        <transition name="fade" mode="out-in">
                            <router-view></router-view>
                        </transition>
                    </el-col>
                </div>
            </section>
        </el-col>
    </el-row>
</el-row>
</template>

<style>
.nav {
    background-color: #20a0ff;
    width: 100%;
}

.nav .el-menu-item {
    color: #fff;
  width: 10%;
  }

.el-menu--horizontal.el-menu--dark .el-submenu .el-menu-item.is-active,
.el-menu-item.is-active {
    background-color: #fff;
    color           : #20a0ff;
}

.nav .el-menu-item:hover {
    background-color: #fff;
}
</style>

<script>
import printComponent from '../components/printComponent';
import cainiaoAccoutBar from '../components/cainiaoAccoutBar';
import exportExcel from '../components/exportExcel';

export default {
    components: {
        printComponent,
        cainiaoAccoutBar,
        exportExcel: exportExcel
    },
    data() {
        return {
            sysName: "打单系统",
        };
    },
    methods: {
        logout() {
            window.location = '/api/user/logout?url=/login'
        }
    },
    mounted() {
      this.$store.state.exportExcel = this.$refs.exportExcel;
    }
};
</script>

<style lang="scss" scoped>

.container {
  position: absolute;
  top     : 0px;
  bottom  : 0px;
  width   : 100%;

  .header {
    height     : 60px;
    line-height: 60px;
    background : #20a0ff;
    color      : #fff;

    .userinfo {
      text-align   : right;
      padding-right: 35px;
      float        : right;

      .userinfo-inner {
        cursor: pointer;
        color : #fff;

        img {
          width        : 40px;
          height       : 40px;
          border-radius: 20px;
          margin       : 10px 0px 10px 10px;
          float        : right;
        }
      }
    }

    .logo {
      //width:230px;
      height            : 60px;
      font-size         : 18px;
      padding-left      : 20px;
      padding-right     : 20px;
      border-color      : rgba(238, 241, 146, 0.3);
      border-right-width: 1px;
      border-right-style: solid;

      img {
        width : 40px;
        float : left;
        margin: 10px 10px 10px 18px;
      }

      .txt {
        color: #fff;
      }
    }

    .logo-width {
      width: 230px;
    }

    .logo-collapse-width {
      width: 60px;
    }

    .tools {
      padding    : 0px 23px;
      width      : 14px;
      height     : 60px;
      line-height: 60px;
      cursor     : pointer;
    }
  }

  .main {
    display: flex;
    // background: #324057;
    position: absolute;
    top     : 60px;
    bottom  : 0px;
    overflow: hidden;

    aside {
      flex : 0 0 230px;
      width: 230px;

      // position: absolute;
      // top: 0px;
      // bottom: 0px;
      .el-menu {
        height: 100%;
      }

      .collapsed {
        width: 60px;

        .item {
          position: relative;
        }

        .submenu {
          position: absolute;
          top     : 0px;
          left    : 60px;
          z-index : 99999;
          height  : auto;
          display : none;
        }
      }
    }

    .menu-collapsed {
      flex : 0 0 60px;
      width: 60px;
    }

    .menu-expanded {
      flex : 0 0 230px;
      width: 230px;
    }

    .content-container {
      // background: #f1f2f7;
      flex: 1;
      // position: absolute;
      // right: 0px;
      // top: 0px;
      // bottom: 0px;
      // left: 230px;
      overflow-y: scroll;
      padding   : 20px;
      height    : 85%;

      .breadcrumb-container {
        //margin-bottom: 15px;
        .title {
          width: 200px;
          float: left;
          color: #475669;
        }

        .breadcrumb-inner {
          float: right;
        }
      }

      .content-wrapper {
        background-color: #fff;
        box-sizing      : border-box;
      }
    }
  }
}
</style>
