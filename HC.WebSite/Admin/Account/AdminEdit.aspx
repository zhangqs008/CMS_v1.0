<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AdminEdit.aspx.cs" Inherits="HC.WebSite.Admin.Account.AdminEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="p" class="easyui-panel" title='<%=RequestString("action").ToLower()=="modify"?"修改":"添加" %>管理员'
        style="height: 500px; background: #fafafa;">
        <table cellpadding="10" cellspacing="0" class="edit_table" style="width: 100%">
            <tr>
                <td class="panel-header td_left">
                    用户名：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtName" name="LoginName" data-options="required:true,missingMessage:'用户名不能为空'"
                        class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    真实姓名：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtTrueName" name="TrueName" class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    密 码：
                </td>
                <td class="panel-header td_right">
                    <input type="password" id="txtPassword" name="Password" class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    性别：
                </td>
                <td class="panel-header td_right">
                    <input type="radio" id="radioSex1" value="1" checked="checked" name="Sex">男</input>
                    <input type="radio" id="radioSex2" value="0" name="Sex">女</input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    邮 箱：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtEmail" name="Email" data-options="required:true,invalidMessage:'邮箱格式不正确',missingMessage:'邮箱不能为空',validType:['email','length[0,20]']"
                        class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    手机号码：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtPhone" name="Phone" class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    出生日期：
                </td>
                <td class="panel-header td_right">
                    <div id="cc" class="easyui-datebox" name="Birthday">
                    </div>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    是否启用：
                </td>
                <td class="panel-header td_right">
                    <input type="radio" id="radio1" value="0" checked="checked" name="Status">启用</input>
                    <input type="radio" id="radio2" value="-1" name="Status">禁用</input>
                </td>
            </tr>
            <tr class="tr_btns">
                <td class="panel-header td_right">
                </td>
                <td class="panel-header td_right">
                    <a href="javascript:void(0)" id="btnsave" class="easyui-linkbutton" data-options="iconCls:'icon-save'">
                        保存</a> <a href="javascript:void(0)" id="btnClose" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'"
                            onclick=" top.closeTab() ">关闭</a>
                </td>
            </tr>
            <tr style="display: none">
                <td colspan="2">
                    <input type="text" id="id" name="Id" data-options="required:true" value="0" class="easyui-textbox"></input>
                    <input type="text" id="_action" name="_action" value="AdminFormHandler" class="easyui-textbox"></input>
                </td>
            </tr>
        </table>
        <%--初始化--%>
        <script type="text/javascript">
            var action = '<%= RequestString("action").ToLower() %>';
            $(function () {
                if (action == "modify") {
                    init();
                }
            });

            function init() {
                $('#aspnetForm').form('load', {
                    Id: "<%= Instance.Id %>",
                    LoginName: "<%= Instance.LoginName %>",
                    TrueName: "<%= Instance.TrueName %>",
                    Password: "<%= Instance.Password %>",
                    Email: "<%= Instance.Email %>",
                    Phone: "<%= Instance.Phone %>",
                    Sex: '<%= Instance.Sex?"1":"0" %>',
                    Birthday: "<%= Instance.Birthday%>",
                    Status: "<%= Instance.Status %>"
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
    </div>
</asp:Content>
