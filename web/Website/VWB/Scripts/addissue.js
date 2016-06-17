$(function() {
		var orderOfArticles = [];
		var articlesToDelete = [];
		var issueID = $("input[name=issue]").val();

		$( "#datepicker" ).datepicker();

		$(".js-delete-article").click(function() {
			var locInDeleteArray = articlesToDelete.indexOf($(this).parent().attr("id"));
			if (locInDeleteArray >= 0 ) {
				$(this).parent().removeClass("to-delete");
				articlesToDelete.splice(locInDeleteArray, 1);
			}
			else {
				$(this).parent().addClass("to-delete");
				articlesToDelete.push($(this).parent().attr("id"));
			}
		});

		$(".js-draggable-wrapper").sortable({
			items: "> .js-draggable",
			create: function( event, ui ) {
				orderOfArticles = $( ".draggable-wrapper" ).sortable( "toArray" );
			},
			stop: function( event, ui ) {
				orderOfArticles = $( ".draggable-wrapper" ).sortable( "toArray" );
			},
			over: function (event, ui) {
				$(ui.item).css("width", ui.item.parent().outerWidth() + "px");
			},
			out: function (event, ui) {
				$(ui.item).css("width", "100%");
			}
		});

		$(".js-archive").click(function(e) {
			e.preventDefault();

			$("#js-dialog").html("<p>Are you sure you want to archive this issue?</p>");

			$("#js-dialog").dialog({
				closeText: "",
				resizable: false,
				buttons: [
					{
						text: "Archive Now",
						click: function() {
							$("#js-dialog").html('<p class="centered">Archiving now... <img class="archive-dialog__spinner" src="images/vwb/spinner_gray_160.gif" /></p>');
							$("#js-dialog + .ui-dialog-buttonpane").hide();
							// TODO: Update URL!
							$.post( "archive/endpoint/goes/here",
								{'id': issueID},
								function( data ) {
										if (data.success)  {
											$("#js-dialog").html('<p>Archive successful! <a href="/VWB">Return to Virtual White Board home.</a></p>');
										}
										else {
											$("#js-dialog").html("<p>Archive failed.  Please contact your system administrator.</p>");
										}
									}).fail(function() {
										$("#js-dialog").html("<p>Archive failed.  Please contact your system administrator.</p>");
									});
						}
					},
					{
						text: "Do Not Archive",
						click: function() {
							$( this ).dialog( "close" );
						}
					}
				]
			});
		});



		$(".js-save").click(function(e) {
			e.preventDefault();

			console.log({
				'id': issueID,
				'order': orderOfArticles,
				'delete': articlesToDelete,
				'title': $('input[name=title]').val(),
				'date': $('#datepicker').val(),
				'notes': $('textarea[name=notes]').val()
			});
			$.post( "save/endpoint/goes/here",
				{
					'id': issueID,
					'order': orderOfArticles,
					'delete': articlesToDelete,
					'title': $('input[name=title]').val(),
					'date': $('#datepicker').val(),
					'notes': $('textarea[name=notes]').val()
				},
				function( data ) {
					if (data.success)  {
						window.location.href = '/VWB';
					}
					else {
						$("#js-dialog").html("Save failed.  Please contact your system administrator.");
						$("#js-dialog").dialog({
							closeText: "",
							resizable: false
						});
					}
				}).fail(function() {
					$("#js-dialog").html("Save failed.  Please contact your system administrator.");
					$("#js-dialog").dialog({
						closeText: "",
						resizable: false
					});
				});
		});
	});
