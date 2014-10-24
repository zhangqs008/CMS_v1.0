//--------------------------------------------------------------------------------
// 文件描述：整站Ajax请求封装
// 文件作者：张清山
// 创建日期：2013-12-10 15:09:32
// 修改记录：
//--------------------------------------------------------------------------------

//调用代码示例：
//$HC.ajax('ArticleTagHandler.AddMember', {
//    params: {
//        username: userName
//    },
//    success: function (response) {
//        var result = eval(response)[0];
//        if (result.status.toLocaleLowerCase()=="true"){
//            alert(result.body);
//        }
//    },
//    err: function (msg) {
//        alert("异常：" + msg);
//    }
//});

//网站根目录
var rootpath = location.protocol + "//" + location.hostname + (location.port == 80 ? "" : ":" + location.port) + "/";
$.hc = $.hc || { version: '1.0' };
$.extend($.hc, {
    htmlEncode: function (text) {
        var value = text;
        try {
            value = value.replace(/&emsp;/g, "&nbsp;");
            value = value.replace(/&/, "&amp;");
            value = value.replace(/</g, "&lt;");
            value = value.replace(/>/g, "&gt;");
            value = value.replace(/'/g, "&apos;");
            value = value.replace(/"/g, "&quot;");
        } catch (e) {
            var span = $('<span>');
            span.html(value);
            value = span.html();
            value = value.replace(/&/, "&amp;");
            value = value.replace(/</g, "&lt;");
            value = value.replace(/>/g, "&gt;");
            value = value.replace(/'/g, "&apos;");
            value = value.replace(/"/g, "&quot;");
        }

        return value;
    },
    ajax: function (cmdName, params) {
        if (typeof (cmdName) != 'string' || cmdName == 'undefind')
            return;
        var defaultParms = {
            url: rootpath + "ajaxpost.aspx",
            data: '',
            type: 'POST',
            params: {}
        };
        var getXml = function (setting) {
            var xml = '';
            for (var arg in setting.params) {
                xml += ('<' + arg + '>' + $.hc.htmlEncode(setting.params[arg]) + '</' + arg + '>');
            }
            return xml;
        };
        //参考：http://www.w3schools.com/html/html_entities.asp
        var settings = $.extend(defaultParms, params);
        var data = '<!DOCTYPE your_root_name[';
        data += '<!ENTITY nbsp "&#160;">';
        data += '<!ENTITY copy "&#169;">';
        data += '<!ENTITY reg "&#174;">';
        data += '<!ENTITY trade "&#8482;">';
        data += '<!ENTITY mdash "&#8212;">';
        data += '<!ENTITY ldquo "&#8220;">';
        data += '<!ENTITY rdquo "&#8221;"> ';
        data += '<!ENTITY pound "&#163;">';
        data += '<!ENTITY yen "&#165;">';
        data += '<!ENTITY euro "&#8364;">';
        data += ']><root><_type>' + cmdName + '</_type>';
        data += getXml(settings);
        data += '</root>';
        settings.data = data;
        $.ajax(settings);

    },
    alert: function (message) {
        top.$.messager.alert("提示信息", message, 'info');
    },
    alertSucessMsg: function (message) {
        top.$.messager.alert("成功提示信息", message, 'info');
    },
    alertErrorMsg: function (message) {
        top.$.messager.alert("错误提示信息", message, 'info');
    },
    confirm: function (message, fnOk, fnCancel) {
        top.$.messager.confirm("提示信息", message, function (ok) {
            if (ok) {
                if (fnOk) {
                    fnOk();
                }
            } else {
                if (fnCancel) {
                    fnCancel();
                }
            }
        });
    },
    fixJsonDate: function (jsonDate, format) {
        var date = null;
        if (jsonDate) {
            var strDate = jsonDate.replace("/Date(", "").replace(")/", "");
            try {
                date = new Date(parseInt(strDate) - (8 * 3600 * 1000));
            } catch (ex) {

            }
        }
        if (!date) {
            return "";
        }
        date = { year: date.getFullYear(), month: date.getMonth() + 1, day: date.getDate(), hour: date.getHours(), minutes: date.getMinutes() };
        switch (format) {
            case '-':
                return date.year + '-' + fixTime(date.month) + '-' + fixTime(date.day);
            case 'zh':
                return date.year + '年' + fixTime(date.month) + '月' + fixTime(date.day) + '日';
            default:
                return date.year + '-' + fixTime(date.month) + '-' + fixTime(date.day) + ' ' + fixTime(date.hour) + ':' + fixTime(date.minutes);
        }
        function fixTime(value) {
            return value.toString().length > 1 ? value : "0" + value;
        }
    },
    clearHTML: function (html) {
        return html.replace(/<[^>]*>/g, "");
    }
});

