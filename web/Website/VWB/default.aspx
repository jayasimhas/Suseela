<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Elsevier.Web.VWB._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <script type="text/javascript" src="/VWB/Scripts/jquery.js"></script>
    <script type="text/javascript" src="/VWB/Scripts/jquery-ui/jquery-ui.js"></script>
    <script type="text/javascript" src="/VWB/Scripts/jquery-ui/jquery.ui.timepicker.js"></script>
    
    <link rel="stylesheet" type="text/css" href="/VWB//Scripts/jquery-ui/jquery-ui-1.8.11.custom.css" />
    <link rel="stylesheet" type="text/css" href="/VWB//Styles/vwb.css" />
    <link rel="stylesheet" type="text/css" href="/VWB//Scripts/jquery-ui/jquery.ui.timepicker.css" />
    <title><%= Elsevier.Library.Reference.Constants.BusinessName %> - Virtual Whiteboard</title>

</head>
<body>
    <form id="form1" runat="server">
        <div class="wrapper">
            <img class="banner" width="317" height="122" src="<%= LogoUrl %>"
                alt="<%= Elsevier.Library.Reference.Constants.BusinessName %> - Virtual Whiteboard">
            <div class="top">
                <asp:CheckBoxList runat="server" ID="chkPublications" Width="250px"></asp:CheckBoxList>
                <div id="dateRangeWrapper">
                    <div class="left radioButtonWrapper">
                        <asp:RadioButton ID="rbNoDate" runat="server" Text="Default" GroupName="choice" class="enabledate" />
                    </div>
                    <div class="left radioButtonWrapper">
                        <asp:RadioButton ID="rbDateRange" runat="server" Text="Date Range" GroupName="choice" class="enabledate" />
                    </div>
                    <div class="left inputWrapper">
                        <div class="left dateRangeLabel">From</div>
                        <asp:TextBox ID="txtStart" runat="server" class="date" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txtStartTime" runat="server" class="time" Enabled="false"></asp:TextBox>
                        <br />
                        <div class="left dateRangeLabel">To</div>
                        <asp:TextBox ID="txtEnd" runat="server" class="date" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txtEndTime" runat="server" class="time" Enabled="false"></asp:TextBox>
                        <br />
                        <asp:CheckBox runat="server" ID="chkShowInProgressArticles" Text="Show in-progress articles only" />
                    </div>


                    <div class="appbuttons">
                        <asp:Button ID="btnRunReport" runat="server" Text="Run Report" OnClick="RunReport" OnClientClick="$('#hidPubs').val($('#ddlP').val())" />
                        &nbsp;<asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="ResetReport" />
                        &nbsp;<asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="Logout" />
                    </div>
                </div>

            </div>
            <br />
            <br />
            <br />
            <div class="report">
                <asp:Table ID="tblResults" runat="server" border="1">
                </asp:Table>
                <asp:DropDownList ID="ddColumns" runat="server"
                    AutoPostBack="true" OnSelectedIndexChanged="AddColumn">
                </asp:DropDownList>
            </div>
        </div>
    </form>
    <script>
        jQuery(function ($) {
            $('#txtStartTime, #txtEndTime').timepicker({
                showPeriod: true,
                showLeadingZero: false
            });

            $('#txtEnd').datepicker();
            $('#txtStart').datepicker();
            $('#rbIssue').click(function () {
                $("#txtStart").attr("disabled", "disabled");
                $("#txtEnd").attr("disabled", "disabled");
                $("#txtStartTime").attr("disabled", "disabled");
                $("#txtEndTime").attr("disabled", "disabled");
                $("#ddIssue").attr("disabled", "");
            });

            $('#rbDateRange').click(function () {
                $("#txtStart").attr("disabled", "");
                $("#txtEnd").attr("disabled", "");
                $("#txtStartTime").attr("disabled", "");
                $("#txtEndTime").attr("disabled", "");
                $("#ddIssue").attr("disabled", "disabled");
            });
        });

    </script>
    <script type="text/javascript">

        function UpdateEditorialNote(itemID) {

            var text = $("body").find('textarea[itemID=' + itemID + ']').val();
            $.ajax({
                type: "POST",
                url: "/VWB/services/virtualwhiteboard.asmx/UpdateEditorialNotes",
                data: "{'itemID':'" + itemID + "', 'text':'" + text + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    location.reload(true);
                }
                //FOR DEBUGGING ONLY
                //				, 
                //				error: function (jqXHR, textStatus, errorThrown) {
                //					// The .NET error and stacktrace is hidden 
                //					// inside the XMLHttpRequest response
                //					if ($.isFunction(onFail))
                //						onFail($.parseJSON(jqXHR.response));
                //				}
            });
        }

        //		function onFail(data) {
        //			alert(data.Message + "\n" + data.StackTrace);
        //		}
    </script>
</body>
</html>
