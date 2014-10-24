$(function () {
    $('.navmenu a').click(function () {
        $('.navmenu a').removeClass('active');
        $(this).addClass('active');

        var menu = _menus[$(this).text().trim()];
        cleanNav();
        addNav(menu);
    });

    // 导航菜单绑定初始化
    $("#secondMenuguide").accordion({
        animate: true
    });
    cleanNav();
    addNav(_menus[$('.navmenu a:first').text().trim()]);
    $('.navmenu a').eq(0).addClass('active');
});

//清理已有导航
function cleanNav() { 
    while ($('#secondMenuguide').accordion('panels').length > 0) {
        $('#secondMenuguide').accordion('remove', 0);
    }
}

//添加二级导航
function addNav(data) {
    $.each(data, function (i, subMenu) {
        var menulist = "";
        $.each(subMenu.menus, function (j, o) {
            menulist += '<a href="javascript:addTab(\'' + o.Name + '\',\'' + _basepath + o.Url + '\')"' + ' rel="' + o.Url + '"' + ' title="' + o.Name + '"' +
                ' class="easyui-linkbutton" data-options="plain:true,iconCls:\'icon-custom-' + o.Ico + '\'" style="width: 90%;margin-left:10px; text-align: left">' + o.Name + '</a>';
        });
        $('#secondMenuguide').accordion('add', {
            title: subMenu.menuname,
            content: menulist,
            iconCls: 'icon-custom-' + subMenu.icon
        });
    });
    $('#secondMenuguide').accordion('select', 0);
}

//新建tab页
function addTab(dom) {
    var title = $(dom).attr("title");
    var url = $(dom).attr("rel");
    addTab(title, url);
}
function addTab(title, url) {
    url = url.indexOf("?") > 0 ? url + "&t=" + new Date().getTime() : url + "?t=" + new Date().getTime(); //解决IE下url不变，页面不刷新问题

    if ($('#tt').tabs('exists', title)) {
        $('#tt').tabs('select', title);
        var allTabs = $('#tt').tabs('tabs');
        $.each(allTabs, function () {
            var ctab = this;
            var opt = ctab.panel('options');
            if (opt.title == title) {
                //刷新Tab页 
                var newContent = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
                $('#tt').tabs('update', {
                    tab: ctab,
                    options: {
                        title: title,
                        content: newContent
                    }
                });
            }
        });
    }
    else {
        var content = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
        $('#tt').tabs('add', {
            title: title,
            content: content,
            height: '400px',
            closable: true
        });
    }
}
//关闭tab页
function closeTab() {
    var tab = $('#tt').tabs('getSelected');
    if (tab) {
        var index = $('#tt').tabs('getTabIndex', tab);
        $('#tt').tabs('close', index);
    }
}

