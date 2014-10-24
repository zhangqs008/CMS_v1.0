<%@ Page Title="站点配置" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeFile="SiteInfo.aspx.cs" Inherits="HC.WebSite.Admin.Configuration.SiteInfo" %>

<%@ Import Namespace="HC.Foundation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--KindEditor编辑器上传插件--%>
    <link rel="stylesheet" href="<%=BasePath%>Scripts/kindeditor-v4.1.7/themes/default/default.css" />
    <link rel="stylesheet" href="<%=BasePath%>Scripts/kindeditor-v4.1.7/plugins/code/prettify.css" />
    <script type="text/javascript" charset="utf-8" src="<%=BasePath%>Scripts/kindeditor-v4.1.7/kindeditor-all.js"></script>
    <script type="text/javascript" charset="utf-8" src="<%=BasePath%>Scripts/kindeditor-v4.1.7/lang/zh-CN.js"></script>
    <script type="text/javascript" charset="utf-8" src="<%=BasePath%>Scripts/kindeditor-v4.1.7/plugins/code/prettify.js"></script>
    <div id="p" class="easyui-panel" title='' style="background: #fafafa; height: 500px;">
        <table cellpadding="10" cellspacing="0" width="100%" class="edit_table">
            <tr>
                <td class="panel-header td_left">
                    站点名称：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtSiteName" name="SiteName" data-options="required:true,missingMessage:'名称不能为空'"
                        class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    Icon：
                </td>
                <td class="panel-header td_right">
                    <script type="text/javascript">
                        KindEditor.ready(function (k) {
                            var editor = k.editor({
                                uploadJson: '<%=BasePath%>FileManagerHandler.aspx',
                                fileManagerJson: '<%=BasePath%>UploadFilesHandler.aspx',
                                allowFileManager: true
                            });
                            k('#insertIcon').click(function () {
                                editor.loadPlugin('image', function () {
                                    editor.plugin.imageDialog({
                                        imageUrl: k('#url').val(),
                                        clickFn: function (url, title, width, height, border, align) {
                                            k('#url').val(url);
                                            $("#imgIcon").attr("src", url);
                                            document.getElementById("txtIcon").value = url;
                                            editor.hideDialog();
                                        }
                                    });
                                });
                            });
                        });
                    </script>
                    <img id="imgIcon" src="<%=SiteConfig.SiteInfo.Icon %>" style="width: 32px; height: 32px;" /><br />
                    <input type="text" name="Icon" id="txtIcon" style="display: none" />
                    <input type="button" id="insertIcon" value="选择图片" />
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    Logo：
                </td>
                <td class="panel-header td_right">
                    <script type="text/javascript">
                        KindEditor.ready(function (k) {
                            var editor = k.editor({
                                uploadJson: '<%=BasePath%>FileManagerHandler.aspx',
                                fileManagerJson: '<%=BasePath%>UploadFilesHandler.aspx',
                                allowFileManager: true
                            });
                            k('#insertfile').click(function () {
                                editor.loadPlugin('image', function () {
                                    editor.plugin.imageDialog({
                                        imageUrl: k('#url').val(),
                                        clickFn: function (url, title, width, height, border, align) {
                                            $("#imgLogo").attr("src", url);
                                            document.getElementById("txtLogo").value = url;
                                            editor.hideDialog();
                                        }
                                    });
                                });
                            });
                        });
                    </script>
                    <img id="imgLogo" src="<%=SiteConfig.SiteInfo.Logo %>" style="width: 180px; height: 60px;" /><br />
                    <input type="text" name="Logo" id="txtLogo" style="display: none" />
                    <input type="button" id="insertfile" value="选择图片" />
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    Cookie有效期：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtCookieTime" name="TicketTime" style="width: 50px" class="easyui-textbox"></input>分钟
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    网站域名：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtMainDomain" name="MainDomain" class="easyui-textbox"></input>(Cookie使用)
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    页脚版权：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtCopyright" name="Copyright" value="<%= Instance.Copyright %>" multiline="true" style="width: 400px;height: 80px" class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_right">
                    &nbsp;
                </td>
                <td class="panel-header td_right">
                    <div class="button_submit_div">
                        <a href="javascript:void(0)" id="btnsave" class="easyui-linkbutton" data-options="iconCls:'icon-save'">
                            保存</a> <a href="javascript:void(0)" id="btnClose" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'"
                                onclick=" top.closeTab() ">关闭</a>
                    </div>
                </td>
            </tr>
            <tr style="display: none">
                <td colspan="2">
                    <input type="text" id="_action" name="_action" value="SiteInfoFormHandler" class="easyui-textbox"></input>
                </td>
            </tr>
        </table>
    </div>
    <%--初始化--%>
    <script type="text/javascript">
        $(function () {
            init();
        });
        function init() {
            $('#aspnetForm').form('load', {
                SiteName: "<%= Instance.SiteName %>",
                TicketTime: "<%= Instance.TicketTime %>",
                MainDomain: "<%= Instance.MainDomain %>",
                Logo: "<%= Instance.Logo %>",
                Icon: "<%= Instance.Icon %>" 
            });
        }
    </script>
    <%--保存按钮--%>
    <script type="text/javascript">
        $(function () {
            $("#btnsave").click(function () {
                $('#aspnetForm').form('submit', {
                    url: "<%= BasePath %>AjaxForm.aspx",
                    onSubmit: function (param) {
                        return $(this).form('enableValidation').form('validate');
                    },
                    success: function (response) {
                        if (response.toLocaleLowerCase() != "true") {
                            $.hc.alertErrorMsg("操作失败：" + response);
                        } else {
                            $.hc.confirm("操作成功!<br/><br/>关闭当前窗口？<br/><br/>", function () { top.closeTab(); });
                        }
                    }
                });
            });
        })
    </script>
</asp:Content>
