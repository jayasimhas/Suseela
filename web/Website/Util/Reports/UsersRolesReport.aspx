<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsersRolesReport.aspx.cs" Inherits="Informa.Web.Util.Reports.UsersRolesReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../DCD/styles/style.css" rel="stylesheet" />
    <style>
        #content table.data th a {
            color: White;
        }

        #content table.data th.sortup a {
            background: url("/img/arrow_sort_up_active.png") no-repeat right center;
            padding-right: 15px;
        }

        #content table.data th.sortdown a {
            background: url("/img/arrow_sort_down_active.png") no-repeat right center;
            padding-right: 15px;
        }

        #content table.data.clickable tr:hover td {
            background: #F4BC50;
            text-decoration: underline;
            cursor: pointer;
        }
    </style>
    <title></title>

</head>
<body class="fullpage">
    <form id="form2" runat="server">
        <div>
            <div id="wrapper">
                <div id="innerwrapper">
                    <!-- Header -->
                    <div id="header">
                        <!-- Logo -->

                    </div>
                    <!-- End header -->
                    <div id="contentwrapper">
                        <div id="breadcrumb">
                        </div>
                        <div id="innercontentwrapper">
                            <div id="content">
                                <h2 class="title">Users/Roles Report</h2>
                                <div>
                                    <asp:DataGrid runat="server" AutoGenerateColumns="true" ID="grdUsersRoles"></asp:DataGrid>
                                </div>
                                <asp:LinkButton runat="server" ID="lnkExport" Text="Download as Excel" OnClick="lnkExport_Click"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
