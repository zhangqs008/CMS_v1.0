$(function () {
    var themes = [{
        value: 'default',
        text: 'Default',
        group: 'Base'
    },
    {
        value: 'gray',
        text: 'Gray',
        group: 'Base'
    },
    {
        value: 'metro',
        text: 'Metro',
        group: 'Base'
    },
    {
        value: 'bootstrap',
        text: 'Bootstrap',
        group: 'Base'
    },
    {
        value: 'black',
        text: 'Black',
        group: 'Base'
    },
    {
        value: 'metro-blue',
        text: 'Metro Blue',
        group: 'Metro'
    },
    {
        value: 'metro-gray',
        text: 'Metro Gray',
        group: 'Metro'
    },
    {
        value: 'metro-green',
        text: 'Metro Green',
        group: 'Metro'
    },
    {
        value: 'metro-orange',
        text: 'Metro Orange',
        group: 'Metro'
    },
    {
        value: 'metro-red',
        text: 'Metro Red',
        group: 'Metro'
    },
    {
        value: 'ui-cupertino',
        text: 'Cupertino',
        group: 'UI'
    },
    {
        value: 'ui-dark-hive',
        text: 'Dark Hive',
        group: 'UI'
    },
    {
        value: 'ui-pepper-grinder',
        text: 'Pepper Grinder',
        group: 'UI'
    },
    {
        value: 'ui-sunny',
        text: 'Sunny',
        group: 'UI'
    }];

    //<select id="cb-theme" style="width:120px"></select>
    $('#cb-theme').combobox({
        groupField: 'group',
        data: themes,
        editable: false,
        panelHeight: 'auto',
        onChange: onChangeTheme,
        onLoadSuccess: function () {
            $(this).combobox('setValue', _theme);
        }
    }); 
});
function onChangeTheme(theme) {
    if (theme != _theme) {
        $.hc.confirm("确定要使用该主题吗？", function () {
            $.hc.ajax('AdminPostHandler.SetAdminTheme', {
                params: { theme: theme },
                success: function (response) {
                    var item = eval(response)[0];
                    if (item.status.toLocaleLowerCase() == "true") {
                        window.location.href = window.location.href;
                    } else {
                        $.hc.alertErrorMsg("操作失败，请与管理员联系！");
                    }
                }
            });
        });
    }
} 