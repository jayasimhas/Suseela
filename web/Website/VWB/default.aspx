<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Elsevier.Web.VWB._default" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/VWB/Scripts/jquery-ui/jquery.ui.timepicker.js"></script>

    <link rel="stylesheet" type="text/css" href="/VWB/Scripts/jquery-ui/jquery-ui-1.10.4.custom.css" />
    <link rel="stylesheet" type="text/css" href="/VWB/Styles/vwb.css" />
    <link rel="stylesheet" type="text/css" href="/VWB/Scripts/jquery-ui/jquery.ui.timepicker.css" />
    <title>Informa Business Information - Virtual Whiteboard</title>

</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="IssueTitleInput"></asp:HiddenField>
        <asp:HiddenField runat="server" ID="IssuePublishedDateInput"></asp:HiddenField>
        <asp:HiddenField runat="server" ID="IssueArticleIdsInput"></asp:HiddenField>
        <!-- pipe bar separated guids -->
        <asp:Button runat="server" ID="NewIssueSubmitButton" OnClick="NewIssueSubmitButton_OnClick" CssClass="hidden-button" />

        <div class="wrapper">
            <asp:Image ID="imgLogo" CssClass="banner" AlternateText="Informa Business Information - Virtual Whiteboard" runat="server" />
            <div class="top">
                <asp:DropDownCheckBoxes ID="ddlPublications" runat="server" Width="200px" UseSelectAllNode="true" AddJQueryReference="false">
                    <style selectboxwidth="195" dropdownboxboxwidth="160" dropdownboxboxheight="250" />
                    <texts selectboxcaption="Select Publication(s)" />
                </asp:DropDownCheckBoxes>
                <div id="dateRangeWrapper">
                    <div class="left radioButtonWrapper">
                        <asp:RadioButton ID="rbNoDate" runat="server" Text="Default" GroupName="choice" class="enabledate" ToolTip="Displays articles that have been scheduled to be published 1 day back, 30 days ahead"/>
                         <label style="color: red; font-size: 8pt">*</label>	
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
                        <asp:Label runat="server" ID="lblMsg"></asp:Label>
                        <br />
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
                <label style="color: red; font-size: 8pt">* Displays articles that have been scheduled to be published 1 day back, 30 days ahead.</label>	
               <br />
                <asp:Table ID="tblResults" runat="server" border="1">
                </asp:Table>

                <div class="vwb-report-options">

                    <asp:DropDownList ID="ddColumns" runat="server"
                        AutoPostBack="true" OnSelectedIndexChanged="AddColumn" />

                    <hr />

                    <button disabled class="orange-button js-create-new-issue">Add Selected Articles to New Issue</button>

                    <hr />

                    <asp:DropDownList runat="server" ID="ExistingIssueSelector" AutoPostBack="False" />

                    <br />
                    <br />

                    <button disabled class="orange-button js-go-to-issue">Go To Issue</button>

                    <br />
                    <br />

                    <asp:Button runat="server" ID="btnAddArticleToExistingIssue" Text="Add Selected Articles to Existing Issue" OnClick="btnAddArticleToExistingIssue_OnClick" CssClass="orange-button js-append-to-issue" />
                </div>

            </div>
        </div>
    </form>

    <!-- NEW ISSUE MODAL -->
    <div class="new-issue-modal js-new-issue-modal" title="Dialog Title">
        <label>Issue Title</label>
        <p class="title-error js-form-error">
            Issue Title is required
        </p>
        <input type="text" class="js-new-issue-title" />
        <label>Published Date</label>
        <p class="date-error js-form-error">
            Published Date is required
        </p>
        <input class="js-new-issue-pub-date" type="text" />
        <button class="orange-button js-submit-new-issue">Create New Issue</button>
    </div>

    <script type="text/javascript" src="/VWB/Scripts/selected-articles.js"></script>

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
                $("#ddIssue").attr("disabled", null);
            });

            $('#rbDateRange').click(function () {
                $("#txtStart").attr("disabled", null);
                $("#txtEnd").attr("disabled", null);
                $("#txtStartTime").attr("disabled", null);
                $("#txtEndTime").attr("disabled", null);
                $("#ddIssue").attr("disabled", "disabled");
            });
        });

    </script>
    <script type="text/javascript">

        function UpdateEditorialNote(itemID) {

            var text = $("body").find('textarea[itemID="' + itemID + '"]').val();
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
