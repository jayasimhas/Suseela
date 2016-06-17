<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/jquery-ui.min.js"></script>
	<script type="text/javascript" src="/VWB/Scripts/addissue.js"></script>

	<link rel="stylesheet" type="text/css" href="/VWB//Scripts/jquery-ui/jquery-ui-1.8.11.custom.css" />
	<link rel="stylesheet" type="text/css" href="/VWB//Styles/vwb.css" />
	<title>Add Issue - Virtual Whiteboard</title>

</head>
<body class="bigger">
	<div class="pad-it">
		<form id="form1">
		<h2>Issue Information</h2>

		<table class="basic">
			<tr>
				<td>
					Issue Number
				</td>
				<td>
					<!-- PLACEHOLDER! BE should fill hidden field and visible text -->
					4735687436573
					<input type="hidden" name="issue" value="4735687436573" />
					<button class="js-archive">Archive Issue</button>
				</td>
			</tr>
			<tr>
				<td>
					Title
				</td>
				<td>
					<!-- PLACEHOLDER! BE should fill initial value -->
					<input type="text" name="title" value="This value comes from BE on page load" />
				</td>
			</tr>
			<tr>
				<td>
					Date Published
				</td>
				<td>
					<!-- PLACEHOLDER! BE should fill initial value -->
					<input type="text" name="date" id="js-datepicker" value="06/08/2016" />
				</td>
			</tr>
		</table>

		<h2>Articles in Issue</h2>

		<div class="draggable-wrapper js-draggable-wrapper">
			<div class="draggable draggable-header">
				<div>
					Article Number
				</div>
				<div>
					Article Name
				</div>
				<div>
					Delete/Re-Add
				</div>
			</div>
			<!-- PLACEHOLDER! BE should create row for each article -->
			<div class="draggable js-draggable" id="98765432109876">
				<div>
					98765432109876
				</div>
				<div>
					Placeholder Article Title with some other things
				</div>
				<div class="delete-article js-delete-article">

					<a class="delete-button">Toggle Delete</a>
				</div>
			</div>
			<div class="draggable js-draggable" id="12345678901234">
				<div>
					12345678901234
				</div>
				<div>
					Placeholder Article Title with some other things
				</div>
				<div class="delete-article js-delete-article">

					<a class="delete-button">Toggle Delete</a>
				</div>
			</div>
			<div class="draggable js-draggable" id="27485629873465">
				<div>
					27485629873465
				</div>
				<div>
					Placeholder Article Title with some other things
				</div>
				<div class="delete-article js-delete-article">

					<a class="delete-button">Toggle Delete</a>
				</div>
			</div>
			<div class="draggable js-draggable" id="36450246589694">
				<div>
					36450246589694
				</div>
				<div>
					Placeholder Article Title with some other things
				</div>
				<div class="delete-article js-delete-article">

					<a class="delete-button">Toggle Delete</a>
				</div>
			</div>
		</div>

		<div class="issue-notes">
			<h3>Issue Notes</h3>
			<textarea name="notes"></textarea>
		</div>

		<button class="js-save">Save Updates to Issue</button>
		<button class="js-save">Save and Add a New Article</button>
		<a href="/vwb">Leave Without Saving</a>

		</form>
	</div>

	<div id="js-dialog">
	</div>
</body>
</html>
