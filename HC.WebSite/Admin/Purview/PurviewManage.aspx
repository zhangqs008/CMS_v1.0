<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="PurviewManage.aspx.cs" Inherits="HC.WebSite.Admin.Purview.PurviewManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var selectedRow = undefined;
        function onClickRow(row) {
            selectedRow = row;
        }

        var toolbar = [{
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                var pid = 0;
                if (selectedRow) {
                    pid = selectedRow.Id;
                    top.addTab("编辑权限", "<%=BasePath%>Admin/Purview/PurviewEdit.aspx?parentId=" + pid);
                } else {
                    top.addTab("编辑权限", "<%=BasePath%>Admin/Purview/PurviewEdit.aspx");
                }
            }
        }, {
            text: '编辑',
            iconCls: 'icon-edit',
            handler: function () {
                if (!selectedRow) {
                    $.hc.alertErrorMsg('请选择要操作的数据行!');
                    return;
                }
                top.addTab("编辑权限", "<%=BasePath%>Admin/Purview/PurviewEdit.aspx?action=modify&Id=" + selectedRow.Id);
            }
        }, "-",
            {
                text: '排序',
                iconCls: 'icon-custom-sortalphabet',
                handler: function () {
                    if (!selectedRow) {
                        $.hc.alertErrorMsg('请选择要操作的数据行!');
                        return;
                    }
                    if (selectedRow.children && selectedRow.children.length > 0) {
                        top.addTab("权限排序", "<%=BasePath%>Admin/Purview/PurviewSort.aspx?action=sort&Id=" + selectedRow.Id);
                    } else {
                        $.hc.alertErrorMsg('对不起，该权限下无子权限，不能进行排序');
                        return;
                    }
                }
            }, "-", {
                text: '删除',
                iconCls: 'icon-remove',
                handler: function () {
                    if (!selectedRow) {
                        $.hc.alertErrorMsg('请选择要操作的数据行!');
                        return;
                    }
                    $.hc.confirm("该权限的所有子权限也将被删除\n确定要删除该权限吗？", function () {
                        $.hc.ajax('PurviewPostHandler.Delete', {
                            params: { id: selectedRow.Id },
                            success: function (itemResponse) {
                                var item = eval(itemResponse)[0];
                                if (item.status.toLocaleLowerCase() == "true") {
                                    $('#table').treegrid('reload');
                                    selectedRow = undefined;
                                    $.hc.confirm("操作成功!<br/><br/>关闭当前窗口？<br/><br/>", function () { top.closeTab(); });
                                } else {
                                    $.hc.alertErrorMsg("操作失败，请与管理员联系！");
                                }
                            }
                        });

                    });
                }
            },
            {
                text: '刷新',
                iconCls: 'icon-reload',
                handler: function () {
                    $('#table').treegrid('reload');
                    selectedRow = undefined;
                }
            }];
    </script>
    <div class="easyui-layout">
        <table id="table" title="" style="height: 580px" class="easyui-treegrid" data-options="
				url: '<%=BasePath%>ajaxget.aspx?_t='+new Date().getTime()+'&_action=PurviewGetHandler.InitTree',
				method: 'get',
				rownumbers: true, 
				idField: 'Id',
				treeField: 'Name',
                columns:[[
                   {field:'ck',checkbox:true},
                   {field:'Name',title:'名称',width:220,align:'left'},
                   {field:'OperateCode',title:'操作码',width:200,align:'left'},
                   {field:'Description',title:'描述',width:400,align:'left'}
                   ]],
                toolbar:toolbar,
                lines: true,
                onClickRow: onClickRow
			">
        </table>
    </div>
</asp:Content>
