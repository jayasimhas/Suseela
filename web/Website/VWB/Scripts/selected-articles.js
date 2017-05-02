var createTexonamyArr = [], createauthCodesArr = [], createcontTypeArr = [], createmedTypeArr = [];
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

function loadDropdownVals(data) {
    for (var key in data) {
        if (key == 'taxonomies') {
            var taxonomiesDrop = '<div id="checks">';
            for (var prop in data[key]) {
                taxonomiesDrop += '<div>';
                taxonomiesDrop += '<input type="checkbox" value="' + prop + '" id="' + prop + '">';
                taxonomiesDrop += '<label for="' + prop + '">' + data[key][prop] + '</label>';
                taxonomiesDrop += '</div>';
            }
            taxonomiesDrop += '</div>';
            $('#ddlTaxonomies_dv').html(taxonomiesDrop);
			localStorage.setItem('texonamyObj', JSON.stringify($('#ddlTaxonomies_dv').html()));
        }
        else if (key == 'authors') {
            var authorsDrop = '<div id="checks">';
            for (var prop in data[key]) {
                authorsDrop += '<div>';
                authorsDrop += '<input type="checkbox" value="' + prop + '" id="' + prop + '">';
                authorsDrop += '<label for="' + prop + '">' + data[key][prop] + '</label>';
                authorsDrop += '</div>';
            }
            authorsDrop += '</div>';
            $('#ddlAuthors_dv').html(authorsDrop);
			localStorage.setItem('authCodesObj', JSON.stringify($('#ddlAuthors_dv').html())); 
        }
        else if (key == 'contentTypes') {
            var contentTypesDrop = '<div id="checks">';
            for (var prop in data[key]) {
                contentTypesDrop += '<div>';
                contentTypesDrop += '<input type="checkbox" value="' + prop + '" id="' + prop + '">';
                contentTypesDrop += '<label for="' + prop + '">' + data[key][prop] + '</label>';
                contentTypesDrop += '</div>';
            }
            contentTypesDrop += '</div>';
            $('#ddlContentType_dv').html(contentTypesDrop);
			localStorage.setItem('contentTypeObj', JSON.stringify($('#ddlContentType_dv').html()));
        }
        else if (key == 'mediaTypes') {
            var mediaTypesDrop = '<div id="checks">';
            for (var prop in data[key]) {
                mediaTypesDrop += '<div>';
                mediaTypesDrop += '<input type="checkbox" value="' + prop + '" id="' + prop + '">';
                mediaTypesDrop += '<label for="' + prop + '">' + data[key][prop] + '</label>';
                mediaTypesDrop += '</div>';
            }
            mediaTypesDrop += '</div>';
            $('#ddlMediaType_dv').html(mediaTypesDrop); 
			localStorage.setItem('mediaTypeObj', JSON.stringify($('#ddlMediaType_dv').html()));
        }
    };
}

function recreateIds(id){
	if(id.indexOf('-') === -1){
		var spl = id.split(''), recreateStr = '';
		
		for(var i=0; i<spl.length; i++){
			if(i == 7 || i == 11 || i == 15 || i == 19){
				recreateStr += spl[i] + '-';
			}
			else{
				recreateStr += spl[i];
			}
		}
		return recreateStr;
	}
	else{
		return id;
	}
}

function loadAjaxData(curEle){
	 var ddlVerticals = $('#ddlVerticals').val(), ddlPublications = $('#ddlPublications_sl input[type=checkbox]:checked'), getSelectedVal = '';
			
	for (var i = 0; i < ddlPublications.length; i++) {
		var coma = (i+1 == ddlPublications.length) ? '' : ',';
		getSelectedVal += ddlPublications[i].value + coma;
	}
	if($('#rbDateRange').is(':checked')){
		var txtStart = $('#txtStart').val(), txtEnd = $('#txtEnd').val();
	}
	else{
		var txtStart = '', txtEnd = '';
	}
	$.ajax({
		url: 'api/InformaVWBSearch',
		data: { 'pId': 'a0163a51-2ff8-4a9c-8fba-6516546e5ae1', 'verticalroot': ddlVerticals, 'PublicationCode': getSelectedVal, 'plannedpublishdate': txtStart + ';' + txtEnd },
		dataType: 'json',
		type: 'GET',
		beforeSend: function(){
			if(curEle !== 'btnRunReport'){
				$('.loadingIcon').css('display', 'block');
			}
		},
		success: function (data) {
			loadDropdownVals(data);
			$('.loadingIcon').removeAttr('style');
			$('.loadTexonamyData').removeClass('in');
		},
		error: function (err) {
			console.log(err);
		}
	});
}
		
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
		
		$('#ddlTaxonomies_sl').on('click', function(){
			if(createTexonamyArr.length){
				for(var i=0; i<createTexonamyArr.length; i++){
					$('#' + createTexonamyArr[i]).prop('checked', 'checked');
				}
			} 
		});
		
		$('#ddlTaxonomies_sl').on('click', function(){
			if(createauthCodesArr.length){
				for(var i=0; i<createauthCodesArr.length; i++){
					$('#' + createauthCodesArr[i]).attr('checked', true);
				} 
			}
		});
		
		$('#ddlContentType_sl').on('click', function(){
			if(createcontTypeArr.length){
				for(var i=0; i<createcontTypeArr.length; i++){
					$('#' + createcontTypeArr[i]).attr('checked', true);
				} 
			}
		});
		
		$('#ddlMediaType_sl').on('click', function(){
			if(createmedTypeArr.length){
				for(var i=0; i<createmedTypeArr.length; i++){
					$('#' + createmedTypeArr[i]).attr('checked', true);
				} 
			} 
		});
		
		var href = window.location.href;
		if(href.indexOf('taxCodes') !== -1){
			var taxCodes = href.split('taxCodes=')[1].split('&')[0], texTexonamyArr = taxCodes.split(',');
			$('#ddlTaxonomies_dv').html(JSON.parse(localStorage.getItem('texonamyObj')));
			for(var i=0; i<texTexonamyArr.length; i++){
				createTexonamyArr.push(recreateIds(texTexonamyArr[i]));
			} 
		}
		if(href.indexOf('authCodes') !== -1){
			var authCodes = href.split('authCodes=')[1].split('&')[0], authArr = authCodes.split(',');
			$('#ddlAuthors_dv').html(JSON.parse(localStorage.getItem('authCodesObj')));
			for(var i=0; i<authArr.length; i++){
				createauthCodesArr.push(recreateIds(authArr[i]));
			} 
		}
		if(href.indexOf('contTypeCodes') !== -1){
			var contTypeCodes = href.split('contTypeCodes=')[1].split('&')[0], contTypeArr = contTypeCodes.split(',');
			$('#ddlContentType_dv').html(JSON.parse(localStorage.getItem('contentTypeObj'))); 
			for(var i=0; i<contTypeArr.length; i++){
				createcontTypeArr.push(recreateIds(contTypeArr[i]));
			} 
		}
		if(href.indexOf('medTypeCodes') !== -1){
			var medTypeCodes = href.split('medTypeCodes=')[1].split('&')[0], medTypeArr = medTypeCodes.split(',');
			$('#ddlMediaType_dv').html(JSON.parse(localStorage.getItem('mediaTypeObj'))); 
			for(var i=0; i<medTypeArr.length; i++){
				createmedTypeArr.push(recreateIds(medTypeArr[i]));
			}
		}
		
        $(document).on('click', '#btnLoadFilters', function () {
			$('.loadTexonamyData').addClass('in');
			localStorage.clear();
			createTexonamyArr.length = 0;
			createauthCodesArr.length = 0;
			createcontTypeArr.length = 0;
			createmedTypeArr.length = 0;
			loadAjaxData('btnLoadFilters');  
        });
		
		$('#ddlTaxonomies_dv, #ddlAuthors_dv, #ddlContentType_dv, #ddlMediaType_dv').on('click', 'input[type=checkbox]', function(){
			var $this = $(this);
			if($this.is(':checked')){
				$this.attr('checked', 'checked');
			}
			else{
				$this.removeAttr('checked');
			}
		});
	
		$('#ddlPublications_dv').on('click', 'input[type=checkbox]', function(){
			if(!$('#ddlPublications_dv input[type=checkbox]:checked').length){
				$('#ddlTaxonomies_dv, #ddlAuthors_dv, #ddlContentType_dv, #ddlMediaType_dv').html('');
			}
		}); 
		
		$('#btnRunReport').on('click', function(){
			var Taxonomies_dvStr = '', Authors_dvStr = '', ContentType_dvStr = '', MediaType_dvStr = '';
			$('#ddlTaxonomies_dv input[type=checkbox]:checked').each(function(idx, val){
				Taxonomies_dvStr += val.value +',';
			});
		
			$('#ddlAuthors_dv input[type=checkbox]:checked').each(function(idx, val){
				Authors_dvStr += val.value +',';
			});
		
			$('#ddlContentType_dv input[type=checkbox]:checked').each(function(idx, val){
				ContentType_dvStr += val.value +',';
			});
		
			$('#ddlMediaType_dv input[type=checkbox]:checked').each(function(idx, val){
				MediaType_dvStr += val.value +',';
			});
			
			$('#hdnSelectedTaxs').val(Taxonomies_dvStr);
			$('#hdnSelectedAuths').val(Authors_dvStr);
			$('#hdnSelectedContTypes').val(ContentType_dvStr);
			$('#hdnSelectedMedTypes').val(MediaType_dvStr); 
		});
		
		$('#txtArticleNumber').on('keydown', function(e){
			if(e.keyCode == 13){
				e.preventDefault();
				$('#btnRunReport').trigger('click');
			}
		});
		
		$('#btnLoadFilters').prop('disabled', true);
		$('#ddlPublications_dv input[type=checkbox]').click(function(){
			var checkedbox = $('#ddlPublications_dv input[type=checkbox]:checked');
			(checkedbox.length) ? $('#btnLoadFilters').prop('disabled', false) : $('#btnLoadFilters').prop('disabled', true);
		});
		
		if(!$('#ddlPublications_dv input[type=checkbox]:checked').length){
			$('#ddlTaxonomies_dv, #ddlAuthors_dv, #ddlContentType_dv, #ddlMediaType_dv').html('');
		}
		else{
			if(localStorage.getItem('texonamyObj') !== null){
				$('#ddlTaxonomies_dv').html(JSON.parse(localStorage.getItem('texonamyObj')));
			}
			if(localStorage.getItem('authCodesObj') !== null){
				$('#ddlAuthors_dv').html(JSON.parse(localStorage.getItem('authCodesObj')));
			}
			if(localStorage.getItem('contentTypeObj') !== null){
				$('#ddlContentType_dv').html(JSON.parse(localStorage.getItem('contentTypeObj')));
			}
			if(localStorage.getItem('mediaTypeObj') !== null){
				$('#ddlMediaType_dv').html(JSON.parse(localStorage.getItem('mediaTypeObj'))); 
			}
		}
    });

$('.js-article-checkbox input, .js-existing-issue').on('change', function (e) {
    selectedArticlesUpdated();
});