<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
         CodeBehind="RoleManage.aspx.cs" Inherits="HC.WebSite.Admin.Role.RoleManage" %>

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
                handler: function() {
                    top.addTab("编辑角色", "<%= BasePath %>Admin/Role/RoleEdit.aspx");
                }
            }, {
                text: '编辑',
                iconCls: 'icon-edit',
                handler: function() {
                    if (!selectedRow) {
                        $.hc.alert("请选择要操作的数据行");
                        return;
                    }
                    top.addTab("编辑角色", "<%= BasePath %>Admin/Role/RoleEdit.aspx?action=modify&id=" + selectedRow.id);
                }
            }, "-", {
                text: '成员管理',
                iconCls: 'icon-custom-user',
                handler: function() {
                    if (!selectedRow) {
                        $.hc.alertErrorMsg('请选择要操作的数据行!');
                        return;
                    }
                    top.addTab("角色成员管理", "<%= BasePath %>Admin/Role/RoleMember.aspx?Id=" + selectedRow.id);
                }
            },
            {
                text: '菜单权限',
                iconCls: 'icon-custom-lock',
                handler: function() {
                    if (!selectedRow) {
                        $.hc.alertErrorMsg('请选择要操作的数据行!');
                        return;
                    }
                    top.addTab("角色菜单权限", "<%= BasePath %>Admin/Role/RolePurview.aspx?Id=" + selectedRow.id);
                }
            }, {
                text: '排序',
                iconCls: 'icon-custom-sortalphabet',
                handler: function() {
                    if (!selectedRow) {
                        $.hc.alertErrorMsg('请选择要操作的数据行!');
                        return;
                    }
                    if (selectedRow.children && selectedRow.children.length > 0) {
                        top.addTab("角色排序", "<%= BasePath %>Admin/Role/RoleSort.aspx?action=sort&Id=" + selectedRow.id);
                    } else {
                        $.hc.alertErrorMsg('对不起，该节点下无子节点，不能进行排序');
                        return;
                    }
                }
            }, "-", {
                text: '删除',
                iconCls: 'icon-remove',
                handler: function() {
                    if (!selectedRow) {
                        $.hc.alert("请选择要操作的数据行");
                        return;
                    }
                    $.hc.confirm("该节点的所有子节点也将被删除\n\r确定要删除该节点吗？", function() {
                        $.hc.ajax('RolePostHandler.Delete', {
                            params: { id: selectedRow.id },
                            success: function(itemResponse) {
                                var item = eval(itemResponse)[0];
                                if (item.status.toLocaleLowerCase() == "true") {
                                    $('#dataTable').treegrid('reload');
                                    selectedRow = undefined;
                                                                            $.hc.confirm("操作成功!<br/><br/>关闭当前窗口？<br/><br/>", function () { top.closeTab(); });
                                } else {
                                    $.hc.alertErrorMsg("操作失败，请与管理员联系！");
                                }
                            }
                        });
                    });

                }
            }, {
                text: '刷新',
                iconCls: 'icon-reload',
                handler: function() {
                    $('#dataTable').treegrid('reload');
                    selectedRow = undefined;
                }
            }];
    </script>
    <div class="easyui-layout">
        <table title="" id="dataTable" class="easyui-treegrid" style="height: 500px;"
               data-options="
                url: '<%= BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=RoleGetHandler.InitTree',
                method: 'get',
                lines: true,
                rownumbers: true,
                border:true, 
				idField: 'id',
				treeField: 'text',
                columns:[[{field:'ck',checkbox:true},{field:'text',title:'名称',width:220,align:'left'}]],
                toolbar:toolbar,
                lines: true,
                onClickRow: onClickRow
            ">
        </table>
    </div>
</asp:Content>