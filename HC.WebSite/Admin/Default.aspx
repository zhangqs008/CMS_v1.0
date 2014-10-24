<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HC.WebSite.Admin._Default" %>

<%@ Import Namespace="HC.Components.AjaxGet" %>
<%@ Import Namespace="HC.Components.Service" %>
<%@ Import Namespace="HC.Foundation" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <link rel="icon" href="<%= "" + GetIconPath() %>" type="image/x-icon" />
    <title>
        <%= SiteConfig.SiteInfo.SiteName %></title>
</head>
<body class="easyui-layout">
    <script type="text/javascript" src="<%= BasePath %>Admin/Menu/MenuJs.aspx"> </script>
    <script type="text/javascript">
        var _basepath = "<%= BasePath %>";
        var _theme = "<%= CustomTheme %>"; 
    </script>
    <link rel="stylesheet" type="text/css" href="<%= BasePath %>Admin/Scripts/jquery-easyui-1.4/themes/<%= CustomTheme %>/easyui.css">
    <link rel="stylesheet" type="text/css" href="<%= BasePath %>Admin/Scripts/jquery-easyui-1.4/themes/icon.css">
    <link rel="stylesheet" type="text/css" href="<%= BasePath %>Admin/Style/common.css" />
    <script type="text/javascript" src="<%= BasePath %>Admin/Scripts/jquery-easyui-1.4/jquery.min.js"> </script>
    <script type="text/javascript" src="<%= BasePath %>Admin/Scripts/jquery-easyui-1.4/jquery.easyui.min.js"> </script>
    <script type="text/javascript" src="<%= BasePath %>Admin/Scripts/jquery-easyui-1.4/jquery.ezsyui.plugin.tab.js"> </script>
    <script type="text/javascript" src="<%= BasePath %>Admin/Scripts/hc.ajax.js"> </script>
    <script src="<%= BasePath %>Admin/Scripts/jquery-easyui-1.4/jquery.easyui.theme.js"
        type="text/javascript"> </script>
    <script src="<%= BasePath %>Admin/Scripts/hc.menu.js" type="text/javascript"> </script>
    <script src="<%= BasePath %>Admin/Scripts/hc.loading.js" type="text/javascript"> </script>
    <form id="form1" runat="server">
    <div data-options="region:'north',split:false" style="height: 30px; color: white;
        border: 0; background: url(<%= BasePath %>Admin/Scripts/jquery-easyui-1.4/themes/<%= CustomTheme %>/images/top.gif) 50% 50% repeat-x rgb(127, 153, 190);">
        <!--Logo-->
        <div class="top_left" style="width: 230px; float: left; font-size: 16px; font-weight: bold;
            text-align: left; padding: 4px; font-family: Helvetica, Tahoma, Arial, STXihei, 华文细黑, Microsoft YaHei, 微软雅黑, SimSun, 宋体, Heiti, 黑体, sans-serif;">
            <a href="<%=BasePath %>Admin/Default.aspx" style="color: white; text-decoration: none">
                <%= SiteConfig.SiteInfo.SiteName %></a>
        </div>
        <!--一级导航-->
        <div class="navmenu" style="width: 800px; position: absolute; left: 245px; top: 2px;">
            <%= MenuService.Instance.GetFirstLevelMenu() %>
        </div>
        <div class="top_userinfo" style="width: 250px; float: right; top: 2px; position: absolute;
            right: 10px; text-align: right; vertical-align: middle">
            您好，<span class="lbluserName"><%= HCContext.Current.Admin.LoginName %></span> <a href="<%=BasePath %>default.aspx"
                target="_blank" id="A1" style="color: white" class="easyui-linkbutton" data-options="iconCls:'icon-custom-home',plain:true">
                网站首页</a> <a href="javascript:void(0)" id="btnLogout" style="color: white" class="easyui-linkbutton"
                    data-options="iconCls:'icon-custom-door_in',plain:true">退出</a>
        </div>
    </div>
    <!--左侧导航-->
    <div id="west" data-options="region:'west',split:true,title:'控制面板'" style="padding: 0px;
        width: 240px;">
        <div id="secondMenuguide" class="easyui-accordion" data-options="fit:true,border:false">
        </div>
    </div>
    <!--右侧部分-->
    <div data-options="region:'center'">
        <div id="tt" class="easyui-tabs" fit="true" border="false" plain="true" style="height: 80%;
            width: 100%;">
            <div title="首页" data-options="tools:'#p-tools',fit:true,plain:true" style="padding: 20px;">
                您好，<%= HCContext.Current.Admin.Identity.Name %>！<a id="btnLogout">退出</a><br />
                <br />
                <img title="主题切换" src="<%= BasePath %>Admin/Scripts/jquery-easyui-1.4/themes/icon-custom/palette.png"
                    style="margin-right: -2px; vertical-align: middle;" />
                <select id="cb-theme" style="width: 100px">
                </select>
                <br />
            </div>
        </div>
    </div>
    <div id="tab_rightmenu" class="easyui-menu" style="width: 150px;">
        <div name="tab_menu-tabcloseall">
            关闭全部标签</div>
        <div name="tab_menu-tabcloseother">
            关闭其他标签</div>
        <div class="menu-sep">
        </div>
        <div name="tab_menu-tabcloseright">
            关闭右侧标签</div>
        <div name="tab_menu-tabcloseleft">
            关闭左侧标签</div>
    </div>
    <div region="south" data-options="split:false" style="height: 30px;">
        <div class="footer" style="color: #444; padding: 3px; text-align: center;">
            <a href="http://www.miitbeian.gov.cn/" style="color: gray; text-decoration: none">
                <%= SiteConfig.SiteInfo.Copyright %></a>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $(function () {
            $("#btnLogout").click(function () {
                $.hc.confirm("确定要退出系统吗？", function () {
                    window.location.href = "<%= BasePath %>Admin/Account/Logout.aspx";
                });
            });
        }); 
    </script>
</body>
</html>
