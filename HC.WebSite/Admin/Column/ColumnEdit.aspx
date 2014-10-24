<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ColumnEdit.aspx.cs" Inherits="HC.WebSite.Admin.Column.ColumnEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="p" class="easyui-panel" title='<%= RequestString("action").ToLower() == "modify" ? "修改" : "添加" %>系统栏目'
        style="background: #fafafa; height: 500px;">
        <table cellpadding="10" cellspacing="0" width="100%" class="edit_table">
            <tr style="display: none">
                <td colspan="2">
                    <input type="text" id="id" name="Id" data-options="required:true" value="0" class="easyui-textbox"></input>
                    <input type="text" id="_action" name="_action" value="ColumnFormHandler" class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    栏目名称：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtName" name="Name" data-options="required:true,width:200,missingMessage:' 栏目名称不能为空'"
                        class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    上级栏目：
                </td>
                <td class="panel-header td_right">
                    <select id="ParentId" name="ParentId" class="easyui-combobox" style="width: 200px">
                        <%=ParentColumnHtml%>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    前台显示：
                </td>
                <td class="panel-header td_right">
                    <input type="radio" value="1" checked="checked" name="IsShowFront" id="radioIsShowFrontYes" />
                    <label for="radioIsShowFrontYes">
                        是</label>
                    <input type="radio" value="0" name="IsShowFront" id="radio1" />
                    <label for="radioIsShowFrontYes">
                        否</label>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    绑定模型：
                </td>
                <td class="panel-header td_right">
                    <select name="ModuleId" class="easyui-combobox" style="width: 200px">
                        <%=ModuleIdHtml%>
                    </select>
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
                    Id: "<%= Instance.Id %>",
                    Name: "<%= Instance.Name %>",
                    ParentId: "<%= Instance.ParentId %>",
                    Level: "<%= Instance.Level %>",
                    IsShowFront: '<%= Instance.IsShowFront?"1":"0" %>',
                    ModuleId: "<%= Instance.ModuleId %>"

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
