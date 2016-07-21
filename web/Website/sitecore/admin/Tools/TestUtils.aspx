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
              <h4>Instructions</h4>
              <p>
                  Enter a Record Id OR a Record Number of a Company or Deal.  The Id is the numerical database identifier.
                  The record number is the value seen in the URL when searching for a company or deal on 
                  www.pharmamedtechbi.com.
                  For example, the deal found here: https://www.pharmamedtechbi.com/deals/200130604
                  Has a record number of 200130604, but the record id is totally different (25128).
                  Once an id or number is entered, click Get Company or Get Deal.
                  Results should display in the box below.
              </p>
              
              <p>
                  Note: This is a hastily thrown together test utility.  There is no guarantee that it will function.
              </p>

              <asp:Label runat="server" AssociatedControlID="DCDInput">Record Id</asp:Label>
              <asp:TextBox runat="server" ID="DCDInput"></asp:TextBox>
              <br/>
              <asp:Label runat="server" AssociatedControlID="DCDInputRecordNumber">Record Number</asp:Label>
              <asp:TextBox runat="server" ID="DCDInputRecordNumber"></asp:TextBox>
              <br/>
              <asp:Button runat="server" UseSubmitBehavior="True" Text="Get Company" OnClick="GetCompanyClick"/>
              <asp:Button runat="server" UseSubmitBehavior="True" Text="Get Deal" OnClick="GetDealClick"/>
              <br/>
              <asp:TextBox runat="server" ID="DCDOutput" TextMode="MultiLine" Height="400" Width="650"></asp:TextBox>
              
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
