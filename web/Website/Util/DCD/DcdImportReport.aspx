<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DcdImportReport.aspx.cs" Inherits="Informa.Views.Util.DCD.DcdImportReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Deals, Companies, and Drugs Import Report</title>

    <link href="styles/style.css" rel="stylesheet"  media="screen"/>
    <link href="styles/jquery-ui-1.8.11.custom.css" rel="stylesheet" />
    <link href="styles/style.css" rel="stylesheet" />
    <script src="js/jquery.js"></script>
    <script src="js/jquery-ui.js"></script>
    <script src="js/jquery.multiSelect.js"></script>
    <script src="js/scripts.js"></script>

	<style>
		#content table.data th a
		{
			color: White;
		}
		#content table.data th.sortup a
		{
			background: url("/img/arrow_sort_up_active.png") no-repeat right center;
			padding-right: 15px;
		}
		#content table.data th.sortdown a
		{
			background: url("/img/arrow_sort_down_active.png") no-repeat right center;
			padding-right: 15px;
		}
		#content table.data.clickable tr:hover td
		{
			background: #F4BC50;
			text-decoration: underline;
			cursor: pointer;
		}
	</style>
</head>
<body class="fullpage">
    <form id="form1" runat="server">
        <div>
            <div id="wrapper">
                <div id="innerwrapper">
                    <!-- Header -->
                    <div id="header">
                        <!-- Logo -->
                        <a class="site-logo" href="#">
                            <img src="/util/DCD/img/header_logo.png" /></a>
                    </div>
                    <!-- End header -->
                    <div id="contentwrapper">
                        <div id="breadcrumb">
                        </div>
                        <div id="innercontentwrapper">
                            <div id="content">
                                <h2 class="title">Deals, Companies, and Drugs Report Builder</h2>
                                <asp:PlaceHolder ID="phImports" runat="server">
                                    <div class="date-fields">
                                        <label style="padding-right: 20px;">
                                            <span class="highlight">Start Date:</span>
                                            <asp:TextBox ID="txtDateStart" runat="server" CssClass="date-start" size="10" MaxLength="10" />
                                        </label>
                                        <label>
                                            <span class="highlight">End Date:</span>
                                            <asp:TextBox ID="txtDateEnd" runat="server" CssClass="date-end" size="10" MaxLength="10" />
                                        </label>
                                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" CssClass="button" />
                                    </div>

                                    <script type="text/javascript">
                                        jQuery(document).ready(function ($) {
                                            $(".date-start, .date-end").datepicker({
                                                changeMonth: true,
                                                changeYear: true,
                                                showOtherMonths: true,
                                                selectOtherMonths: true,
                                                maxDate: "+0",
                                                dayNamesMin: ['S', 'M', 'T', 'W', 'T', 'F', 'S']
                                            });
                                        });
                                    </script>

                                    <br />
                                    <asp:GridView ID="dgResults" runat="server" CssClass="data clickable" AllowSorting="true" OnSorting="dgResults_Sorting" OnRowDataBound="dgResults_rowDatabound">
                                    </asp:GridView>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="phSingleImport" runat="server" Visible="false">
                                    <asp:HyperLink ID="hlBack" runat="server">« BACK</asp:HyperLink>

                                    <h3>
                                        <asp:Literal ID="litSingleImportTitle" runat="server" /></h3>
                                    <asp:GridView ID="dgSingleImport" runat="server" CssClass="data" AllowSorting="true" OnSorting="dgResults_Sorting" OnRowDataBound="dgSingleImport_rowDatabound">
                                    </asp:GridView>
                                </asp:PlaceHolder>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
