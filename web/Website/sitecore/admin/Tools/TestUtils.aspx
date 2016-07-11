<%@ Page language="c#" Codebehind="TestUtils.aspx.cs" AutoEventWireup="false" Inherits="Informa.Web.sitecore.admin.Tools.TestUtils" %>
<!DOCTYPE html>
<HTML>
  <HEAD>
    <title>Test Utils</title>
  </HEAD>
  <body>
      <div>
          <form runat="server">
              
              <h2>DCD Inspector</h2>
              <asp:Button runat="server" UseSubmitBehavior="True" Text="Get Companies" OnClick="GetCompaniesClick"/>
              <asp:TextBox runat="server" ID="DCDOutput" TextMode="MultiLine"></asp:TextBox>
              
              <hr />

              <h2>Crypto Tester</h2>
              <div>Input: <asp:TextBox runat="server" ID="InputTxt"></asp:TextBox></div>
              <div>Cypher: <asp:TextBox runat="server" ID="OutputTxt"></asp:TextBox></div>
              <div>Key: <asp:TextBox runat="server" ID="KeyTxt"></asp:TextBox></div>
              <div>
                  <asp:Button runat="server" UseSubmitBehavior="True" Text="Encrypt" OnClick="EncryptClick"/>
                  <asp:Button runat="server" UseSubmitBehavior="True" Text="Decrypt" OnClick="DecryptClick"/>
              </div>
              
          </form>
      </div>
  </body>
</HTML>
