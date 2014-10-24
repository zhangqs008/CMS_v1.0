<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ColumnManage.aspx.cs" Inherits="HC.WebSite.Admin.Column.ColumnManage" %>

<%@ Import Namespace="HC.Foundation.Context.Principal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var selectedRow = undefined;
        function onClickRow(row) {
            selectedRow = row;
        }

        function onDblClickRow(row) {
            //$("#table").treegrid("toggle", row.id);
            top.addTab("编辑栏目", "<%= BasePath %>Admin/Column/ColumnEdit.aspx?action=modify&Id=" + row.id);
        }

        //添加
        function Add() {
            var parentId = 1;
            if (selectedRow) {
                parentId = selectedRow.id;
            }
            top.addTab("编辑栏目", "<%= BasePath %>Admin/Column/ColumnEdit.aspx?parentId=" + parentId);
        }
        //编辑
        function Edit() {
            if (!selectedRow) {
                $.hc.alertErrorMsg('请选择要操作的数据行!');
                return;
            }
            top.addTab("编辑栏目", "<%= BasePath %>Admin/Column/ColumnEdit.aspx?action=modify&Id=" + selectedRow.id);
        }
        //排序
        function Sort() {
            if (!selectedRow) {
                $.hc.alertErrorMsg('请选择要操作的数据行!');
                return;
            }
            if (selectedRow.children && selectedRow.children.length > 0) {
                top.addTab("栏目排序", "<%= BasePath %>Admin/Column/ColumnSort.aspx?action=sort&Id=" + selectedRow.id);
            } else {
                $.hc.alertErrorMsg('对不起，该栏目下无子栏目，不能进行排序');
                return;
            }
        }

        //字段管理
        function FieldManage() {
            if (!selectedRow) {
                $.hc.alertErrorMsg('请选择要操作的数据行!');
                return;
            }
            top.addTab(selectedRow.text + " 字段管理", "<%= BasePath %>Admin/Column/ColumnFieldManage.aspx?ColumnId=" + selectedRow.id);
        }

        //删除
        function Delete() {
            if (!selectedRow) {
                $.hc.alertErrorMsg('请选择要操作的数据行!');
                return;
            }
            $.hc.confirm("该栏目的所有子栏目也将被删除\n确定要删除该栏目吗？", function () {
                $.hc.ajax('ColumnPostHandler.Delete', {
                    params: { id: selectedRow.id },
                    success: function (itemResponse) {
                        var item = eval(itemResponse)[0];
                        if (item.status.toLocaleLowerCase() == "true") {
                            Reload();
                            selectedRow = undefined;
                            $.hc.confirm("操作成功!<br/><br/>关闭当前窗口？<br/><br/>", function () { top.closeTab(); });
                        } else {
                            $.hc.alertErrorMsg("操作失败，请与管理员联系！");
                        }
                    }
                });
            });
        }
        //刷新
        function Reload() {
            $('#table').treegrid('options').url = $('#table').treegrid('options').url.replace(/_t=\d+/, "_t=" + new Date().getTime());
            $('#table').treegrid('reload');
            $('#table').treegrid('unselectAll');
            selectedRow = undefined;
        }
        function onContextMenu(e, row) {
            e.preventDefault();
            $(this).treegrid('select', row.id);
            $('#mm').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        }
        function cellStyler(value, row, index) {
          
        }
        function add() {
            var node = $('#table').treegrid('getSelected');
            top.addTab("添加信息", "<%= BasePath %>Admin/Content/Edit.aspx?columnId=" + node.id);
        }
        function view() {
            var node = $('#table').treegrid('getSelected');
            top.addTab("信息管理", "<%= BasePath %>Admin/Content/List.aspx?columnId=" + node.id);
        }
        function edit() {
            var node = $('#table').treegrid('getSelected');
            top.addTab("编辑栏目", "<%= BasePath %>Admin/Column/ColumnEdit.aspx?action=modify&Id=" + node.id);
        }
        function fields() {
            var node = $('#table').treegrid('getSelected');
            top.addTab(node.text + " 栏目字段", "<%= BasePath %>Admin/Column/ColumnFieldManage.aspx?ColumnId=" + node.id);
        }
    </script>
    <div class="easyui-layout">
        <table id="table" title="" style="height: 400px;" class="easyui-treegrid" data-options="
				url: '<%= BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=ColumnGetHandler.InitTree',
				method: 'get',
				rownumbers: false, 
				idField: 'id',
				treeField: 'text',
                columns:[[{field:'text',title:'栏目信息',width:220,align:'left',styler:cellStyler}]],
                
                toolbar:'#myToolbar',
                lines: true,
                onClickRow: onClickRow, 
                onDblClickRow:onDblClickRow,
                onContextMenu: onContextMenu
			">
        </table>
    </div>
    <div id="myToolbar" class="datagrid-toolbar">
        <table cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style='<%= AdministratorService.Instance.HasPermissions("ColumnAdd") ? "": "display:none"%>'>
                        <a onclick="Add();" data-options="plain:true,iconCls:'icon-add'" class='easyui-linkbutton'
                            href='javascript:void(0)'>添加 </a>
                    </td>
                    <td style='<%=AdministratorService.Instance.HasPermissions("ColumnEdit") ? "": "display:none"%>'>
                        <a onclick="Edit();" data-options="plain:true,iconCls:'icon-edit'" class='easyui-linkbutton'
                            href='javascript:void(0)'>编辑 </a>
                    </td>
                    <td style='<%=AdministratorService.Instance.HasPermissions("ColumnFieldManage") ? "": "display:none"%>'>
                        <a onclick="FieldManage();" data-options="plain:true,iconCls:'icon-custom-detail'" class='easyui-linkbutton'
                            href='javascript:void(0)'>字段管理 </a>
                    </td>
                    <td style='<%=AdministratorService.Instance.HasPermissions("ColumnSort") ? "": "display:none"%>'>
                        <a onclick="Sort();" data-options="plain:true,iconCls:'icon-custom-sortalphabet'"
                            class='easyui-linkbutton' href='javascript:void(0)'>排序 </a>
                    </td>
                    <td style='<%= AdministratorService.Instance.HasPermissions("ColumnDelete") ? "": "display:none"%>'>
                        <a onclick="Delete();" data-options="plain:true,iconCls:'icon-remove'" class='easyui-linkbutton'
                            href='javascript:void(0)'>删除 </a>
                    </td>
                    <td>
                        <a onclick="Reload();" data-options="plain:true,iconCls:'icon-reload'" class='easyui-linkbutton'
                            href='javascript:void(0)'>刷新 </a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
     <div id="mm" class="easyui-menu" style="width: 120px;">
                <div onclick="add()" data-options="iconCls:'icon-add'">
                    添加信息</div>
                <div onclick="view()" data-options="iconCls:'icon-custom-ui_saccordion'">
                    管理信息</div>
                <div class="menu-sep">
                </div>
                <div onclick="edit()" data-options="iconCls:'icon-edit'">
                    编辑栏目</div>
                <div onclick="fields()" data-options="iconCls:'icon-custom-detail'">
                    栏目字段</div>
            </div>
</asp:Content>
