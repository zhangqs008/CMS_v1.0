<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ContentManage.aspx.cs" Inherits="HC.WebSite.Admin.Content.ContentManage" %>

<%@ Import Namespace="HC.Foundation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="<%= BasePath %>Admin/Scripts/jquery-easyui-1.4/jquery.ezsyui.plugin.tab.js"> </script>
    <div class="easyui-layout">
        <div id="west" data-options="region:'west',split:true" title="" style="width: 280px;">
            <script type="text/javascript">
                var selectedRow = undefined;
                function onClickRow(row) {
                    selectedRow = row;
                    var node = $('#table').treegrid('getSelected');
                    addTab("信息管理", "List.aspx?columnId=" + node.id);
                }
                function onDblClickRow(row) {
                    top.addTab("编辑栏目", "<%= BasePath %>Admin/Column/ColumnEdit.aspx?action=modify&Id=" + row.id);
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
                    return 'border-right: 0;border-left: 0';
                }

                function add() {
                    var node = $('#table').treegrid('getSelected');
                    addTab("添加信息", "<%= BasePath %>Admin/Content/Edit.aspx?columnId=" + node.id);
                }
                function view() {
                    var node = $('#table').treegrid('getSelected');
                    addTab("信息管理", "<%= BasePath %>Admin/Content/List.aspx?columnId=" + node.id);
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
            <table id="table" title="" style="height: 400px;" class="easyui-treegrid" data-options="
				url: '<%= BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=ColumnGetHandler.InitTree',
				method: 'get',
				rownumbers: false, 
                lines: true,
				idField: 'id',
				treeField: 'text',
                columns:[[{field:'text',title:'栏目信息（☆提示：右键弹出菜单操作）',width:220,align:'left',styler:cellStyler}]], 
                onClickRow: onClickRow, 
                onDblClickRow:onDblClickRow,fit:true,border:0,fitColumns:true,
                onContextMenu: onContextMenu
			">
            </table>
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
        </div>
        <div id="right" data-options="region:'center'" title="">
            <div id="tt" class="easyui-tabs" fit="true" border="false" plain="true" style="height: 80%;
                width: 100%;">
                <div title="信息管理" data-options="tools:'#p-tools',fit:true,plain:true" style="padding: 1px;">
                    <div style="padding: 10px;">
                        温馨提示：请从左侧栏目导航弹出右键菜单以进行操作，谢谢</div>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            function addTab(title, url) {
                if ($('#tt').tabs('exists', title)) {
                    $('#tt').tabs('select', title);
                    var allTabs = $('#tt').tabs('tabs');
                    $.each(allTabs, function () {
                        var ctab = this;
                        var opt = ctab.panel('options');
                        if (opt.title == title) {
                            //刷新Tab页 
                            var newContent = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
                            $('#tt').tabs('update', {
                                tab: ctab,
                                options: {
                                    title: title,
                                    content: newContent
                                }
                            });
                        }
                    });
                }
                else {
                    var content = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
                    $('#tt').tabs('add', {
                        title: title,
                        content: content,
                        height: '400px',
                        closable: true
                    });
                }
            }
            //关闭tab页
            function closeTab() {
                var tab = $('#tt').tabs('getSelected');
                if (tab) {
                    var index = $('#tt').tabs('getTabIndex', tab);
                    $('#tt').tabs('close', index);
                }
            }
        </script>
    </div>
</asp:Content>
