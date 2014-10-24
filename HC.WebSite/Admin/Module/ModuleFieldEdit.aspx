<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ModuleFieldEdit.aspx.cs" Inherits="HC.WebSite.Admin.Module.ModuleFieldEdit" %>

<%@ Import Namespace="HC.Enumerations.Content" %>
<%@ Import Namespace="HC.Framework.Extension" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="p" class="easyui-panel" title='<%=Module.Name %> <%= RequestString("action").ToLower() == "modify" ? "修改" : "添加" %>字段'
        style="background: #fafafa; height: 500px;">
        <table cellpadding="10" cellspacing="0" width="100%" class="edit_table">
            <tr style="display: none">
                <td colspan="2">
                    <input type="text" id="id" name="Id" data-options="required:true" value="0" class="easyui-textbox"></input>
                    <input type="text" id="ModuleId" name="ModuleId" data-options="required:true" value="<%=Module.Id %>"
                        class="easyui-textbox"></input>
                    <input type="text" id="_action" name="_action" value="ModuleFieldFormHandler" class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    备注名称：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtNote" name="Note" style="width: 150px;" data-options="required:true,missingMessage:' 备注名称不能为空'"
                        class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    字段名称：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="txtName" name="Name" style="width: 150px;" data-options="required:true,missingMessage:' 字段名称不能为空'"
                        class="easyui-textbox"></input>
                    （字段名称只能为英文并无空格）
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    提示信息：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="Tips" name="Tips" style="width: 300px;" data-options="" class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    默认值：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="DefaultValue" name="DefaultValue" style="width: 300px;" data-options=""
                        class="easyui-textbox"></input>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    字段类型：
                </td>
                <td class="panel-header td_right">
                    <select class="easyui-combobox" data-options="onSelect: function (rec) {ShowFiledType(rec.value);}"
                        name="Type" id="Type" style="width: 150px;">
                        <%=ModuleFieldTypes%>
                    </select>
                </td>
            </tr>
            <%--单行文本--%>
            <tr class="FieldConfig" typeid="<%=(int)ModuleFieldType.TextBox %>" style="<%=GetFieldStyle((int)ModuleFieldType.TextBox)%>">
                <td class="panel-header td_left">
                    字段长度：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="TextBoxLength" name="TextBoxLength" value="<%=FieldOption.TextBoxLength %>"
                        style="width: 150px" class="easyui-textbox"></input>
                </td>
            </tr>
            <tr class="FieldConfig" typeid="<%=(int)ModuleFieldType.TextBox %>" style="<%=GetFieldStyle((int)ModuleFieldType.TextBox)%>">
                <td class="panel-header td_left">
                    允许为空：
                </td>
                <td class="panel-header td_right">
                    <input type="radio" value="0" name="TextBoxAllowNull" <%=FieldOption.TextBoxAllowNull?"":"checked=\"checked\"" %> />否
                    <input type="radio" value="1" name="TextBoxAllowNull" <%=FieldOption.TextBoxAllowNull?"checked=\"checked\"":"" %> />是
                </td>
            </tr>
            <tr class="FieldConfig" typeid="<%=(int)ModuleFieldType.TextBox %>" style="<%=GetFieldStyle((int)ModuleFieldType.TextBox)%>">
                <td class="panel-header td_left">
                    验证规则：
                </td>
                <td class="panel-header td_right">
                    <select name="TextBoxRegex" class="easyui-combobox" style="width: 150px;">
                        <option selected="selected" value="">无</option>
                        <option value="Number" <%=String.CompareOrdinal(FieldOption.TextBoxRegex, "Number")==0?" selected='selected' ":"" %>>数字</option>
                        <option value="ZipCode" <%=String.CompareOrdinal(FieldOption.TextBoxRegex, "ZipCode")==0?" selected='selected' ":"" %>>邮政编码</option>
                        <option value="Mobile" <%=String.CompareOrdinal(FieldOption.TextBoxRegex, "Mobile")==0?" selected='selected' ":"" %>>手机号码</option>
                        <option value="CHS" <%=String.CompareOrdinal(FieldOption.TextBoxRegex, "CHS")==0?" selected='selected' ":"" %>>中文</option>
                        <option value="EN" <%=String.CompareOrdinal(FieldOption.TextBoxRegex, "EN")==0?" selected='selected' ":"" %>>英文</option>
                        <option value="URL" <%=String.CompareOrdinal(FieldOption.TextBoxRegex, "URL")==0?" selected='selected' ":"" %>>URL</option>
                        <option value="QQ" <%=String.CompareOrdinal(FieldOption.TextBoxRegex, "QQ")==0?" selected='selected' ":"" %>>QQ</option>
                        <option value="LoginName" <%=String.CompareOrdinal(FieldOption.TextBoxRegex, "LoginName")==0?" selected='selected' ":"" %>>用户名</option>
                        <option value="Pwd" <%=String.CompareOrdinal(FieldOption.TextBoxRegex, "Pwd")==0?" selected='selected' ":"" %>>密码</option>
                        <option value="IDCard" <%=String.CompareOrdinal(FieldOption.TextBoxRegex, "IDCard")==0?" selected='selected' ":"" %>>身份证</option>
                     </select>
                </td>
            </tr>
            <%--多行文本--%>
            <tr class="FieldConfig" typeid="<%=(int)ModuleFieldType.TextArea %>" style="<%=GetFieldStyle((int)ModuleFieldType.TextArea)%>">
                <td class="panel-header td_left">
                    允许为空：
                </td>
                <td class="panel-header td_right">
                    <input type="radio" value="0" name="TextAreaAllowNull" <%=FieldOption.TextAreaAllowNull?"":"checked=\"checked\"" %> />否
                    <input type="radio" value="1" name="TextAreaAllowNull" <%=FieldOption.TextAreaAllowNull?"checked=\"checked\"":"" %> />是
                </td>
            </tr>
            <tr class="FieldConfig" typeid="<%=(int)ModuleFieldType.TextArea %>" style="<%=GetFieldStyle((int)ModuleFieldType.TextArea)%>">
                <td class="panel-header td_left">
                    宽度：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="TextAreaWidth" name="TextAreaWidth" value="<%=FieldOption.TextAreaWidth %>"
                        style="width: 100px" class="easyui-textbox"></input>&nbsp;px
                </td>
            </tr>
            <tr class="FieldConfig" typeid="<%=(int)ModuleFieldType.TextArea %>" style="<%=GetFieldStyle((int)ModuleFieldType.TextArea)%>">
                <td class="panel-header td_left">
                    高度：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="TextAreaHeight" name="TextAreaHeight" value="<%=FieldOption.TextAreaHeight %>"
                        style="width: 100px" class="easyui-textbox"></input>&nbsp;px
                </td>
            </tr>
            <%--单选按钮--%>
            <tr class="FieldConfig" typeid="<%=(int)ModuleFieldType.Radio %>" style="<%=GetFieldStyle((int)ModuleFieldType.Radio)%>">
                <td class="panel-header td_left">
                    选项值：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="RadioValue" name="RadioValue" style="width: 400px" value="<%=FieldOption.RadioValue %>"
                        class="easyui-textbox"></input>
                    多个选项请以"|"隔开；
                </td>
            </tr>
            <tr class="FieldConfig" typeid="<%=(int)ModuleFieldType.Radio %>" style="<%=GetFieldStyle((int)ModuleFieldType.Radio)%>">
                <td class="panel-header td_left">
                    选项文本：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="RadioText" name="RadioText" style="width: 400px" value="<%=FieldOption.RadioText %>"
                        class="easyui-textbox"></input>
                    多个选项请以"|"隔开；
                </td>
            </tr>
            <%--复选框--%>
            <tr class="FieldConfig" typeid="<%=(int)ModuleFieldType.CheckBox %>" style="<%=GetFieldStyle((int)ModuleFieldType.CheckBox)%>">
                <td class="panel-header td_left">
                    选项值：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="CheckBoxValue" name="CheckBoxValue" value="<%=FieldOption.CheckBoxValue %>"
                        style="width: 400px" class="easyui-textbox"></input>
                    多个选项请以"|"隔开；
                </td>
            </tr>
            <tr class="FieldConfig" typeid="<%=(int)ModuleFieldType.CheckBox %>" style="<%=GetFieldStyle((int)ModuleFieldType.CheckBox)%>">
                <td class="panel-header td_left">
                    选项文本：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="CheckBoxText" name="CheckBoxText" value="<%=FieldOption.CheckBoxText %>"
                        style="width: 400px" class="easyui-textbox"></input>
                    多个选项请以"|"隔开；
                </td>
            </tr>
            <%--编辑器--%>
            <tr class="FieldConfig" typeid="<%=(int)ModuleFieldType.Editer %>" style="<%=GetFieldStyle((int)ModuleFieldType.Editer)%>">
                <td class="panel-header td_left">
                    宽度：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="EditerWidth" name="EditerWidth" value="<%=FieldOption.EditerWidth %>"
                        style="width: 100px" class="easyui-textbox"></input>&nbsp;px
                </td>
            </tr>
            <tr class="FieldConfig" typeid="<%=(int)ModuleFieldType.Editer %>" style="<%=GetFieldStyle((int)ModuleFieldType.Editer)%>">
                <td class="panel-header td_left">
                    高度：
                </td>
                <td class="panel-header td_right">
                    <input type="text" id="EditerHeight" name="EditerHeight" value="<%=FieldOption.EditerHeight %>"
                        style="width: 100px" class="easyui-textbox"></input>&nbsp;px
                </td>
            </tr>
            <tr class="FieldConfig" typeid="<%=(int)ModuleFieldType.Editer %>" style="<%=GetFieldStyle((int)ModuleFieldType.Editer)%>">
                <td class="panel-header td_left">
                    模式：
                </td>
                <td class="panel-header td_right">
                    <select id="cc" class="easyui-combobox" name="EditerStyle" style="width: 100px;">
                        <option value="default" <%=FieldOption.EditerStyle.ToStr().ToLower()=="default"?"selected='selected'":"" %>>
                            默认模式</option>
                        <option value="simple" <%=FieldOption.EditerStyle.ToStr().ToLower()=="simple"?"selected='selected'":"" %>>
                            精简模式</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="panel-header td_left">
                    &nbsp;
                </td>
                <td class="panel-header td_right">
                    <div class="button_submit_div">
                        <a href="javascript:void(0)" id="btnsave" class="easyui-linkbutton" data-options="iconCls:'icon-save'">
                            保存</a> <a href="javascript:void(0)" id="btnClose" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'"
                                onclick=" top.closeTab() ">关闭</a>
                    </div>
                </td>
            </tr>
        </table>
        <%--初始化--%>
        <script type="text/javascript">
            var action = '<%= RequestString("action").ToLower() %>';
            $(function () {
                if (action == "modify") {
                    init();
                } else {
                    $(".FieldConfig[typeid='<%=(int)ModuleFieldType.TextBox %>']").show();
                }
            });

            function init() {
                $('#aspnetForm').form('load', {
                    Id: "<%= Field.Id %>",
                    ModuleId: "<%= Field.ModuleId%>",
                    Name: "<%= Field.Name %>",
                    Note: "<%= Field.Note %>",
                    Tips: "<%= Field.Tips %>",
                    DefaultValue: "<%= Field.DefaultValue %>",
                    Type: "<%= Field.Type %>"
                });
            }

            //显示字段配置
            function ShowFiledType(type) {
                $(".FieldConfig").hide();
                $(".FieldConfig[typeid=" + type + "]").show();
            }

        </script>
        <%--保存按钮--%>
        <script type="text/javascript">
            $(function () {
                $("#btnsave").click(function () {
                    $('#aspnetForm').form('submit', {
                        url: "<%= BasePath %>AjaxForm.aspx",
                        onSubmit: function (param) {
                            return $(this).form('enableValidation').form('validate');
                        },
                        success: function (response) {
                            if (response.toLocaleLowerCase() != "true") {
                                $.hc.alertErrorMsg(response);
                            } else {
                                //保存字段配置 
                                $.hc.confirm("操作成功!<br/><br/>关闭当前窗口？<br/><br/>", function () { top.closeTab(); });
                            }
                        }
                    });
                });
            })
        </script>
    </div>
</asp:Content>
