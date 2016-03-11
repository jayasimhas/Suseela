<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewDCDXML.aspx.cs" Inherits="Informa.Web.Util.DCD.ViewDCDXML" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
			Company Number: <asp:TextBox runat="server" ID="txtCompanyNumber"></asp:TextBox> <br/><br/>
			Deal Number: <asp:TextBox runat="server" ID="txtDealNumber"></asp:TextBox>
			<asp:Button runat="server" ID="btnGo" Text="GO" OnClick="btnGo_Click"/>
    </div>
    </form>
</body>
</html>
