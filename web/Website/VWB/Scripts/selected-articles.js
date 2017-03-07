var selectedArticlesUpdated = function () {
    if ($('.js-article-checkbox input:checked').length) {

        $('.js-create-new-issue').prop('disabled', null);

        var existingIssue = $('.js-existing-issue').val();

        if (existingIssue && existingIssue !== 'DEFAULT') {
            $('.js-append-to-issue').prop('disabled', null);
        } else {
            $('.js-append-to-issue').prop('disabled', 'disabled');
        }

    } else {
        $('.js-create-new-issue, .js-append-to-issue').prop('disabled', 'disabled');
    }

    serializeSelectedArticles();

};

var serializeSelectedArticles = function () {

    var pipedArticles = '';
    var selectedArticles = $('.js-article-checkbox input:checked');
    if (selectedArticles.length) {
        selectedArticles.each(function (ind, elm) {
            if (ind > 0) {
                pipedArticles += '|' + (elm.checked ? elm.value : null);
            } else {
                pipedArticles = elm.checked ? elm.value : null;
            }
        });
    }

    $('#IssueArticleIdsInput').val(pipedArticles);

};

$(document)
    .ready(function () {

        selectedArticlesUpdated();

        $(".js-new-issue-modal")
            .dialog({
                autoOpen: false,
                minWidth: 400,
                title: 'Create a New Issue'
            });

        $('.js-new-issue-pub-date').datepicker();

        $('.js-append-to-issue, .js-create-new-issue')
            .on('click',
                function (e) {
                    var articles = serializeSelectedArticles();
                });

        $('.js-create-new-issue')
            .on('click',
                function (e) {
                    // Prevents instant form submit, just in case
                    e.preventDefault();
                    $(".js-new-issue-modal").dialog("open");
                });

        $('.js-submit-new-issue')
            .on('click',
                function (e) {
                    var title = $('.js-new-issue-title').val();
                    if (title.length < 1) {
                        $('.title-error').show();
                        return;
                    } else {
                        $('.title-error').hide();
                    }

                    var date = $('.js-new-issue-pub-date').val();
                    if (date.length < 1) {
                        $('.date-error').show();
                        return;
                    } else {
                        $('.date-error').hide();
                    }

                    $('#IssueTitleInput').val(title);
                    $('#IssuePublishedDateInput').val(date);

                    selectedArticlesUpdated();
                    //document.forms[0].submit();

                    $('#NewIssueSubmitButton').click();
                });

        $('.js-existing-issue')
            .on('change',
                function (e) {
                    var existingIssue = $('.js-existing-issue').val();
                    if (existingIssue && existingIssue !== 'DEFAULT') {
                        $('.js-go-to-issue').prop('disabled', null);
                    } else {
                        $('.js-go-to-issue').prop('disabled', 'disabled');
                    }
                });

        $('.js-go-to-issue')
            .on('click',
                function (e) {
                    e.preventDefault();
                    var existingIssue = $('.js-existing-issue').val();
                    if (existingIssue && existingIssue !== 'DEFAULT') {
                        window.open("/vwb/addissue?id=" + existingIssue, "_blank");
                    }
                });


        // results filters
       $('#tblResults select').bind('change', function () {
            var selectedVal = $(this).val(),
                idx = $(this).closest('td').index(),
                getallTrs = $('#tblResults tr').not('.tableheader');
            getallTrs.show();
            getallTrs.each(function (i, v) {
                if (selectedVal !== "0") {
                    var tr = $(this), td = tr.find('td:eq(' + idx + ')'), html = td.html();
                    if (html.toLowerCase().indexOf(selectedVal.toLowerCase()) !== -1) {
                        td.attr('data-filtered', 'positive');
                    }
                    else {
                        td.attr('data-filtered', 'negitive');
                    }
                }
                else {
                    var tr = $(this), td = tr.find('td:eq(' + idx + ')');
                    td.attr('data-filtered', 'positive');
                }
            });
            for (var i = 0; i < getallTrs.length; i++) {
                if ($(getallTrs[i]).find('td[data-filtered=negitive]').size() > 0) {
                    $(getallTrs[i]).hide();
                }
            }
        });
    });

$('.js-article-checkbox input, .js-existing-issue').on('change', function (e) {
    selectedArticlesUpdated();
});
