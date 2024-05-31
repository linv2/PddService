import babelpolyfill from 'babel-polyfill'
import Vue from 'vue'
import App from './App'
import ElementUI from 'element-ui'
// import './assets/theme/theme-green/index.css'
import VueRouter from 'vue-router'
import store from './vuex/store'
import Vuex from 'vuex'
// import NProgress from 'nprogress'
// import 'nprogress/nprogress.css'
import routes from './routes.vue'
import axios from 'axios'
import _mixin from './common/mixin.vue'
Vue.use(ElementUI)
Vue.use(VueRouter)
Vue.use(Vuex)
Vue.mixin(_mixin)
import pluginInstall from './plugin/install'
Vue.use(pluginInstall)

import VueNativeSock from 'vue-native-websocket'
Vue.use(VueNativeSock, 'ws://localhost:13528', {reconnection: true,store: store})
axios.interceptors.request.use(request => {
  request.withCredentials = true;
  return request
})
axios.interceptors.response.use(response => {
  if (response.data && response.data.code === 401&&response.config.url!=="/user/info") {
     router.replace('/login')
  }
  return response
})

const router = new VueRouter({
  //mode: 'history',
routes})

router.beforeEach((to, from, next) => {
  next()
})

// router.afterEach(transition => {
// NProgress.done()
// })

new Vue({
  // el: '#app',
  // template: '<App/>',
  router,
  store,
  // components: { App }
  render: h => h(App)
}).$mount('#app')
