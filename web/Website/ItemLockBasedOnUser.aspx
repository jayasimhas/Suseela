<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemLockBasedOnUser.aspx.cs" Inherits="Informa.Web.ItemLockBasedOnUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">            
        <fieldset id="fieldItemLock">
            <legend> <H2>Lock Items IN Sitecore Based On User</H2> </legend>
            <center>
                <div>
                User Name: <asp:TextBox runat="server" ID="txtUserName"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorUsertextBox" runat="server" ControlToValidate="txtUserName" ErrorMessage="Please Enter User Name" ForeColor="Red"></asp:RequiredFieldValidator>
&nbsp;AND&nbsp; Email ID :&nbsp;<asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
              <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
              ErrorMessage="Please Enter Email ID" ControlToValidate="txtEmail"
                  ForeColor="Red"></asp:RequiredFieldValidator>
              <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
              runat="server" ErrorMessage="Please Enter Valid Email ID"
                  ControlToValidate="txtEmail"
                  CssClass="requiredFieldValidateStyle"
                  ForeColor="Red"
                  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                  </asp:RegularExpressionValidator>
                    </div>
                <br />
                <div>
                    <asp:Button runat="server" ID="btnLockArticle" Text="Item Lock" OnClick="btnLockArticle_Click"/>
                </div>
                <div>
                    <asp:Label runat="server" ID="lblLockedItemsCount"></asp:Label>
                </div>
                <div>
                    <asp:Label runat="server" ID="lblLockedItems"></asp:Label>
                </div>
            </center>                
        </fieldset>
    </form>
</body>
</html>
