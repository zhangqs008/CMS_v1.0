<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
         CodeBehind="MenuManage.aspx.cs" Inherits="HC.WebSite.Admin.Menu.MenuManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var selectedRow = undefined;

        function onClickRow(row) {
            selectedRow = row;
        }

        function onDblClickRow(row) {
            $("#table").treegrid("toggle", row.id);
        }

        var toolbar = [{
                text: '添加',
                iconCls: 'icon-add',
                handler: function() {
                    var parentId = 1;
                    if (selectedRow) {
                        parentId = selectedRow.id;
                    }
                    top.addTab("编辑菜单", "<%= BasePath %>Admin/Menu/MenuEdit.aspx?parentId=" + parentId);
                }
            }, {
                text: '编辑',
                iconCls: 'icon-edit',
                handler: function() {
                    if (!selectedRow) {
                        $.hc.alertErrorMsg('请选择要操作的数据行!');
                        return;
                    }
                    top.addTab("编辑菜单", "<%= BasePath %>Admin/Menu/MenuEdit.aspx?action=modify&Id=" + selectedRow.id);
                }
            }, '-',
            {
                text: '排序',
                iconCls: 'icon-custom-sortalphabet',
                handler: function() {
                    if (!selectedRow) {
                        $.hc.alertErrorMsg('请选择要操作的数据行!');
                        return;
                    }
                    if (selectedRow.children && selectedRow.children.length > 0) {
                        top.addTab("菜单排序", "<%= BasePath %>Admin/Menu/MenuSort.aspx?action=sort&Id=" + selectedRow.id);
                    } else {
                        $.hc.alertErrorMsg('对不起，该菜单下无子菜单，不能进行排序');
                        return;
                    }
                }
            }, '-', {
                text: '删除',
                iconCls: 'icon-remove',
                handler: function() {
                    if (!selectedRow) {
                        $.hc.alertErrorMsg('请选择要操作的数据行!');
                        return;
                    }
                    $.hc.confirm("该菜单的所有子菜单也将被删除\n确定要删除该菜单吗？", function() {
                        $.hc.ajax('MenuPostHandler.Delete', {
                            params: { id: selectedRow.id },
                            success: function(itemResponse) {
                                var item = eval(itemResponse)[0];
                                if (item.status.toLocaleLowerCase() == "true") {
                                    reload();
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
                handler: function() {
                    reload();
                    selectedRow = undefined;
                }
            }];
            function reload() {
                $('#table').treegrid('options').url = $('#table').treegrid('options').url.replace(/_t=\d+/, "_t=" + new Date().getTime());
                $('#table').treegrid('reload');
                $('#table').treegrid('unselectAll');
            }
    </script>
    <div class="easyui-layout">
        <table id="table" title="" style="height: 400px;" class="easyui-treegrid" data-options="
				url: '<%= BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=MenuGetHandler.InitTree',
				method: 'get',
				rownumbers: false, 
				idField: 'id',
				treeField: 'text',
                columns:[[{title:'编号',field:'id',width:50},{field:'text',title:'名称',width:220,align:'left'}]],
                toolbar:toolbar,
                onClickRow: onClickRow, 
                onDblClickRow:onDblClickRow
			">
        </table>
    </div>
</asp:Content>