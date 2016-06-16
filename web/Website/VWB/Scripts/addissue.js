$( document ).ready(function() {
	jQuery(function ($) {
		var orderOfArticles = [];
		var articlesToDelete = [];

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
			console.log("To Delete: ", articlesToDelete);
		});

		$(".js-draggable-wrapper").sortable({
			items: "> .js-draggable",
			create: function( event, ui ) {
				orderOfArticles = $( ".draggable-wrapper" ).sortable( "toArray" );
			},
			stop: function( event, ui ) {
				orderOfArticles = $( ".draggable-wrapper" ).sortable( "toArray" );
				console.log("Article order: ", orderOfArticles);
			},
			over: function (event, ui) {
				$(ui.item).css("width", ui.item.parent().width() + "px");
			},
			out: function (event, ui) {
				$(ui.item).css("width", "100%");
			}
		});
	});
});
