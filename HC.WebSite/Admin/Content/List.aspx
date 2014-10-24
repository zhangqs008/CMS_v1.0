<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="HC.WebSite.Admin.Content.List" %>

<%@ Import Namespace="HC.Foundation.Context.Principal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div region="center" style="width: 100%; height: 100%" border="false">
        <table id="table" class="easyui-datagrid" style="height: 580px" data-options="
				title: '<%=Column.Name %>',
				url: '<%=BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=ContentGetHandler.GetPageData&columnId=<%=Column.Id %>&Title=*',
				method: 'get',
                singleSelect:true,
				rownumbers: true, 
				idField: 'Id', 
                columns:[[ 
                    {field:'ck',checkbox:true},
                    {field:'Id',title:'Id',width:30,align:'left'},

                    <%=TableColumn %>
                     
                    {field:'CreateDate',title:'操作时间',width:120,align:'center'} 
                ]], 
                pagination:true,
                toolbar:'#myToolbar',
                fitColumns: true,
                pageSize:20,
                onClickRow:onClickRow,
                onDblClickRow:onDblClickRow 
			">
        </table>
        <div id="myToolbar" class="datagrid-toolbar">
            <table cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                       <%-- <td>
                            关键字&nbsp;<input name="keyword" id="keyword" type="text" class="easyui-textbox" style="width: 230px;" />
                            <script type="text/javascript">
                                $(function () {
                                    var defaultKeyword = "字段名称,字段备注";
                                    $("[name='keywork']").prev().val(defaultKeyword);
                                    //获取焦点
                                    $("[name='keywork']").prev().focus(function () {
                                        if ($.trim($(this).val()) == defaultKeyword) {
                                            $(this).val("");
                                        }
                                    });
                                    //失去焦点
                                    $("[name='keywork']").prev().blur(function () {
                                        if ($.trim($(this).val()) == "") {
                                            $(this).val(defaultKeyword);
                                        }
                                    });
                                    $("[name='keywork']").prev().bind('keydown', function (e) {
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
                        </td>--%>
                        <td>
                            <a onclick="Edit();" data-options="plain:true,iconCls:'icon-edit'" class='easyui-linkbutton'
                                href='javascript:void(0)'>编辑 </a>
                        </td>
                        <td>
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
        <script type="text/javascript">
            var editId = 0;
            function onClickRow() {
                var item = $('#table').datagrid("getSelected");
                editId = item.Id;
            }
            function onDblClickRow() {
                var item = $('#table').datagrid("getSelected");
                editId = item.Id;
                parent.addTab("编辑信息", '<%=BasePath%>Admin/Content/Edit.aspx?action=modify&Id=' + editId+"&ColumnId="+<%=Column.Id %>);
            }

            //搜索
            function Search() {
                $('#table').datagrid('options').url = '<%=BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=ContentGetHandler.GetPageData&columnId=<%=Column.Id %>&Title=' + escape($("[name='keyword']").val());
                $('#table').datagrid("reload");
            }

            //编辑
            function Edit() {
                if (editId <= 0) {
                    $.hc.alertErrorMsg('请选择要操作的数据行!');
                    return;
                }
                parent.addTab("编辑信息", '<%=BasePath%>Admin/Content/Edit.aspx?action=modify&Id=' + editId+"&ColumnId="+<%=Column.Id %>);
            }

            //删除
            function Delete() {
                if (!editId) {
                    $.hc.alertErrorMsg('请选择要操作的数据行!');
                    return;
                }
                $.hc.confirm("确定要删除该项吗？", function () {
                    $.hc.ajax('ContentPostHandler.Delete', {
                        params: { id: editId,columnId:<%=Column.Id %> },
                        success: function (response) {
                            var item = eval(response)[0];
                            if (item.status.toLocaleLowerCase() == "true") {
                                Reload();
                                editId = undefined;
                                $.hc.alertSucessMsg("操作成功！");
                            } else {
                                $.hc.alertErrorMsg(item.body);
                            }
                        }
                    });
                });
            }

            //刷新
            function Reload() {
                $('#table').datagrid('options').url = $('#table').datagrid('options').url.replace(/_t=\d+/, "_t=" + new Date().getTime());
                $('#table').datagrid('reload');
                editId = 0;
            }
         
        </script>
    </div>
</asp:Content>
