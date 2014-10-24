<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="LogManage.aspx.cs" Inherits="HC.WebSite.Admin.Log.LogManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var editId = 0;
        function onClickRow() {
            var item = $('#table').datagrid("getSelected");
            editId = item.Id;
        }

        function showLog(msg) {
            $('#details').html(htmlencode(msg));
            $('#details').dialog({
                title: '查看日志',
                width: 600,
                height: 300,
                iconCls: 'icon-custom-eye',
                closed: false,
                cache: false,
                modal: true
            });
        }

        function onDblClickRow(rowIndex, rowData) {
            showLog(rowData.Message);
        }
        function reload() {
            $('#table').datagrid('options').url = $('#table').datagrid('options').url.replace(/_t=\d+/, "_t=" + new Date().getTime());
            $('#table').datagrid('reload');
            $('#table').datagrid('unselectAll');
        }

        var toolbar = [
            {
                text: '关键字&nbsp;<input type="text" id="txtTitle" placeholder="标题,内容,操作者,IP地址"  class="easyui-textbox"/>'
            },
            {
                text: '搜索',
                iconCls: 'icon-search',
                handler: function () {
                    $('#table').datagrid('options').url = '<%=BasePath %>ajaxget.aspx?_t=' + new Date().getTime() + '&_action=LogGetHandler.GetSearchPageData&title=' + escape($("#txtTitle").val());
                    reload();
                }
            }, '-', {
                text: '删除',
                iconCls: 'icon-remove',
                handler: function () {
                    if (!editId) {
                        $.hc.alertErrorMsg('请选择要操作的数据行!');
                        return;
                    }
                    $.hc.confirm("确定要删除选中项吗？", function () {
                        var ids = "";
                        var selectNodes = $("#table").datagrid('getSelections');
                        for (var i = 0; i < selectNodes.length; i++) {
                            if (selectNodes[i]["Id"]) {
                                ids += selectNodes[i]["Id"] + ",";
                            }
                        }
                        $.hc.ajax('LogPostHandler.Delete', {
                            params: { ids: ids },
                            success: function (response) {
                                var item = eval(response)[0];
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
            }, {
                text: '清空',
                iconCls: 'icon-remove',
                handler: function () {

                    $.hc.confirm("删除后将不能还原，系统7天内的日志将保留，确定要清空日志吗？", function () {
                        $.hc.ajax('LogPostHandler.Clear', {
                            params: {},
                            success: function (response) {
                                var item = eval(response)[0];
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
            }, '-',
            {
                text: '刷新',
                iconCls: 'icon-reload',
                handler: function () {
                    reload();
                    editId = 0;
                }
            }];
        function htmlencode(s) {
            var div = document.createElement('div');
            div.appendChild(document.createTextNode(s));
            return div.innerHTML;
        }
        function htmldecode(s) {
            var div = document.createElement('div');
            div.innerHTML = s;
            return div.innerText || div.textContent;
        }  
    </script>
    <table id="table" class="easyui-datagrid" style="height: 370px" data-options="
				title: '',
				url: '<%=BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=LogGetHandler.GetSearchPageData&title=*',
				method: 'get',
                singleSelect:false,
				rownumbers: true, 
				idField: 'Id',
				treeField: 'Title',
                columns:[[ 
                    {field:'ck',checkbox:true},
                    {field:'Title',title:'标题',width:200,align:'left'},
                    {field:'Message',title:'内容',width:400,align:'left',
			                formatter: function(value,row,index){
				                return htmlencode(row.Message);
			         }},
                    {field:'CreateUser',title:'操作者',width:100,align:'center'} ,
                    {field:'UserIP',title:'IP地址',width:100,align:'center'} ,
                    {field:'CreateDate',title:'操作时间',width:120,align:'center',formatter: function(value,row,index){
                            return $.hc.fixJsonDate(row.CreateDate);
                    }} 
                ]],
                toolbar:toolbar,
                pagination:true,
                pageSize:20,
                onClickRow: onClickRow,
                onDblClickRow:onDblClickRow
			">
    </table>
    <div id="details" style="padding: 10px;">
    </div>
</asp:Content>
