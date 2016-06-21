var selectedArticlesUpdated = function () {
	if($('.js-article-checkbox input:checked').length) {

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

	serializeSelectedArticles();

};

var serializeSelectedArticles = function() {

	var pipedArticles = '';
	var selectedArticles = $('.js-article-checkbox input:checked');
	if(selectedArticles.length) {
		selectedArticles.each(function(ind, elm) {
			if(ind > 0) {
				pipedArticles += '|' + (elm.checked ? elm.value : null);
			} else {
				pipedArticles = elm.checked ? elm.value : null;
			}
		});
	}

	$('#IssueArticleIdsInput').val(pipedArticles);

};

$(document).ready(function() {

	selectedArticlesUpdated();

	$(".js-new-issue-modal").dialog({
		autoOpen: false,
		minWidth: 400,
		title: 'Create a New Issue'
	});

	$('.js-new-issue-pub-date').datepicker();

	$('.js-append-to-issue, .js-create-new-issue').on('click', function(e) {

		// Prevents instant form submit, just in case
        e.preventDefault();
		var articles = serializeSelectedArticles();

	});

	$('.js-create-new-issue').on('click', function(e) {
		$(".js-new-issue-modal").dialog( "open" );
	});

	$('.js-submit-new-issue').on('click', function(e) {
	    $('#IssueTitleInput').val($('.js-new-issue-title').val());
	    $('#IssuePublishedDateInput').val($('.js-new-issue-pub-date').val());

	    selectedArticlesUpdated();
	    //document.forms[0].submit();
	    $('#NewIssueSubmitButton').click();
	});

});

$('.js-article-checkbox input, .js-existing-issue').on('change', function (e) {
	selectedArticlesUpdated();
});
