import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex); 

export default new Vuex.Store( {
  state: {
    socket: {
      isConnected   : false,
      message       : '',
      reconnectError: false,
      printResult   : new Object(),
    }, 
    $route     : {},
    userInfo   : {},
    exportExcel: {},
    "$print"   : undefined,
  }, 
  mutations: {
    SOCKET_ONOPEN (state, event) {
      Vue.prototype.$socket    = event.currentTarget
      state.socket.isConnected = true
    }, 
    SOCKET_ONCLOSE (state, event) {
      state.socket.isConnected = false
    }, 
    SOCKET_ONERROR (state, event) {
     // console.error(state, event)
    }, 
    // default handler called for all methods
    SOCKET_ONMESSAGE (state, message) {
      state.socket.message = message
    }, 
    // mutations for reconnect methods
    SOCKET_RECONNECT(state, count) {
      //console.info(state, count)
    }, 
    SOCKET_RECONNECT_ERROR(state) {
      state.socket.reconnectError = true;
    }, 
  }, 
  actions: {
    sendMessage:function(context, message) {
      Vue.prototype.$socket.send(message)
    }, 
    transportObj:function(context, obj) {
      this.state.$print = obj;
    },
    notifyPrintResult:function(context,data){
      this.state.socket.printResult = data;
    }
  }
})
