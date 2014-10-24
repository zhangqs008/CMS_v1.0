<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="RoleMember.aspx.cs" Inherits="HC.WebSite.Admin.Role.RoleMember" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var toolbar = [
            {
                text: '用户名<input type="text" id="txtLoginName" class="easyui-textbox"/>'
            },
            {
                text: '搜索',
                iconCls: 'icon-search',
                handler: function() {
                    $('#table').datagrid('options').url = '<%= BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=AdminGetHandler.GetSearchPageData&name=' + escape($("#txtLoginName").val());
                    $('#table').datagrid("reload");
                }
            }, {
                text: '添加',
                iconCls: 'icon-add',
                handler: function() {
                    $.hc.confirm("确定要将选中用户添加到该角色吗？", function() {
                        var adminIds = "";
                        var selectNodes = $("#table").datagrid('getSelections');
                        for (var i = 0; i < selectNodes.length; i++) {
                            if (selectNodes[i]["Id"]) {
                                adminIds += selectNodes[i]["Id"] + ",";
                            }
                        }
                        if (adminIds.length > 0) {
                            $.hc.ajax('RoleMemberPostHandler.Add', {
                                params: {
                                    roleId: <%= RequestInt32("Id") %>,
                                    adminIds: adminIds
                                },
                                success: function(response) {
                                    var item = eval(response)[0];
                                    if (item.status.toLocaleLowerCase() == "true") {
                                        $('#tableExsit').datagrid('reload');
                                                                                $.hc.confirm("操作成功!<br/><br/>关闭当前窗口？<br/><br/>", function () { top.closeTab(); });
                                    } else {
                                        $.hc.alertErrorMsg("操作失败，请与管理员联系！");
                                    }
                                }
                            });
                        }
                    });

                }
            },
            {
                text: '刷新',
                iconCls: 'icon-reload',
                handler: function() {
                    $('#table').datagrid('reload');
                }
            }];
    </script>
    <table id="table" class="easyui-datagrid" style="height: 300px" data-options="
				title: '用户列表',
				url: '<%= BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=AdminGetHandler.GetSearchPageData&name=*',
				method: 'get',
                singleSelect:false,
				rownumbers: true, 
				idField: 'Id',
				treeField: 'LoginName',
                columns:[[ 
                    {field:'ck',checkbox:true},
                    {field:'LoginName',title:'用户名',width:200,align:'left'},
                    {field:'Email',title:'邮箱',width:200,align:'left'},
                    {field:'Phone',title:'电话',width:200,align:'left'}
                ]],
                toolbar:toolbar,
                pagination:true,
			">
    </table>
    <table id="tableExsit" class="easyui-datagrid" style="height: 300px" data-options="
				title: '<%= Name %> 已有用户',
				url: '<%= BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=AdminGetHandler.GetRoleAdminPageData&roleid=<%= RequestInt32("id") %>&name=*',
				method: 'get',
                singleSelect:false,
				rownumbers: true, 
				idField: 'Id',
				treeField: 'LoginName',
                columns:[[ 
                    {field:'ck',checkbox:true},
                    {field:'LoginName',title:'用户名',width:200,align:'left'},
                    {field:'Email',title:'邮箱',width:200,align:'left'},
                    {field:'Phone',title:'电话',width:200,align:'left'}
                ]] ,
                toolbar:toolbarExsit,
                pagination:true,
			">
    </table>
    <script type="text/javascript">
        var toolbarExsit = [
            {
                text: '用户名<input type="text" id="txtExsitLoginName" class="easyui-textbox"/>'
            }, {
                text: '搜索',
                iconCls: 'icon-search',
                handler: function() {
                    $('#tableExsit').datagrid('options').url = '<%= BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=AdminGetHandler.GetRoleAdminPageData&RoleId=<%= RequestInt32("id") %>&name=' + escape($("#txtExsitLoginName").val());
                    $('#tableExsit').datagrid("reload");
                }
            }, {
                text: '移除',
                iconCls: 'icon-add',
                handler: function() {
                    $.hc.confirm("确定要将选中用户从该角色移除吗？", function() {
                        var adminIds = "";
                        var selectNodes = $("#tableExsit").datagrid('getSelections');
                        for (var i = 0; i < selectNodes.length; i++) {
                            if (selectNodes[i]["Id"]) {
                                adminIds += selectNodes[i]["Id"] + ",";
                            }
                        }
                        if (adminIds.length > 0) {
                            $.hc.ajax('RoleMemberPostHandler.Delete', {
                                params: {
                                    roleId: <%= RequestInt32("Id") %>,
                                    adminIds: adminIds
                                },
                                success: function(response) {
                                    var item = eval(response)[0];
                                    if (item.status.toLocaleLowerCase() == "true") {
                                        Reload();
                                        $.hc.confirm("操作成功!<br/><br/>关闭当前窗口？<br/><br/>", function () { top.closeTab(); });
                                    } else {
                                        $.hc.alertErrorMsg("操作失败，请与管理员联系！");
                                    }
                                }
                            });
                        }
                    });

                }
            }, {
                text: '刷新',
                iconCls: 'icon-reload',
                handler: function() {
                    Reload();
                }
            }];

        function Reload() {
            $('#tableExsit').datagrid('options').url = $('#tableExsit').datagrid('options').url.replace(/_t=\d+/, "_t=" + new Date().getTime());
            $('#tableExsit').datagrid('reload'); 
        }
    </script>
    <script type="text/javascript">
        window.customResize = function () {
            var width = $(window).width();
            var height = $(window).height();
            $('#table').datagrid('resize', { width: width, height: height / 2 });
            $('#tableExsit').datagrid('resize', { width: width, height: height / 2 });
        };
    </script>
</asp:Content>
