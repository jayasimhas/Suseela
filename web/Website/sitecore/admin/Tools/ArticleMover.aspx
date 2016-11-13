<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticleMover.aspx.cs" Inherits="Informa.Web.sitecore.admin.Tools.ArticleMover" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #main_container {
            margin: 0 auto;
            width: 900px;
        }
    </style>
</head>
<body>
    <div id="main_container">
        <form runat="server">
            <h2>Article Moving Utility </h2>
            <%--<label>This tool helps you move artilces between publications</label>
            <hr />--%>
            <%--<asp:RadioButton runat="server" Text="Move by Article IDs" ID="rdMoveByArticleIDs" GroupName="SourceArticles" Checked="true" />--%>
            <label>Source ID(s):</label>
            <br />
            <asp:RadioButton runat="server" Checked="true" GroupName="IDType" Text="Item ID(s) e.g. {459D11DE-8BF5-4D4A-8D90-1F225903E3B4}" ID="rdIsItemIDs" />
            <asp:RadioButton runat="server" GroupName="IDType" Text="Article ID(s) e.g. SC000012" ID="rdIsArticleIDs" />
            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtIDs"></asp:TextBox>
            <br />
            <label style="color: red; font-size: 8pt">Use Vertical bar (|) for multiples</label>
            <hr />
            <asp:RadioButton runat="server" Text="Preserve date folders hierarchy under the main Articles folder ID" ID="rdPreserveDateFoldersHier" GroupName="DestinationLocation" Checked="true" />
            <asp:TextBox runat="server" ID="txtDestinationArticlesFolder"></asp:TextBox>
            <br />
            <br />
            <asp:RadioButton runat="server" Text="Move to the following folder ID" ID="rdMoveToFolderID" GroupName="DestinationLocation" />
            <asp:TextBox runat="server" Width="150px" ID="txtToFolderID"></asp:TextBox>
            <hr />
            <asp:CheckBox runat="server" Text="Clear Taxonomy Fields" ID="chkClearTaxonomy" />
            <br />
            <asp:CheckBox runat="server" Text="Add Taxonomy Fields" ID="chkNewTaxonomyFields" />
            <asp:TextBox runat="server" Width="60%" ID="txtNewTaxonomyFields"></asp:TextBox>
            <label style="color: red; font-size: 8pt">Use Vertical bar (|) for multiples</label>
            <br />
            <asp:CheckBox runat="server" ID="chkPublishDestination" Text="Publish Destination Items" />
            <hr />
            <asp:Button runat="server" Text="Start" ID="bntStart" OnClick="bntStart_Click" />
            <asp:Button runat="server" Text="Reset" ID="btnReset" OnClick="btnReset_Click" />
            <br />
            <asp:Label runat="server" ForeColor="Red" ID="lblError"></asp:Label>
        </form>
    </div>
</body>
</html>
