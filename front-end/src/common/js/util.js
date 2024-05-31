var SIGN_REGEXP = /([yMdhsm])(\1*)/g
var DEFAULT_PATTERN = 'yyyy-MM-dd'
function padding (s, len) {
  var len = len - (s + '').length
  for (var i = 0; i < len; i++) { s = '0' + s; }
  return s
}

export default {
   getUUID:function(len, radix) {
    var chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'.split(''); 
    var uuid = []; 
    radix = radix || chars.length; 
    if (len) {
      for (var i = 0; i < len; i ++ ) {uuid[i] = chars[0 | Math.random() * radix]; }
    }else {
      var r; 
      uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-'; 
      uuid[14] = '4'; 
      for (var i = 0; i < 36; i++ ) {
        if ( ! uuid[i]) {
          r = 0 | Math.random() * 16; 
          uuid[i] = chars[(i == 19)?(r & 0x3) | 0x8:r]; 
        }
      }
    }
    return uuid.join(''); 
},
  getQueryStringByName: function (name) {
    var reg     = new RegExp('(^|&)' + name + '=([^&]*)(&|$)', 'i')
    var r       = window.location.search.substr(1).match(reg)
    var context = ''
    if (r != null)
      context = r[2]
      reg     = null
      r       = null
    return context == null || context == '' || context == 'undefined' ? '': context
  },
  formatDate: {
    format: function (date, pattern) {
      pattern = pattern || DEFAULT_PATTERN
      return pattern.replace(SIGN_REGEXP, function ($0) {
        switch ($0.charAt(0)) {
          case 'y': 
            return padding(date.getFullYear(), $0.length)
          case 'M': 
            return padding(date.getMonth() + 1, $0.length)
          case 'd': 
            return padding(date.getDate(), $0.length)
          case 'w': 
            return date.getDay() + 1
          case 'h': 
            return padding(date.getHours(), $0.length)
          case 'm': 
            return padding(date.getMinutes(), $0.length)
          case 's': 
            return padding(date.getSeconds(), $0.length)
        }
      })
    },
    parse: function (dateString, pattern) {
      var matchs1 = pattern.match(SIGN_REGEXP)
      var matchs2 = dateString.match(/(\d)+/g)
      if (matchs1.length == matchs2.length) {
        var _date = new Date(1970, 0, 1)
        for (var i = 0; i < matchs1.length; i++) {
          var _int = parseInt(matchs2[i])
          var sign = matchs1[i]
          switch (sign.charAt(0)) {
            case 'y': 
              _date.setFullYear(_int)
              break
            case 'M': 
              _date.setMonth(_int - 1)
              break
            case 'd': 
              _date.setDate(_int)
              break
            case 'h': 
              _date.setHours(_int)
              break
            case 'm': 
              _date.setMinutes(_int)
              break
            case 's': 
              _date.setSeconds(_int)
              break
          }
        }
        return _date
      }
      return null
    },
    timeShow: function (millisecond) {
      var days   = Math.floor(millisecond / (24 * 3600 * 1000))
      var leave1 = millisecond % (24 * 3600 * 1000)              // 计算天数后剩余的毫秒数
      var hours  = Math.floor(leave1 / (3600 * 1000))
      // 计算相差分钟数
      var leave2  = leave1 % (3600 * 1000)            // 计算小时数后剩余的毫秒数
      var minutes = Math.floor(leave2 / (60 * 1000))

      // 计算相差秒数
      var leave3  = leave2 % (60 * 1000)       // 计算分钟数后剩余的毫秒数
      var seconds = Math.round(leave3 / 1000)
      var ret     = []
      days > 0 && ret.push(days + '天')
      hours > 0 && ret.push(hours + '小时')
      minutes > 0 && ret.push(minutes + '分钟')
      seconds > 0 && ret.push(seconds + '秒')
      return ret.join('')
    }

  }

}
