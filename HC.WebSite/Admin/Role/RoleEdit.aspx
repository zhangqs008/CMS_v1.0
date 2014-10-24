<%@ Page Title="编辑角色" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeFile="RoleEdit.aspx.cs" Inherits="HC.WebSite.Admin.Role.RoleEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="p" class="easyui-panel" title='<%= RequestString("action").ToLower() == "modify" ? "修改" : "添加" %>角色'
        style="background: #fafafa; height: 500px;">
        <table cellpadding="10" cellspacing="0" width="100%" class="edit_table">
            <tr>
                <td class="panel-header td_left">
                    角色名称：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtName" name="Name" data-options="required:true,missingMessage:'名称不能为空'"
                        class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    上级角色：
                </td>
                <td class="panel-header td_right">
                    <asp:DropDownList ID="dropParentId" Width="200" CssClass="easyui-combobox" runat="server">
                    </asp:DropDownList>
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
                    <input type="text" id="id" name="Id" data-options="required:true" value="0" class="easyui-textbox"></input>
                    <input type="text" id="_action" name="_action" value="RoleFormHandler" class="easyui-textbox"></input>
                </td>
            </tr>
        </table>
    </div>
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
                <%=dropParentId.ClientID %>: "<%= Instance.ParentId %>"
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
