<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ModuleManage.aspx.cs" Inherits="HC.WebSite.Admin.Module.ModuleManage" %>

<%@ Import Namespace="HC.Foundation.Context.Principal" %>
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
            top.addTab("编辑内容模型", "<%=BasePath%>Admin/Module/ModuleEdit.aspx?action=modify&Id=" + editId);
        }

        //搜索 
        function Search() {
            $('#table').datagrid('options').url = '<%=BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=ModuleGetHandler.GetSearchPageData&name=' + escape($("[name='keyword']").val());
            $('#table').datagrid("reload");
        }
        //添加
        function Add() {
            top.addTab("编辑内容模型", "<%=BasePath%>Admin/Module/ModuleEdit.aspx");
        }
        //编辑
        function Edit() {
            if (editId <= 0) {
                $.hc.alertErrorMsg('请选择要操作的数据行!');
                return;
            }
            top.addTab("编辑内容模型", "<%=BasePath%>Admin/Module/ModuleEdit.aspx?action=modify&Id=" + editId);

        }
        //删除
        function Delete() {
            if (!editId) {
                $.hc.alertErrorMsg('请选择要操作的数据行!');
                return;
            }
            $.hc.confirm("确定要删除该项吗？", function () {
                $.hc.ajax('ModulePostHandler.Delete', {
                    params: { id: editId },
                    success: function (itemResponse) {
                        var item = eval(itemResponse)[0];
                        if (item.status.toLocaleLowerCase() == "true") {
                            $('#table').datagrid('reload');
                            editId = undefined;
                            $.hc.confirm("操作成功!<br/><br/>关闭当前窗口？<br/><br/>", function () { top.closeTab(); });
                        } else {
                            $.hc.alertErrorMsg("操作失败，请与管理员联系！");
                        }
                    }
                });
            });

        }

        //字段管理
        function FieldManage() {
            if (editId <= 0) {
                $.hc.alertErrorMsg('请选择要操作的数据行!');
                return;
            }
            top.addTab("字段管理", "/Admin/Module/ModuleFieldManage.aspx?&moduleId=" + editId);
        }
        //刷新
        function Reload() {
            $('#table').datagrid('reload');
            editId = 0;
        } 
    </script>
    <div region="center" style="width: 100%; height: 100%" border="false">
        <table id="table" class="easyui-datagrid" style="height: 580px" data-options="
				title: '',
				url: '<%=BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=ModuleGetHandler.GetSearchPageData&name=*',
				method: 'get',
                singleSelect:true,
				rownumbers: true, 
				idField: 'Id', 
                columns:[[ 
                    {field:'ck',checkbox:true}, 
                     {field:'Name',title:'模型名称',width:100,align:'left'},
                     {field:'TableName',title:'数据库表名',width:100,align:'left'},
                     {field:'Description',title:'模型描述',width:100,align:'left'},      
                     {field:'CreateDate',title:'操作时间',width:100,align:'center',formatter: function(value,row,index){
                            return $.hc.fixJsonDate(row.CreateDate);
                    }} 
                ]],
                toolbar:'#myToolbar',
                pagination:true,
                fitColumns: true,
                pageSize:20,
                onClickRow: onClickRow,
                onDblClickRow:onDblClickRow
			">
        </table>
    </div>
    <div id="myToolbar" class="datagrid-toolbar">
        <table cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td>
                        关键字&nbsp;<input name="keyword" id="keyword" type="text" class="easyui-textbox" style="width: 230px;" />
                        <script type="text/javascript">
                            $(function () {
                                var defaultKeyword = "模型名称,描述,数据库表名";
                                $("[name='keyword']").prev().val(defaultKeyword);
                                //获取焦点
                                $("[name='keyword']").prev().focus(function () {
                                    if ($.trim($(this).val()) == defaultKeyword) {
                                        $(this).val("");
                                    }
                                });
                                //失去焦点
                                $("[name='keyword']").prev().blur(function () {
                                    if ($.trim($(this).val()) == "") {
                                        $(this).val(defaultKeyword);
                                    }
                                });
                                $("[name='keyword']").prev().bind('keydown', function (e) {
                                    var key = e.which;
                                    if (key == 13) {
                                        e.preventDefault();
                                        $(this).blur(); //失去焦点，给隐藏域赋值
                                        Search();
                                    }
                                });
                            })
                        </script>
                    </td>
                    <td>
                        <a onclick="Search();" data-options="plain:true,iconCls:'icon-search'" class='easyui-linkbutton'
                            href='javascript:void(0)'>搜索 </a>
                    </td>
                    <td style='<%= AdministratorService.Instance.HasPermissions("ModuleAdd") ? "": "display:none"%>'>
                        <a onclick="Add();" data-options="plain:true,iconCls:'icon-add'" class='easyui-linkbutton'
                            href='javascript:void(0)'>添加 </a>
                    </td>
                    <td style='<%=AdministratorService.Instance.HasPermissions("ModuleEdit") ? "": "display:none"%>'>
                        <a onclick="Edit();" data-options="plain:true,iconCls:'icon-edit'" class='easyui-linkbutton'
                            href='javascript:void(0)'>编辑 </a>
                    </td>
                    <td style='<%=AdministratorService.Instance.HasPermissions("ModuleFieldManage") ? "": "display:none"%>'>
                        <a onclick="FieldManage();" data-options="plain:true,iconCls:'icon-custom-detail'" class='easyui-linkbutton'
                            href='javascript:void(0)'>字段管理 </a>
                    </td>
                    <td style='<%= AdministratorService.Instance.HasPermissions("ModuleDelete") ? "": "display:none"%>'>
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
</asp:Content>
