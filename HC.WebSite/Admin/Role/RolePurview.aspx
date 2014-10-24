<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
         CodeBehind="RolePurview.aspx.cs" Inherits="HC.WebSite.Admin.Role.RolePurview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="easyui-layout">
        <table id="table" title="菜单权限" style="height: 500px" class="easyui-treegrid">
        </table>
    </div>
    <script type="text/javascript">

        $(function() {
            $('#table').treegrid({
                title: '编辑 <span style="color:red"><%= Name %></span> 菜单权限',
                url: '<%= BasePath %>ajaxget.aspx?_t='+new Date().getTime()+'&_action=MenuGetHandler.InitRoleMenuTree&RoleId=<%= RequestInt32("Id") %>',
                method: 'get',
                rownumbers: true,
                idField: 'id',
                treeField: 'text',
                singleSelect: false,
                loadMsg: "数据加载中，请稍后……",
                columns: [[
                    { field: 'ck', checkbox: true },
                    {
                        field: 'text',
                        title: '名称',
                        width: 220,
                        align: 'left',
                        formatter: function(value, rec) {
                            var target = $("#table");
                            if (rec.enable == "true") {
                                $(target).treegrid('select', rec.id);
                                return rec.text;
                            } else {
                                return rec.text;
                            }
                        }
                    }
                ]],
                onClickRow: function(row) {
                    var target = $("#table");
                    var id = row.id;
                    var idField = "id";
                    var status = false; //用来标记当前节点的状态，true:勾选，false:未勾选  
                    var selectNodes = $(target).treegrid('getSelections'); //获取当前选中项  
                    for (var i = 0; i < selectNodes.length; i++) {
                        if (selectNodes[i][idField] == id)
                            status = true;
                    }
                    if (status) {
                        selectParent(target, id, idField, status);
                        selectChildren(target, id, idField, true, status);
                    } else {
                        cancleSelectChildren(target, id, idField, true, status);
                    }
                },
                toolbar: [
                    {
                        text: '保存',
                        iconCls: 'icon-add',
                        handler: function() {
                            var target = $("#table");
                            var items = "";
                            var selectNodes = $(target).treegrid('getSelections'); //获取当前选中项  
                            for (var i = 0; i < selectNodes.length; i++) {
                                items += selectNodes[i]["id"] + ",";
                            }
                            $.hc.confirm("确定要保存角色菜单权限吗？", function() {
                                $.hc.ajax('RolePostHandler.SetRoleMenuPurview', {
                                    params: { roleId: <%= RequestInt32("id") %>, purview: items },
                                    success: function(itemResponse) {
                                        var item = eval(itemResponse)[0];
                                        if (item.status.toLocaleLowerCase() == "true") {
                                            $('#table').treegrid('reload');
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
                            $('#table').treegrid('reload');
                        }
                    }
                ]
            });

        });

        /** 
        * 级联选择父节点 
        * @param {Object} target 
        * @param {Object} id 节点ID 
        * @param {Object} status 节点状态，true:勾选，false:未勾选 
        * @return {TypeName}  
        */

        function selectParent(target, id, idField, status) {
            var parent = $(target).treegrid('getParent', id);
            if (parent) {
                var parentId = parent[idField];
                $(target).treegrid('select', parentId);
                selectParent(target, parentId, idField, status);
            }
        }

        /** 
        * 级联选择子节点 
        * @param {Object} target 
        * @param {Object} id 节点ID 
        * @param {Object} deepCascade 是否深度级联 
        * @param {Object} status 节点状态，true:勾选，false:未勾选 
        * @return {TypeName}  
        */

        function selectChildren(target, id, idField, deepCascade, status) {
            //深度级联时先展开节点  
            if (!status && deepCascade)
                $(target).treegrid('expand', id);
            //根据ID获取下层孩子节点  
            var children = $(target).treegrid('getChildren', id);
            for (var i = 0; i < children.length; i++) {
                var childId = children[i][idField];
                $(target).treegrid('select', childId);
                selectChildren(target, childId, idField, deepCascade, status); //递归选择子节点  
            }
        }

        /** 
        * 级联取消选择子节点 
        * @param {Object} target 
        * @param {Object} id 节点ID 
        * @param {Object} deepCascade 是否深度级联 
        * @param {Object} status 节点状态，true:勾选，false:未勾选 
        * @return {TypeName}  
        */

        function cancleSelectChildren(target, id, idField, deepCascade, status) {
            //深度级联时先展开节点  
            if (!status && deepCascade)
                $(target).treegrid('expand', id);
            //根据ID获取下层孩子节点  
            var children = $(target).treegrid('getChildren', id);
            for (var i = 0; i < children.length; i++) {
                var childId = children[i][idField];
                $(target).treegrid('unselect', childId);
                cancleSelectChildren(target, childId, idField, deepCascade, status); //递归选择子节点  
            }
        }
    </script>
</asp:Content>