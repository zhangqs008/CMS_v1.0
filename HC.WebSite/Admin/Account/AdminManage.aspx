<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AdminManage.aspx.cs" Inherits="HC.WebSite.Admin.Log.LogManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var editId = 0;
        function onClickRow() {
            var item = $('#table').datagrid("getSelected");
            editId = item.Id;
        }

        function onDblClickRow() {
            var item = $('#table').datagrid("getSelected");
            editId = item.Id;
            top.addTab("编辑用户", "<%=BasePath%>Admin/Account/AdminEdit.aspx?action=modify&Id=" + editId);
        }

        var toolbar = [
            {
                text: '关键字&nbsp;<input type="text" id="txtLoginName" style="width:200px;" placeholder="用户名,邮箱,电话,个人主题"  class="easyui-textbox"/>'
            },
            {
                text: '搜索',
                iconCls: 'icon-search',
                handler: function () {
                    $('#table').datagrid('options').url = '<%=BasePath %>ajaxget.aspx?_t=' + new Date().getTime() + '&_action=AdminGetHandler.GetSearchPageData&name=' + escape($("#txtLoginName").val());
                    reload();
                }
            }, {
                text: '添加',
                iconCls: 'icon-add',
                handler: function () {
                    top.addTab("编辑用户", "<%=BasePath%>Admin/Account/AdminEdit.aspx");
                }
            }, {
                text: '编辑',
                iconCls: 'icon-edit',
                handler: function () {
                    if (editId <= 0) {
                        $.hc.alertErrorMsg('请选择要操作的数据行!');
                        return;
                    }
                    top.addTab("编辑用户", "<%=BasePath%>Admin/Account/AdminEdit.aspx?action=modify&Id=" + editId);

                }
            }, '-', {
                text: '删除',
                iconCls: 'icon-remove',
                handler: function () {
                    if (!editId) {
                        $.hc.alertErrorMsg('请选择要操作的数据行!');
                        return;
                    }
                    $.hc.confirm("确定要删除该项吗？", function () {
                        $.hc.ajax('AdminPostHandler.Delete', {
                            params: { id: editId },
                            success: function (itemResponse) {
                                var item = eval(itemResponse)[0];
                                if (item.status.toLocaleLowerCase() == "true") {
                                    reload();
                                    editId = undefined;
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
                text: '菜单权限',
                iconCls: 'icon-custom-lock',
                handler: function () {
                    if (!editId) {
                        $.hc.alertErrorMsg('请选择要操作的数据行!');
                        return;
                    }
                    top.addTab("用户菜单权限", "<%=BasePath%>Admin/Account/AdminPurview.aspx?Id=" + editId);
                }
            },
            {
                text: '刷新',
                iconCls: 'icon-reload',
                handler: function () {
                    reload();
                    editId = 0;
                }
            }];
        function reload() {

            var url = $('#table').datagrid('options').url;
            if (url.indexOf("_t=") > 0) {
                url = url.replace(/_t=\d+/, "_t=" + new Date().getTime());
            } else {
                url = url.indexOf("?") > 0
                    ? url + "&_t=" + new Date().getTime()
                    : url + "?_t=" + new Date().getTime();
            }


            $('#table').datagrid('options').url = url;
            $('#table').datagrid('reload');
            $('#table').datagrid('unselectAll');
        }
    </script>
    <table id="table" class="easyui-datagrid" style="height: 370px" data-options="
				title: '',
				url: '<%=BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=AdminGetHandler.GetSearchPageData&name=*',
				method: 'get',
                singleSelect:true,
				rownumbers: true, 
				idField: 'id',
				treeField: 'LoginName',
                columns:[[ 
                    {field:'ck',checkbox:true},
                    {field:'LoginName',title:'用户名',width:150,align:'left'},
                    {field:'Email',title:'邮箱',width:150,align:'left'},
                    {field:'Phone',title:'电话',width:100,align:'left'},
                    {field:'Theme',title:'个人主题',width:80,align:'center'},
                    {field:'LoginCount',title:'登录次数',width:80,align:'center'},
                    {field:'CreateDate',title:'注册时间',width:120,align:'center',formatter: function(value,row,index){
                            return $.hc.fixJsonDate(row.CreateDate);
                    }},
                    {field:'LastLogOffTime',title:'最后登录时间',width:120,align:'center',formatter: function(value,row,index){
                            return $.hc.fixJsonDate(row.LastLogOffTime);
                    }}
                ]],
                toolbar:toolbar,
                pagination:true,
                pageSize:20,
                onClickRow: onClickRow,
                onDblClickRow:onDblClickRow
			">
    </table>
</asp:Content>
