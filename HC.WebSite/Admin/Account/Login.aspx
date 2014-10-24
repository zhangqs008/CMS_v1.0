<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="HC.WebSite.Admin.Account.Login" %>

<%@ Import Namespace="HC.Foundation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="<%=BasePath %>Admin/Style/login.css" rel="stylesheet" type="text/css" />
    <table width="100%" height="166" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td height="42" valign="top">
                <table width="100%" height="42" border="0" cellpadding="0" cellspacing="0" class="login_top_bg">
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table width="100%" height="532" border="0" cellpadding="0" cellspacing="0" class="login_bg">
                    <tr>
                        <%--左侧--%>
                        <td width="49%">
                            <table width="100%" class="login_bg2" height="532" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="left_blank">
                                                </td>
                                                <td class="left_txt">
                                                    <span class="top_left" style="display: inline-block; height: 50px; font-size: 24px;
                                                        font-weight: bold; padding: 4px; font-family: 微软雅黑,Helvetica, Tahoma, Arial, STXihei, 华文细黑, Microsoft YaHei,  SimSun, 宋体, Heiti, 黑体, sans-serif;
                                                        color: #275DA3">
                                                        <%= SiteConfig.SiteInfo.SiteName %>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="left_blank">
                                                </td>
                                                <td class="left_txt">
                                                    <p>
                                                        1.简单易用的管理平台，为您企业信息门户建站首选方案。</p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="left_blank">
                                                </td>
                                                <td class="left_txt">
                                                    <p>
                                                        2.强大灵活的模板引擎，让您的设计灵感发挥的淋漓尽致。</p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="left_blank">
                                                </td>
                                                <td class="left_txt">
                                                    <p>
                                                        3.安全稳定的系统后台，让您管理内容易如反掌。</p>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <%--中间--%>
                        <td width="1%">
                            &nbsp;
                        </td>
                        <%--左侧--%>
                        <td width="50%">
                            <table width="100%" height="532" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="right_blank">
                                                </td>
                                                <td>
                                                    <span class="login_txt_bt">管理登陆</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="right_blank">
                                                </td>
                                                <td>
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td width="13%" height="38" class="top_hui_text_left">
                                                                <span class="login_txt">管理员：&nbsp;&nbsp; </span>
                                                            </td>
                                                            <td height="38" colspan="2" class="top_hui_text_right">
                                                                <asp:TextBox runat="server" autocomplete="off" ID="txtName" Width="200" CssClass="input_text" /><asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator1" ControlToValidate="txtName" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="13%" height="35" class="top_hui_text_left">
                                                                <span class="login_txt">密 码： &nbsp;&nbsp; </span>
                                                            </td>
                                                            <td height="35" colspan="2" class="top_hui_text_right">
                                                                <asp:TextBox runat="server" ID="txtPwd" autocomplete="off" Width="200" TextMode="Password"
                                                                    CssClass="input_text" /><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                                                        ControlToValidate="txtPwd" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="13%" height="35" class="top_hui_text_left">
                                                                <span class="login_txt">验证码：</span>
                                                            </td>
                                                            <td height="35" colspan="2" class="top_hui_text_right">
                                                                <asp:TextBox runat="server" ID="txtVerifiyCode" MaxLength="4" autocomplete="off"
                                                                    Width="100" CssClass="input_text" /><asp:RequiredFieldValidator ID="RequiredFieldValidator3"
                                                                        ControlToValidate="txtVerifiyCode" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator><img
                                                                            src="<%= BasePath %>VerifiyCode.aspx" style="vertical-align: middle" width="71"
                                                                            height="23" onclick=" this.src = this.src + '?' " />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="35">
                                                                &nbsp;
                                                            </td>
                                                            <td height="35" colspan="2" class="top_hui_text_right">
                                                                <asp:Button runat="server" ID="btnSave" Text="登录" OnClick="BtnSaveClick" />
                                                                <asp:Button runat="server" Visible="False" ID="btnRegiste" OnClick="BtnRegisteClick"
                                                                    CausesValidation="False" Text="注册" /><asp:Button runat="server" ID="btnCancle" OnClick="BtnCancleClick"
                                                                        CausesValidation="False" Text="重置" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="login-buttom-bg">
                    <tr>
                        <td align="center">
                            <span class="login-buttom-txt"><a style="color: #ABCAD3; text-decoration: none" href="http://www.miitbeian.gov.cn/"
                                target="_blank">粤ICP备14030001号</a>&nbsp;&nbsp;<a style="color: #ABCAD3; text-decoration: none"
                                    href="http://www.qingshanboke.com" target="_blank">Copyright © 清山博客 2014</a></span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
