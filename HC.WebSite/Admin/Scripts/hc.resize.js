
//窗口缩放时，重绘布局控件尺寸
var settime = null;
function redraw() {
    if (window.customResize) {
        customResize(); //自定义缩放函数，页面若使用多个布局控件，需自定义缩放函数处理
    } else {
        var width = $(window).width();
        var height = $(window).height();
        $('.easyui-panel').panel('resize', { width: width, height: height });
        $('.easyui-layout').layout('resize', { width: width, height: height });
        $('.easyui-treegrid').treegrid('resize', { width: width, height: height });
        $('.easyui-datagrid').datagrid('resize', { width: width, height: height });
    }
}

$(function () {
    redraw();
    $(window).resize(function () {
        if (settime != null)
            clearTimeout(settime);
        settime = setTimeout("redraw()", 100);
    });
}); 