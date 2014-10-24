<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
         CodeBehind="Register.aspx.cs" Inherits="HC.WebSite.Admin.Account.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .editTable {
            border: 1px solid lavender;
            margin-left: auto;
            margin-right: auto;
            margin-top: 100px;
            padding: 10px;
            width: 400px;
        }

        .td_left {
            text-align: right;
            width: 30%;
        }

        .input_text {
            border: 1px solid lavender;
            height: 22px;
            padding-left: 6px;
        }

        th {
            background: #0092DC;
            color: white;
            height: 25px;
        }

        td { height: 25px; }

        .td_btns { text-align: center; }
    </style>
    <table class="editTable">
        <tr>
            <th colspan="2">
                用户注册
            </th>
        </tr>
        <tr>
            <td class="td_left">
                用户名：
            </td>
            <td class="td_right">
                <asp:TextBox runat="server" Width="200" autocomplete="off" ID="txtUserName" CssClass="input_text" /><asp:RequiredFieldValidator
                                                                                                                        ID="RequiredFieldValidator1" ControlToValidate="txtUserName" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="td_left">
                邮 箱：
            </td>
            <td class="td_right">
                <asp:TextBox runat="server" Width="200" autocomplete="off" ID="txtEmail" CssClass="input_text" /><asp:RequiredFieldValidator
                                                                                                                     ID="RequiredFieldValidator2" ControlToValidate="txtEmail" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="td_left">
                密 码：
            </td>
            <td class="td_right">
                <asp:TextBox runat="server" Width="200" TextMode="Password" autocomplete="off" ID="txtPassword"
                             CssClass="input_text" /><asp:RequiredFieldValidator ID="RequiredFieldValidator3"
                                                                                 ControlToValidate="txtPassword" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="td_left">
                验证码：
            </td>
            <td class="td_right">
                <asp:TextBox runat="server" ID="txtVerifiyCode" MaxLength="4" autocomplete="off"
                             Width="100" CssClass="input_text" /><asp:RequiredFieldValidator ID="RequiredFieldValidator4"
                                                                                             ControlToValidate="txtVerifiyCode" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                <img src="<%= BasePath %>VerifiyCode.aspx" style="vertical-align: middle" width="71"
                     height="23" onclick=" this.src = this.src + '?' " />
            </td>
        </tr>
        <tr class="tr_btns">
            <td class="td_btns" colspan="2">
                <asp:Button runat="server" ID="btnSave" Text="注册" OnClick="BtnSaveClick" />
                <asp:Button runat="server" ID="btnCancle" OnClick="BtnCancleClick" CausesValidation="False"
                            Text="返回" />
            </td>
        </tr>
    </table>
</asp:Content>