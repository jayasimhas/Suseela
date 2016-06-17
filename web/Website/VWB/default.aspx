<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Elsevier.Web.VWB._default"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
	<script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/jquery-ui.min.js"></script>
	<script type="text/javascript" src="/VWB/Scripts/jquery-ui/jquery.ui.timepicker.js"></script>

	<link rel="stylesheet" type="text/css" href="/VWB/Scripts/jquery-ui/jquery-ui-1.10.4.custom.css" />
	<link rel="stylesheet" type="text/css" href="/VWB/Styles/vwb.css" />
	<link rel="stylesheet" type="text/css" href="/VWB/Scripts/jquery-ui/jquery.ui.timepicker.css" />
	<title><%= Elsevier.Library.Reference.Constants.BusinessName %> - Virtual Whiteboard</title>

</head>
<body>
	<form id="form1" runat="server">
	<div class="wrapper">
		<img class="banner" width="317" height="122" src="<%= LogoUrl %>"
			alt="<%= Elsevier.Library.Reference.Constants.BusinessName %> - Virtual Whiteboard">
		<div class="top">

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
                    <br/>
                    <asp:CheckBox runat="server" id="chkShowInProgressArticles" Text="Show in-progress articles only"/>
				</div>


				<div class="appbuttons">
					<asp:Button ID="btnRunReport" runat="server" Text="Run Report" OnClick="RunReport"/>
					&nbsp;<asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="ResetReport"/>
					&nbsp;<asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="Logout"/>
				</div>
			</div>

		</div>
		<br />
		<br />
		<br />
		<div class="report">

			<input type="checkbox" class="js-article-checkbox" name="selectedArticle" value="article1234"> Placeholder checkbox for testing

			<!-- THIS TABLE NEEDS CHECKBOXES WITH CLASS .js-article-checkbox -->
			<asp:Table ID="tblResults" runat="server" border="1">
			</asp:Table>

			<div class="vwb-report-options">

				<asp:DropDownList ID="ddColumns" runat="server"
				AutoPostBack="true" OnSelectedIndexChanged="AddColumn">
				</asp:DropDownList>

				<hr>

				<form action="/" method="post">
					<input type="hidden" class="js-hidden-selected-articles" name="selectedArticles" value="">
					<button disabled class="orange-button js-create-new-issue">Add Selected Articles to New Issue</button>
				</form>

				<hr>

				<form action="/" method="post">
					<input type="hidden" class="js-hidden-selected-articles" name="selectedArticles" value="">
					<select class="js-existing-issue">
						<option value="DEFAULT">Select an existing issue...</option>
						<option value="1">Sample Issue 1</option>
						<option value="2">Sample Issue 2</option>
					</select><br><br>
					<button disabled class="orange-button js-append-to-issue">Add Selected Articles to Existing Issue</button>
				</form>

			</div>

		</div>
	</div>
	</form>

	<script type="text/javascript">

	var checkSelectedArticles = function() {
		if($('.js-article-checkbox:checked').length) {

			$('.js-create-new-issue').prop('disabled', null);

			var existingIssue = $('.js-existing-issue').val();

			if(existingIssue && existingIssue !== 'DEFAULT') {
				$('.js-append-to-issue').prop('disabled', null);
			} else {
				$('.js-append-to-issue').prop('disabled', 'disabled');
			}

		} else {
			$('.js-create-new-issue, .js-append-to-issue').prop('disabled', 'disabled');
		}
	};

	var serializeSelectedArticles = function() {
		return $('.js-article-checkbox:checked').serialize();
	};

	$(document).ready(function() {
		checkSelectedArticles();

		$('.js-append-to-issue, .js-create-new-issue').on('click', function(e) {
			// Prevents instant form submit, before we can append selected articles
			e.preventDefault();

			var articles = serializeSelectedArticles();

			var thisForm = $(e.target.form);
			thisForm.find('.js-hidden-selected-articles').val(articles);
			// thisForm.submit();
		});

	});

	$('.js-article-checkbox, .js-existing-issue').on('change', function(e) {
		checkSelectedArticles();
	});

	</script>


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
