<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Elsevier.Web.VWB.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		<asp:Login ID="LoginControl" runat="server" OnLoggingIn="AuthenticateSitecoreUser" DisplayRememberMe="false">
		</asp:Login>
    </div>
    </form>
</body>
</html>
