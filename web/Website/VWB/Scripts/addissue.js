$(function() {
		var orderOfArticles = [];
		var orderedArticlesToKeep = [];
		var articlesToDelete = [];
		var issueID = $("#js-issue").val();

		// initialize jQuery-ui datepicker field
		$( "#js-datepicker" ).datepicker();


		// Split off kept articles (for BE) from all articles (for display)
		var makeKeepArray = function(allarticles, delarticles) {
			var keep = allarticles.slice(0);
			for (var i = 0; i < delarticles.length; i++) {
				var locInArray = keep.indexOf(delarticles[i]);
				if (locInArray >= 0) {
					keep.splice(locInArray, 1);
				}

			}
			return keep;
		};

    // Capture Editorial Notes
    var changedEditorialNotes = {};
    $(".js-editorial-note").change(function (e) {
        var sender = $(e.target);
        changedEditorialNotes[sender.parent().parent().attr("id")] = sender.val();
        $("#js-editorialnotes").val(JSON.stringify(changedEditorialNotes));
    });

		// Delete article toggle
		$(".js-delete-article").click(function() {
			var locInDeleteArray = articlesToDelete.indexOf($(this).parent().attr("id"));

			// If the article is in the deletion array
			if (locInDeleteArray >= 0 ) {
				// we must have changed our minds, so add back
				$(this).parent().removeClass("to-delete");
				articlesToDelete.splice(locInDeleteArray, 1);
				$('#js-todelete').val(articlesToDelete.join('|'));
				$('#js-order').val(makeKeepArray(orderOfArticles, articlesToDelete).join('|'));
			}
			else {
				// otherwise, delete
				$(this).parent().addClass("to-delete");
				articlesToDelete.push($(this).parent().attr("id"));
				$('#js-todelete').val(articlesToDelete.join('|'));
				$('#js-order').val(makeKeepArray(orderOfArticles, articlesToDelete).join('|'));
			}
		});

		// jQuery-ui sortable table
		$(".js-draggable-wrapper").sortable({
			items: "> .js-draggable", // Only allow sorting on things that aren't the table header
			create: function( event, ui ) {
				// When we initialize, grab the starting order of the articles
				orderOfArticles = $( ".draggable-wrapper" ).sortable( "toArray" );
				$('#js-order').val(makeKeepArray(orderOfArticles, articlesToDelete).join('|'));
			},
			stop: function( event, ui ) {
				// Any time we complete a drag, grab the order of articles again
				orderOfArticles = $( ".draggable-wrapper" ).sortable( "toArray" );
				$('#js-order').val(makeKeepArray(orderOfArticles, articlesToDelete).join('|'));
			},
			over: function (event, ui) {
				// Force the item width while dragging...
				$(ui.item).css("width", ui.item.parent().outerWidth() + "px");
			},
			out: function (event, ui) {
				// ...and reset it when we drop
				$(ui.item).css("width", "100%");
			}
		});

		// Archive this article
		$(".js-archive").click(function(e) {
			e.preventDefault();

			// Ini dialog & dialog text
			$("#js-dialog").html("<p>Are you sure you wish to archive this issue? Once you do this the issue cannot be recovered programatically.</p>");
			// Just in case it's been hidden
			$("#js-dialog + .ui-dialog-buttonpane").show();
			$("#js-dialog").dialog({
				closeText: "",
				resizable: false,
				modal: true,
				buttons: [
					{
						text: "Archive Now",
						click: function() {
							// Change dialog text to working spinner and hide the buttons so people
							//   don't try to submit twice
							$("#js-dialog").html('<p class="centered">Archiving now... <img class="archive-dialog__spinner" src="/VWB/images/vwb/spinner_gray_160.gif" /></p>');
							$("#js-dialog + .ui-dialog-buttonpane").hide();
							// Also hide close button so people don't try to work while the
							//   issue is archiving
							$("#js-dialog").siblings(".ui-dialog-titlebar").hide();

							// post to BE
							// TODO: Update URL!
							$.post( "AddIssue/ArchiveIssue",
								{'id': issueID},
								function( data ) {
										if (data.IsSuccess)  {
											$("#js-dialog").html('<p>Archive successful! <a href="/VWB">Return to Virtual White Board home.</a></p>');
										}
										else {
											// Show the close button again on failure
											$("#js-dialog").html("<p>Archive failed.  Please contact your system administrator.</p>");
										}
									}).fail(function() {
										// Show the close button again on failure
										$("#js-dialog").siblings(".ui-dialog-titlebar").show();
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
	});
