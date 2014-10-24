<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataInit.aspx.cs" Inherits="HC.WebSite.DataInit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        用户名：<asp:TextBox ID="txtUserName" runat="server" Width="500" Text="'admin', 'zhangqs008'" />
        <asp:Button ID="Button2" runat="server" Text="设定用户权限" OnClick="ButtonSetUserPurview" />
    </div>
    </form>
</body>
</html>
