
let pluginInstaller = {};

import linq from '../common/js/linq';
let echarts                 = require('echarts');
    pluginInstaller.install = function(Vue) {
    Vue.prototype.$linq    = linq;
    Vue.prototype.$echarts = echarts;
}
export default pluginInstaller; 