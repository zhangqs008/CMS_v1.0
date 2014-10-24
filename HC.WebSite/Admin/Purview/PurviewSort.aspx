<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="PurviewSort.aspx.cs" Inherits="HC.WebSite.Admin.Purview.PurviewSort" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="<%=BasePath %>Admin/Scripts/jquery-ui-v1.10.2/jquery-ui.css" rel="stylesheet"
        type="text/css" />
    <script src="<%=BasePath %>Admin/Scripts/jquery-ui-v1.10.2/jquery-ui.js" type="text/javascript"></script>
    <style type="text/css">
        .sortItem { list-style: none; line-height: 22px; border: 1px solid #ddd; padding: 3px 10px; margin-bottom: 5px; background: #F2F2F2; width: 50%; cursor: pointer; }
    </style>
    <div id="tt">
        <a href="javascript:void(0)" class="icon-save" onclick="javascript:save(<%=ParentId %>)">
        </a>
    </div>
    <div id="p" class="easyui-panel" data-options="tools:'#tt'" title="权限排序" style="padding: 10px;">
        <p style="color: green">
            温馨提示：鼠标拖动以进行排序，点击右侧按钮以进行保存</p>
        <div class="categoryItems">
            <ul id="sortable" class="ui-sortable">
                <%=Html %>
            </ul>
        </div>
        <script type="text/javascript">
            $(function () {
                //拖动排序
                $("#sortable").sortable({
                    revert: true
                });
            });
            function save(parentId) {
                $.hc.confirm("您确定要执行操作吗？", function () {
                    if (parseInt(parentId) > -1) {
                        var sorts = "";
                        var items = $(".sortItem");
                        if (items.length > 0) {
                            for (var i = 0; i < items.length; i++) {
                                sorts += $(items[i]).attr("cid") + "-" + (i + 1) + "|";
                            }
                            $.hc.ajax('PurviewPostHandler.Sort', {
                                params: { pid: parentId, sorts: sorts },
                                success: function (response) {
                                    var item = eval(response)[0];
                                    if (item.status.toLocaleLowerCase() == "true") {
                                        $.hc.confirm("操作成功!<br/><br/>关闭当前窗口？<br/><br/>", function () { top.closeTab(); });
                                    } else {
                                        $.hc.alertErrorMsg("操作失败，请与管理员联系！");
                                    }
                                }
                            });

                        } else {
                            $.hc.alertErrorMsg("没有任何栏目可排序！");
                        }
                    } else {
                        $.hc.alertErrorMsg("请选择您要排序的栏目！");
                    }
                });
            }
        </script>
    </div>
</asp:Content>
