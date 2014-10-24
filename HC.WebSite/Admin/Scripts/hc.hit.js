$(function () {
    var url = window.location.href;
    if (url.toLocaleLowerCase().indexOf("visitstatistics") <= 0) {
        setTimeout(function() {
            $.hc.ajax('VisitStatisticsPostHandler.Add', {
                 params: { path: url },
                 success: function () { }, 
                 err: function () { }
        });
        }, 10000); //停留10秒才计数
    }
});

