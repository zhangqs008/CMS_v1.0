<%@ Page Title="编辑系统菜单表" EnableEventValidation="false" Language="C#" MasterPageFile="~/Admin/Admin.master"
    AutoEventWireup="true" CodeFile="MenuEdit.aspx.cs" Inherits="HC.WebSite.Admin.Menu.MenuEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="p" class="easyui-panel" title='<%= RequestString("action").ToLower() == "modify" ? "修改" : "添加" %>菜单'
        style="background: #fafafa; height: 500px;">
        <table cellpadding="10" cellspacing="0" width="100%" class="edit_table">
            <tr style="display: none">
                <td colspan="2">
                    <input type="text" id="id" name="Id" data-options="required:true" value="0" class="easyui-textbox"></input>
                    <input type="text" id="_action" name="_action" value="MenuFormHandler" class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    菜单名称：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtName" name="Name" data-options="required:true,missingMessage:'菜单名称不能为空'"
                        class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    上级菜单：
                </td>
                <td class="panel-header td_right">
                    <asp:DropDownList ID="dropParentId" Width="200" name="ParentId" CssClass="easyui-combobox"
                        runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    图标：
                </td>
                <td class="panel-header td_right">
                    <asp:HiddenField ID="txtIco" runat="server"></asp:HiddenField>
                    <img style="vertical-align: middle" id="icon" src="<%= BasePath %>Scripts/jquery-easyui-1.4/themes/icon-custom/<%= Icons %>" />
                    <select id="cc" style="width: 220px">
                    </select>
                    <div id="sp">
                        <div style="background: #fafafa; color: #99BBE8; padding: 5px;">
                            选择图标</div>
                        <div style="padding: 6px">
                            <%= RenderIcos() %>
                        </div>
                    </div>
                    <script type="text/javascript">
                        $(function () {
                            $('#cc').combo({
                                editable: false
                            });
                            $('#sp').appendTo($('#cc').combo('panel'));
                            $('#sp input').click(function () {
                                var v = $(this).val();
                                var s = $(this).next('span').text();
                                $("#icon").attr("src", "<%= BasePath %>Scripts/jquery-easyui-1.4/themes/icon-custom/" + v);
                                $('#cc').combo('setValue', v).combo('setText', s).combo('hidePanel');
                                $("#<%= txtIco.ClientID %>").val(v);
                            });
                        });
                    </script>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    链接地址：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtUrl" name="Url" style="width: 220px;" value="Admin/" class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    菜单描述：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtDescription" style="width: 220px;" name="Description" class="easyui-textbox"></input>
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
                    Name: "<%= Instance.Name %>",
                    Id: "<%= Instance.Id %>",
                    ctl00$ContentPlaceHolder1$dropParentId: "<%= Instance.ParentId %>",
                    ctl00$ContentPlaceHolder1$txtIco: "<%= Instance.Ico %>",
                    Url: "<%= Instance.Url %>",
                    Description: "<%= Instance.Description %>"
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
