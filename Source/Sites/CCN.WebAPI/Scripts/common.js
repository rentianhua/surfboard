var urlstr = window.location.href;
urlstr = urlstr.substring(0, urlstr.indexOf('/', 9));

/*获取url中的参数*/
function QueryString() {
    var name, value, i;
    var str = location.href;
    var num = str.indexOf("?")
    str = str.substr(num + 1);
    var arrtmp = str.split("&");
    for (i = 0; i < arrtmp.length; i++) {
        num = arrtmp[i].indexOf("=");
        if (num > 0) {
            name = arrtmp[i].substring(0, num);
            value = arrtmp[i].substr(num + 1);
            this[name] = value;
        }
    }
}

/*
* String.sub().字符串截取(汉字占两位)
*/
String.prototype.sub = function (n) {
    var str = '';
    var r = /[^\x00-\xff]/g;

    if (this == null || this.length <= 0 || this == undefined) {
        return '';
    }
    else if (this.replace(r, "mm").length <= n) {
        return this;
    }
    else {
        var m = Math.floor(n / 2);
        for (var i = m; i < this.length; i++) {
            if (this.substr(0, i).replace(r, "mm").length >= n) {
                return "<abbr title=\"" + this + "\">" + this.substr(0, i) + "…</abbr>";
            }
        }
        return this;
    }
}

//时间格式化
var Dateformat = function (obj, fmt) {
    if (obj != undefined && obj != null && obj != '') {
        if (typeof obj == 'string')
            var dt1 = new Date(Date.parse(obj.replace(/-/g, "/").replace(/T/g," ")));
        else
            var dt1 = obj;
        var o = {
            "M+": dt1.getMonth() + 1, //月份 
            "d+": dt1.getDate(), //日 
            "H+": dt1.getHours(),
            "h+": dt1.getHours(), //小时 
            "m+": dt1.getMinutes(), //分 
            "s+": dt1.getSeconds(), //秒 
            "q+": Math.floor((dt1.getMonth() + 3) / 3), //季度 
            "S": dt1.getMilliseconds() //毫秒 
        };
        if (fmt == undefined || fmt == null || fmt == '')
            fmt = "yyyy-MM-dd";
        if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (dt1.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    } else {
        return '';
    }
}