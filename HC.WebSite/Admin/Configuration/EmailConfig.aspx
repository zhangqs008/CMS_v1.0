<%@ Page Title="邮箱配置" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeFile="EmailConfig.aspx.cs" Inherits="HC.WebSite.Admin.Configuration.EmailConfig" %>

<%@ Import Namespace="HC.Foundation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="p" class="easyui-panel" title='' style="background: #fafafa; height: 500px;">
        <table cellpadding="10" cellspacing="0" width="100%" class="edit_table">
            <tr>
                <td class="td_left">
                    邮件服务器：
                </td>
                <td class="td_right">
                    <input type="text" id="txtHost" name="Host" data-options="required:true,missingMessage:'邮件服务器不能为空'"
                        class="easyui-textbox"></input>
                    <span class="td_tips">邮箱服务器地址，如：<a target="_blank" href="http://help.163.com/09/1223/14/5R7P3QI100753VB8.html">网易163</a>，<a
                        target="_blank" href="http://kf.qq.com/faq/120322fu63YV130422nqIrqu.html">QQ邮箱</a></span>
                </td>
            </tr>
            <tr>
                <td class="td_left">
                    端口：
                </td>
                <td class="td_right">
                    <input type="text" id="txtPort" name="Port" value="25" data-options="required:true,missingMessage:'端口不能为空'"
                        class="easyui-textbox"></input>
                    <span class="td_tips">端口号必须是非负整正数，默认是25端口</span>
                </td>
            </tr>
            <tr>
                <td class="td_left">
                    发件人邮箱：
                </td>
                <td class="td_right">
                    <input type="text" id="txtEmail" name="MailFrom" data-options="required:true,invalidMessage:'邮箱格式不正确',missingMessage:'邮箱不能为空',validType:['email','length[0,20]']"
                        class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="td_left">
                    用户名：
                </td>
                <td class="td_right">
                    <input type="text" id="txtUserName" name="UserName" data-options="required:true,missingMessage:'用户名不能为空'"
                        class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="td_left">
                    密码：
                </td>
                <td class="td_right">
                    <input type="password" id="txtPassword" name="Password" class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="td_left">
                    管理员通知邮箱：
                </td>
                <td class="td_right">
                    <input type="text" id="txtNotifyEmail" name="NotifyEmail" data-options="required:true,invalidMessage:'邮箱格式不正确',missingMessage:'邮箱不能为空',validType:['email','length[0,20]']"
                        class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
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
                    <input type="text" id="_action" name="_action" value="EmailConfigFormHandler" class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="td_left">
                    测试收件箱：
                </td>
                <td class="td_right">
                    <input type="text" id="TestEmail" name="TestEmail" data-options="invalidMessage:'邮箱格式不正确',validType:['email','length[0,20]']"
                        class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    &nbsp;
                </td>
                <td class="panel-header td_right">
                    <div class="button_submit_div">
                        <a href="javascript:void(0)" id="btnSend" class="easyui-linkbutton" data-options="iconCls:'icon-custom-lightning'">
                            发送邮件</a>
                    </div>
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
                Host: "<%= Instance.Host %>",
                MailFrom: "<%= Instance.MailFrom %>",
                NotifyEmail: "<%= Instance.NotifyEmail %>",
                Password: "<%= Instance.Password %>",
                Port: "<%= Instance.Port %>",
                UserName: "<%= Instance.UserName %>"
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
    <script type="text/javascript">
        $(function () {
            $("#btnSend").click(function () {
                var email = $("#TestEmail").val();
                if (email.length > 0) {
                    $.hc.confirm("确定发送测试邮件吗？", function () {
                        $.hc.ajax('AdminPostHandler.SendEmail', {
                            params: { email: email, title: "测试邮件", content: "<span style='color:green'>这是一封测试邮件，您能收到此邮件，说明您的邮箱参数设置正确，请勿回复此邮件。</span>" },
                            success: function (response) {
                                var item = eval(response)[0];
                                if (item.status.toLocaleLowerCase() == "true") {
                                                                            $.hc.confirm("操作成功!<br/><br/>关闭当前窗口？<br/><br/>", function () { top.closeTab(); });
                                } else {
                                    $.hc.alertErrorMsg("操作失败，请与管理员联系！");
                                }
                            }
                        });
                    });
                } else {
                    $("#TestEmail").focus();
                }
            });
        })
    </script>
</asp:Content>
