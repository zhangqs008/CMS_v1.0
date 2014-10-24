<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ColumnFieldManage.aspx.cs" Inherits="HC.WebSite.Admin.Column.ColumnFieldManage" %>

<%@ Import Namespace="HC.Foundation.Context.Principal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="<%= BasePath %>Admin/Scripts/jquery-easyui-1.4/datagrid-dnd.js" type="text/javascript"> </script>
    <script type="text/javascript">
        //搜索
        function Search() {
            $('#table').datagrid('options').url = '<%= BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=ModuleFieldGetHandler.GetSearchPageData&moduleId=<%= Module.Id %>&name=' + escape($("[name='keyword']").val());
            reload();
        }

        //添加模型字段
        function AddField() {
            top.addTab("编辑模型字段", '<%= BasePath %>Admin/Module/ModuleFieldEdit.aspx?moduleId=<%= Module.Id %>');
        }

        //添加到栏目
        function Add() {
            var items = $('#table').datagrid("getSelections");
            var ids = "";
            for (var i = 0; i < items.length; i++) {
                if (items[i]["Id"]) {
                    ids += items[i]["Id"] + ",";
                }
            }
            if (ids.length > 0) {
                $.hc.confirm("确定要将字段 <span style='color:red'>添加</span> 到栏目吗?", function() {
                    $.hc.ajax('ColumnFieldPostHandler.Add', {
                        params: {
                            ids: ids,
                            columnId: <%= Column.Id %>
                        },
                        success: function(response) {
                            var item = eval(response)[0];
                            if (item.status.toLocaleLowerCase() == "true") {
                                reload();
                                ReloadExsit();
                                $.hc.alertSucessMsg("操作成功");
                            } else {
                                $.hc.alertErrorMsg(item.body);
                            }
                        }
                    });
                });
            } else {
                $.hc.alertErrorMsg('请选择要操作的数据行!');
            }
        }

        //刷新 
        function reload() {
                $('#table').datagrid('options').url = $('#table').datagrid('options').url.replace(/_t=\d+/, "_t=" + new Date().getTime());
                $('#table').datagrid('reload');
                $('#table').datagrid('unselectAll');
            }

        //排序 
        function Sort() {
            var items = $('#table').datagrid("getData");
            var sorts = "";
            for (var i = 0; i < items.rows.length; i++) {
                sorts += items.rows[i].Id + "-" + (i + 1) + "|";
            }
            $.hc.confirm("确定要保存排序吗？", function() {
                $.hc.ajax('ModuleFieldPostHandler.Sort', {
                    params: { sorts: sorts },
                    success: function(response) {
                        var item = eval(response)[0];
                        if (item.status.toLocaleLowerCase() == "true") {
                            $.hc.confirm("恭喜您，操作成功！<br/><br/>关闭本窗口？<br/><br/>", function() { top.closeTab(); });
                        } else {
                            $.hc.alertErrorMsg("操作失败，请与管理员联系！");
                        }
                    }
                });
            });
        }
    </script>
    <script type="text/javascript">
        window.customResize = function () {
            var width = $(window).width();
            var height = $(window).height();
            $('#layout').layout('resize', { width: width, height: height });
            $('#table').datagrid('resize', { width: $('#west').css("width"), height: $('#west').css("height") });
            $('#tableExsit').datagrid('resize', { width: $('#right').css("width"), height: $('#right').css("height") });
        };
    </script>
    <div id="layout" class="easyui-layout">
        <div id="west" data-options="region:'west',split:true" title="模型字段 <%= Module.Name %>(<%= Module.TableName %>)"
            style="width: 50%;">
            <table id="table" class="easyui-datagrid" data-options="
				title: '',
				url: '<%= BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=ModuleFieldGetHandler.GetSearchPageData&moduleId=<%= Module.Id %>&name=*',
				method: 'get',
                singleSelect:false,
				rownumbers: true, 
				idField: 'Id', 
                columns:[[ 
                    {field:'ck',checkbox:true}, 
                    {field:'Name',title:'字段名称',width:100,align:'left'},
                    {field:'Note',title:'备注名称',width:100,align:'left'},
                    {field:'TypeName',title:'字段类型',width:100,align:'center'} 
                ]],
                toolbar:'#myToolbar',
                pagination:true,
                fitColumns: true,
                pageSize:20, 
                border:0,
                fit:true,
                onLoadSuccess:function(){
					$(this).datagrid('enableDnd');
				}
			">
            </table>
            <div id="myToolbar" class="datagrid-toolbar">
                <table cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td>
                                <input name="keyword" id="keyword" type="text" class="easyui-textbox" style="width: 120px;" />
                                <script type="text/javascript">
                                    $(function () {
                                        var defaultKeyword = "字段名称,字段备注";
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
                                <a onclick=" Search(); " data-options="plain:true,iconCls:'icon-search'" class='easyui-linkbutton'
                                    href='javascript:void(0)'>搜索 </a>
                            </td>
                            <td style='<%= AdministratorService.Instance.HasPermissions("ColumnFieldManage") ? "": "display:none"%>'>
                                <a onclick=" Add(); " data-options="plain:true,iconCls:'icon-custom-right'" class='easyui-linkbutton'
                                    href='javascript:void(0)'>添加到栏目 </a>
                            </td>
                            <td style='<%= AdministratorService.Instance.HasPermissions("ModuleAddField") ? "": "display:none"%>'>
                                <a onclick=" AddField(); " data-options="plain:true,iconCls:'icon-add'" class='easyui-linkbutton'
                                    href='javascript:void(0)'>模型字段 </a>
                            </td>
                            <td>
                                <a onclick=" Sort(); " data-options="plain:true,iconCls:'icon-custom-sortalphabet'"
                                    class='easyui-linkbutton' href='javascript:void(0)'>排序 </a>
                            </td>
                            <td>
                                <a onclick=" reload(); " data-options="plain:true,iconCls:'icon-reload'" class='easyui-linkbutton'
                                    href='javascript:void(0)'>刷新 </a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <%--已有字段--%>
        <div id="right" data-options="region:'center'" title="已有字段 <%= Column.Name %> ">
            <table id="tableExsit" class="easyui-datagrid" style="height: 580px" data-options="
				title: '',
				url: '<%= BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=ColumnFieldGetHandler.GetSearchPageData&columnId=<%= Column.Id %>&name=*',
				method: 'get',
                singleSelect:true,
				rownumbers: true, 
				idField: 'Id', 
                columns:[[ 
                    {field:'ck',checkbox:true}, 
                    {field:'Name',title:'字段名称',width:100,align:'left'},
                    {field:'Note',title:'备注名称',width:100,align:'left'},
                    {field:'TypeName',title:'字段类型',width:100,align:'center'}
                ]],
                toolbar:'#myToolbar2',
                pagination:true,
                fitColumns: true,
                pageSize:20, 
                border:0,
                fit:true,
                onLoadSuccess:function(){
					$(this).datagrid('enableDnd');
				}
			">
            </table>
            <div id="myToolbar2" class="datagrid-toolbar">
                <table cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td>
                                <input name="keywordExsit" id="keywordExsit" type="text" class="easyui-textbox" style="width: 120px;" />
                                <script type="text/javascript">
                                    $(function () {
                                        var defaultKeyword = "字段名称,字段备注";
                                        $("[name='keywordExsit']").prev().val(defaultKeyword);
                                        //获取焦点
                                        $("[name='keywordExsit']").prev().focus(function () {
                                            if ($.trim($(this).val()) == defaultKeyword) {
                                                $(this).val("");
                                            }
                                        });
                                        //失去焦点
                                        $("[name='keywordExsit']").prev().blur(function () {
                                            if ($.trim($(this).val()) == "") {
                                                $(this).val(defaultKeyword);
                                            }
                                        });
                                        $("[name='keywordExsit']").prev().bind('keydown', function (e) {
                                            var key = e.which;
                                            if (key == 13) {
                                                e.preventDefault();
                                                $(this).blur(); //失去焦点，给隐藏域赋值
                                                SearchExsit();
                                            }
                                        });
                                    })
                                </script>
                            </td>
                            <td>
                                <a onclick=" SearchExsit(); " data-options="plain:true,iconCls:'icon-search'" class='easyui-linkbutton'
                                    href='javascript:void(0)'>搜索 </a>
                            </td>
                            <td style='<%= AdministratorService.Instance.HasPermissions("ColumnFieldManage") ? "": "display:none"%>'>
                                <a onclick=" DeleteExsit(); " data-options="plain:true,iconCls:'icon-custom-left'"
                                    class='easyui-linkbutton' href='javascript:void(0)'>从栏目移除 </a>
                            </td>
                            <td>
                                <a onclick=" EditExsit(); " data-options="plain:true,iconCls:'icon-edit'" class='easyui-linkbutton'
                                    href='javascript:void(0)'>编辑 </a>
                            </td>
                            <td>
                                <a onclick=" SortExsit(); " data-options="plain:true,iconCls:'icon-custom-sortalphabet'"
                                    class='easyui-linkbutton' href='javascript:void(0)'>排序 </a>
                            </td>
                            <td>
                                <a onclick=" ReloadExsit(); " data-options="plain:true,iconCls:'icon-reload'" class='easyui-linkbutton'
                                    href='javascript:void(0)'>刷新 </a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <script type="text/javascript"> 

                //搜索
                function SearchExsit() {
                    $('#tableExsit').datagrid('options').url = '<%= BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=ColumnFieldGetHandler.GetSearchPageData&ColumnId=<%= Column.Id %>&name=' + escape($("[name='keywordExsit']").val());
                    ReloadExsit();
                }


                //删除 
                function DeleteExsit() {
                    var items = $('#tableExsit').datagrid("getSelections");
                    var ids = "";
                    for (var i = 0; i < items.length; i++) {
                        if (items[i]["Id"]) {
                            ids += items[i]["Id"] + ",";
                        }
                    }
                    if (ids.length > 0) {
                        $.hc.confirm("确定要将字段从栏目 <span style='color:red'>移除</span> 吗?", function() {
                            $.hc.ajax('ColumnFieldPostHandler.Delete', {
                                params: {
                                    ids: ids,
                                    columnId: <%= Column.Id %>
                                },
                                success: function(response) {
                                    var item = eval(response)[0];
                                    if (item.status.toLocaleLowerCase() == "true") {
                                        ReloadExsit();
                                        $.hc.alertSucessMsg("操作成功");
                                    } else {
                                        $.hc.alertErrorMsg("操作失败，请与管理员联系！");
                                    }
                                }
                            });

                        });
                    } else {
                        $.hc.alertErrorMsg('请选择要操作的数据行!');
                    }
                }

                //刷新 
                 function ReloadExsit() {
                $('#tableExsit').datagrid('options').url = $('#tableExsit').datagrid('options').url.replace(/_t=\d+/, "_t=" + new Date().getTime());
                $('#tableExsit').datagrid('reload');
                $('#tableExsit').datagrid('unselectAll');
            }

               //编辑
                function EditExsit( ) {
                    var items = $('#tableExsit').datagrid("getSelections");
                    if(items==undefined) {
                      $.hc.alertErrorMsg('请选择要操作的数据行!');
                     }
                    top.addTab("编辑栏目字段", '<%= BasePath %>Admin/Column/ColumnFieldEdit.aspx?action=modify&moduleId=<%= Module.Id %>&id='+items[0].Id);
                }

                //排序 
                function SortExsit() {
                    var items = $('#tableExsit').datagrid("getData");
                    var sorts = "";
                    for (var i = 0; i < items.rows.length; i++) {
                        sorts += items.rows[i].Id + "-" + (i + 1) + "|";
                    }
                    $.hc.confirm("确定要保存排序吗？", function() {
                        $.hc.ajax('ColumnFieldPostHandler.Sort', {
                            params: { sorts: sorts },
                            success: function(response) {
                                var item = eval(response)[0];
                                if (item.status.toLocaleLowerCase() == "true") {
                                    $.hc.confirm("恭喜您，操作成功！<br/><br/>关闭本窗口？<br/><br/>", function() { top.closeTab(); });
                                } else {
                                    $.hc.alertErrorMsg("操作失败，请与管理员联系！");
                                }
                            }
                        });
                    });
                }
            </script>
        </div>
    </div>
</asp:Content>
