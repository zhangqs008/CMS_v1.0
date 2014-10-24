<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="Edit.aspx.cs" Inherits="HC.WebSite.Admin.Content.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--KindEditor编辑器--%>
    <link rel="stylesheet" href="<%=BasePath%>Admin/Scripts/kindeditor-v4.1.7/themes/default/default.css" />
    <link rel="stylesheet" href="<%=BasePath%>Admin/Scripts/kindeditor-v4.1.7/plugins/code/prettify.css" />
    <script type="text/javascript" charset="utf-8" src="<%=BasePath%>Admin/Scripts/kindeditor-v4.1.7/kindeditor-all.js"></script>
    <script type="text/javascript" charset="utf-8" src="<%=BasePath%>Admin/Scripts/kindeditor-v4.1.7/lang/zh-CN.js"></script>
    <script type="text/javascript" charset="utf-8" src="<%=BasePath%>Admin/Scripts/kindeditor-v4.1.7/plugins/code/prettify.js"></script>
    <script src="<%=BasePath%>Admin/Scripts/jquery.validatebox.js" type="text/javascript"></script>
    <div id="p" class="easyui-panel" title='<%= Column.Name %>' style="background: #fafafa;
        height: 500px;">
        <table cellpadding="10" cellspacing="0" width="100%" class="edit_table">
            <tr style="display: none">
                <td colspan="2">
                    <input type="text" id="id" name="Id" data-options="required:true" value='<%=RequestInt32("Id",0) %>'
                        class="easyui-textbox"></input>
                    <input type="text" id="_action" name="_action" value="ContentFormHandler" class="easyui-textbox"></input>
                    <input type="text" id="columnId" name="columnId" value="<%= Column.Id %>" class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    所属栏目：
                </td>
                <td class="panel-header td_right">
                    <%= Column.Name %>
                </td>
            </tr>
            <%=Html.ToString()%>
            <tr>
                <td class="panel-header td_left">
                    &nbsp;
                </td>
                <td class="panel-header td_right">
                    <div class="button_submit_div">
                        <a href="javascript:void(0)" id="btnsave" class="easyui-linkbutton" data-options="iconCls:'icon-save'">
                            保存</a> <a href="javascript:void(0)" id="btnClose" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'"
                                onclick=" parent.closeTab() ">关闭</a>
                    </div>
                </td>
            </tr>
        </table>
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
                                $.hc.confirm("操作成功!<br/><br/>关闭当前窗口？<br/><br/>", function () { parent.closeTab(); });
                            }
                        }
                    });
                });
            })
        </script>
    </div>
</asp:Content>
