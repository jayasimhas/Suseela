<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FreeTrialArticleSearch.aspx.cs" Inherits="Informa.Web.sitecore_modules.Shell.Informa.FreeTrialArticleSearch" %>
<%@ Import Namespace="Sitecore.Data.Items" %>
<%@ Import Namespace="Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Free Trial Article Search</title>
	<link href="css/global.css" rel="stylesheet" type="text/css"/>
    <style type="text/css">
        * { font: bold 8pt tahoma; color: #555555; }
        .clear { clear:both; }
        body { background:#F0F1F2; margin:0px; padding:0px; }
	        h2 { background: none repeat scroll 0 0 #D9D9D9; border-bottom: 1px solid #CCCCCC;
                border-top: 1px solid white; padding: 1px 2px; margin:0px;
		    }    
            .Section { width:100%; clear:both; float:left; font: 8pt tahoma; margin-top:15px; }
		        .formRow { margin:0 0 10px 0; border-left: 4px solid #CCCCCC; padding-left:8px;}
                    a { color:#dc291e; font-weight:normal; text-decoration:none; }
                    a:hover { text-decoration:underline; }
                    .review-entry { margin-bottom:5px; }
    </style>
    <script src="/sitecore/shell/controls/lib/prototype/prototype.js" type="text/javascript"></script>
    <script src="/sitecore/shell/controls/Browser.js" type="text/javascript"></script>
    <script src="/sitecore/shell/controls/Sitecore.js" type="text/javascript"></script>
    <script src="/sitecore/shell/controls/SitecoreObjects.js" type=""></script>
    <script src="/sitecore/shell/controls/SitecoreWindowManager.js" type=""></script>
    <script src="/sitecore/shell/controls/lib/scriptaculous/scriptaculous.js?load=effects" type=""></script>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Articles - Free with Registration</h2>
        <div class="Section">
            <div class="formRow">
                <asp:Literal ID="ltlResultCount" runat="server"></asp:Literal>
            </div>
            <div class="formRow">
                <asp:Repeater ID="rptFreeTrialArticles" runat="server">
                    <ItemTemplate>
                        <div class="review-entry entry-<%# (Container.ItemIndex % 2 == 0) ? "odd" : "even" %>">
                            <%# Container.ItemIndex + 1 %>). <a onclick="javascript:scForm.getParentForm().postRequest('','','','item:load(id=<%# ((IArticle)Container.DataItem)._Id.ToString("B") %>)'); return false;" href="#">
                                <%# ((IArticle)Container.DataItem)._Name %>
                            </a>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>
</body>
</html>
