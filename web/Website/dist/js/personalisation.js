function loadLayoutOneData(data){
	var loadData = '<div class="eachstory layout1">';
	loadData += '<section class="article-preview topic-feat	ured-article">';
	loadData += (data.results[0].ListableImage) ? data.results[0].ListableImage : '';
	loadData += '<div class="article-metadata">';
	loadData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article">';
	loadData += (data.results[0].bookmarkText || data.results[0].bookmarkedText) ? '<span class="action-flag__label js-bookmark-label">'+(data.results[0].isArticleBookmarked) ? data.results[0].bookmarkedText : data.results[0].bookmarkText+'</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg>' : '';
	loadData += '</div>';
	loadData += '<ul>';
	loadData += (data.results[0].ListableDate) ? '<li><time class="article-metadata__date">'+data.results[0].ListableDate+'</time></li>' : '';
	loadData += (data.results[0].linkableText) ? '<li><h6>'+data.results[0].linkableText'+</h6></li>' : '';
	loadData += (data.results[0].listableType) ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	loadData += '</ul>';
	loadData += '</div>';
	loadData += '<div class="topic-featured-article__inner-wrapper">';
	loadData += (data.results[0].listableTitle) ? '<h3 class="topic-featured-article__headline"><a href="'+(data.results[0].linkableUrl) ? data.results[0].linkableUrl : "#"+'" class="click-utag">'+data.results[0].listableTitle+'</a></h3>';
	loadData += (data.results[0].ListableAuthorByLine) ? '<span class="article-preview__byline">'+data.results[0].ListableAuthorByLine+'</span>' : '';
	loadData += '<div class="article-summary">';
	loadData += (data.results[0].ListableSummary) ? data.results[0].ListableSummary : '';
	loadData += '</div>';
	loadData += '</div>';
	loadData += (data.results[0].listableTopics) ? '<div class="article-preview__tags bar-separated-link-list">';
	for(var i=0; i<data.results[0].listableTopics.length; i++){
		loadData += '<a href="'+data.results[0].listableTopics[i].linkableUrl+'">'+data.results[0].listableTopics[i].linkableText+'</a>';
	}
	loadData += '</div>' : '';
	loadData += '</section>';
	
	
	loadData += '<div class="latest-news__articles">';
	loadData += '<section class="article-preview article-preview--small mobview">';
	loadData += '<div class="article-metadata">';
	loadData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article">';
	loadData += (data.results[1].bookmarkText || data.results[1].bookmarkedText) ? '<span class="action-flag__label js-bookmark-label">'+(data.results[1].isArticleBookmarked) ? data.results[1].bookmarkedText : data.results[1].bookmarkText+'</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg>' : '';
	loadData += '</div>';
	loadData += '<ul>';
	loadData += (data.results[1].ListableDate) ? '<li><time class="article-metadata__date">'+data.results[1].ListableDate+'</time></li>' : '';
	loadData += (data.results[1].linkableText) ? '<li><h6>'+data.results[1].linkableText'+</h6></li>' : '';
	loadData += (data.results[1].listableType) ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	loadData += '</ul>';
	loadData += '</div>';
	loadData += '<div class="article-preview__inner-wrapper">';
	loadData += (data.results[1].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+(data.results[1].linkableUrl) ? data.results[1].linkableUrl : "#"+'" class="click-utag">'+data.results[1].listableTitle+'</a></h1>';
	loadData += (data.results[1].ListableAuthorByLine) ? '<span class="article-preview__byline">'+data.results[1].ListableAuthorByLine+'</span>' : '';
	loadData += '<div class="article-summary">';
	loadData += (data.results[1].ListableSummary) ? data.results[1].ListableSummary : '';
	loadData += '</div>';
	loadData += '</div>';
	loadData += (data.results[1].listableTopics) ? '<div class="article-preview__tags bar-separated-link-list">';
	for(var i=0; i<data.results[1].listableTopics.length; i++){
		loadData += '<a href="'+data.results[1].listableTopics[i].linkableUrl+'">'+data.results[1].listableTopics[i].linkableText+'</a>';
	}
	loadData += '</div>' : '';
	loadData += '</section>';
	
	
	loadData += '<section class="article-preview article-preview--small mobview">';
	loadData += '<div class="article-metadata">';
	loadData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article">';
	loadData += (data.results[2].bookmarkText || data.results[2].bookmarkedText) ? '<span class="action-flag__label js-bookmark-label">'+(data.results[0].isArticleBookmarked) ? data.results[2].bookmarkedText : data.results[2].bookmarkText+'</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg>' : '';
	loadData += '</div>';
	loadData += '<ul>';
	loadData += (data.results[2].ListableDate) ? '<li><time class="article-metadata__date">'+data.results[2].ListableDate+'</time></li>' : '';
	loadData += (data.results[2].linkableText) ? '<li><h6>'+data.results[2].linkableText'+</h6></li>' : '';
	loadData += (data.results[2].listableType) ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	loadData += '</ul>';
	loadData += '</div>';
	loadData += '<div class="article-preview__inner-wrapper">';
	loadData += (data.results[2].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+(data.results[2].linkableUrl) ? data.results[2].linkableUrl : "#"+'" class="click-utag">'+data.results[2].listableTitle+'</a></h1>';
	loadData += (data.results[2].ListableAuthorByLine) ? '<span class="article-preview__byline">'+data.results[2].ListableAuthorByLine+'</span>' : '';
	loadData += '<div class="article-summary">';
	loadData += (data.results[2].ListableSummary) ? data.results[2].ListableSummary : '';
	loadData += '</div>';
	loadData += '</div>';
	loadData += (data.results[2].listableTopics) ? '<div class="article-preview__tags bar-separated-link-list">';
	for(var i=0; i<data.results[2].listableTopics.length; i++){
		loadData += '<a href="'+data.results[2].listableTopics[i].linkableUrl+'">'+data.results[2].listableTopics[i].linkableText+'</a>';
	}
	loadData += '</div>' : '';
	loadData += '</section>';
	
	
	loadData += '<section class="article-preview article-preview--small topics">';
	loadData += (data.results[3].linkableText) ? '<h6>'+data.results[3].linkableText+'</h6>' : '';
	
	loadData += (data.results[3].listableTitle) ? '<h1 class="article-preview_rheadline"><a href="'+(data.results[3].linkableUrl) ? data.results[3].linkableUrl : "#"+'" class="click-utag">'+data.results[3].listableTitle+'</a></h1>';
	
	loadData += (data.results[4].listableTitle) ? '<h1 class="article-preview_rheadline"><a href="'+(data.results[4].linkableUrl) ? data.results[4].linkableUrl : "#"+'" class="click-utag">'+data.results[4].listableTitle+'</a></h1>';
	
	loadData += (data.results[5].listableTitle) ? '<h1 class="article-preview_rheadline"><a href="'+(data.results[5].linkableUrl) ? data.results[5].linkableUrl : "#"+'" class="click-utag">'+data.results[5].listableTitle+'</a></h1>';
	
	loadData += '</section>';
	loadData += '</div>';
	
	
	loadData += '<section class="article-preview article-small-preview mobview">';
	loadData += '<div class="article-metadata">';
	loadData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article">';
	loadData += (data.results[6].bookmarkText || data.results[6].bookmarkedText) ? '<span class="action-flag__label js-bookmark-label">'+(data.results[6].isArticleBookmarked) ? data.results[6].bookmarkedText : data.results[6].bookmarkText+'</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg>' : '';
	loadData += '</div>';
	loadData += '<ul>';
	loadData += (data.results[6].ListableDate) ? '<li><time class="article-metadata__date">'+data.results[6].ListableDate+'</time></li>' : '';
	loadData += (data.results[6].linkableText) ? '<li><h6>'+data.results[6].linkableText'+</h6></li>' : '';
	loadData += '</ul>';
	loadData += '</div>';
	loadData += '<div class="article-preview__inner-wrapper">';
	loadData += (data.results[6].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+(data.results[6].linkableUrl) ? data.results[6].linkableUrl : "#"+'" class="click-utag">'+data.results[6].listableTitle+'</a></h1>';
	loadData += '</div>';
	loadData += (data.results[6].listableTopics) ? '<div class="article-preview__tags bar-separated-link-list">';
	for(var i=0; i<data.results[6].listableTopics.length; i++){
		loadData += '<a href="'+data.results[6].listableTopics[i].linkableUrl+'">'+data.results[6].listableTopics[i].linkableText+'</a>';
	}
	loadData += '</section>';
	
	
	loadData += '<div class="latest-news__articles">';
	loadData += '<section class="article-preview article-small-preview mobview">';
	loadData += '<div class="article-metadata">';
	loadData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article">';
	loadData += (data.results[7].bookmarkText || data.results[7].bookmarkedText) ? '<span class="action-flag__label js-bookmark-label">'+(data.results[7].isArticleBookmarked) ? data.results[7].bookmarkedText : data.results[7].bookmarkText+'</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg>' : '';
	loadData += '</div>';
	loadData += '<ul>';
	loadData += (data.results[7].ListableDate) ? '<li><time class="article-metadata__date">'+data.results[7].ListableDate+'</time></li>' : '';
	loadData += (data.results[7].linkableText) ? '<li><h6>'+data.results[7].linkableText'+</h6></li>' : '';
	loadData += '</ul>';
	loadData += '</div>';
	loadData += '<div class="article-preview__inner-wrapper">';
	loadData += (data.results[7].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+(data.results[7].linkableUrl) ? data.results[7].linkableUrl : "#"+'" class="click-utag">'+data.results[7].listableTitle+'</a></h1>';
	loadData += '</div>';
	loadData += (data.results[7].listableTopics) ? '<div class="article-preview__tags bar-separated-link-list">';
	for(var i=0; i<data.results[7].listableTopics.length; i++){
		loadData += '<a href="'+data.results[7].listableTopics[i].linkableUrl+'">'+data.results[7].listableTopics[i].linkableText+'</a>';
	}
	loadData += '</section>';
	
	
	loadData += '<section class="article-preview article-small-preview mobview">';
	loadData += '<div class="article-metadata">';
	loadData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article">';
	loadData += (data.results[8].bookmarkText || data.results[8].bookmarkedText) ? '<span class="action-flag__label js-bookmark-label">'+(data.results[8].isArticleBookmarked) ? data.results[8].bookmarkedText : data.results[8].bookmarkText+'</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg>' : '';
	loadData += '</div>';
	loadData += '<ul>';
	loadData += (data.results[8].ListableDate) ? '<li><time class="article-metadata__date">'+data.results[8].ListableDate+'</time></li>' : '';
	loadData += (data.results[8].linkableText) ? '<li><h6>'+data.results[8].linkableText'+</h6></li>' : '';
	loadData += '</ul>';
	loadData += '</div>';
	loadData += '<div class="article-preview__inner-wrapper">';
	loadData += (data.results[8].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+(data.results[8].linkableUrl) ? data.results[8].linkableUrl : "#"+'" class="click-utag">'+data.results[8].listableTitle+'</a></h1>';
	loadData += '</div>';
	loadData += (data.results[8].listableTopics) ? '<div class="article-preview__tags bar-separated-link-list">';
	for(var i=0; i<data.results[8].listableTopics.length; i++){
		loadData += '<a href="'+data.results[8].listableTopics[i].linkableUrl+'">'+data.results[8].listableTopics[i].linkableText+'</a>';
	}
	loadData += '</section>';
	
	loadData += '</div>';
	loadData += '</div>';
	
	return loadData;
}

function loadLayoutSecondData(data){
    var loadData = '<div class="eachstory layout2">';
	loadData += '<div class="latest-news__articles">';
	loadData += '<div class="latest-news__articles">';
	loadData += (data.results[0].ListableImage) ? data.results[0].ListableImage : '';
	loadData += '<div class="article-metadata">';
	loadData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article">';
	loadData += (data.results[0].bookmarkText || data.results[0].bookmarkedText) ? '<span class="action-flag__label js-bookmark-label">'+(data.results[0].isArticleBookmarked) ? data.results[0].bookmarkedText : data.results[0].bookmarkText+'</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg>' : '';
	loadData += '</div>';
	loadData += '<ul>';
	loadData += (data.results[0].ListableDate) ? '<li><time class="article-metadata__date">'+data.results[0].ListableDate+'</time></li>' : '';
	loadData += (data.results[0].linkableText) ? '<li><h6>'+data.results[0].linkableText'+</h6></li>' : '';
	loadData += (data.results[0].listableType) ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	loadData += '</ul>';
	loadData += '</div>';
	loadData += '<div class="topic-featured-article__inner-wrapper">';
	loadData += (data.results[0].listableTitle) ? '<h3 class="topic-featured-article__headline"><a href="'+(data.results[0].linkableUrl) ? data.results[0].linkableUrl : "#"+'" class="click-utag">'+data.results[0].listableTitle+'</a></h3>';
	loadData += (data.results[0].ListableAuthorByLine) ? '<span class="article-preview__byline">'+data.results[0].ListableAuthorByLine+'</span>' : '';
	loadData += '<div class="article-summary">';
	loadData += (data.results[0].ListableSummary) ? data.results[0].ListableSummary : '';
	loadData += '</div>';
	loadData += '</div>';
	loadData += (data.results[0].listableTopics) ? '<div class="article-preview__tags bar-separated-link-list">';
	for(var i=0; i<data.results[0].listableTopics.length; i++){
		loadData += '<a href="'+data.results[0].listableTopics[i].linkableUrl+'">'+data.results[0].listableTopics[i].linkableText+'</a>';
	}
	loadData += '</section>' : '';
	loadData += '</div>' : '';
	loadData += '</div>';
	
	
	return loadData;
}

$(function(){
	$.post('topic.json', function(data){
		var layout = loadLayoutOneData(data);
		$('.eachstoryMpan').html(layout);
	});
	
	//loadLayoutSecondData();
	
	$('.loadmore').click(function(){
		var $this = $(this), eachstoryMpan = $this.closest('.eachstoryMpan'), layout = eachstoryMpan.find('.eachstory').attr('class').replace(/eachstory/ig, '').replace(/ /ig, ''), getData;
		$.ajax({
			url: '',
			data: {'layout': layout},
			type: 'POST',
			dataType: 'json',
			beforeSend: function(xhr, settings){
				
			},
			success: function(data){
				if(layout === 'layout1'){
					getData = loadNewContent(data);
				}
				else{
					getData //= loadNewContent(data);
				}
				$(eachstoryMpan).find('.eachstory').last().after(getData);
			},
			error: function(xhr, errorType, error){
				console.log('xhr ' + xhr + ' errorType ' + errorType + ' error ' + error);
			}
		});
	});
	
	/*$(window).scroll(function(){
		var eachstoryMpan = $('.personalisationPan .eachstoryMpan').last(), layout = eachstoryMpan.find('.eachstory').attr('class').replace(/eachstory/ig, '').replace(/ /ig, '');
		
		if($(window).scrollTop() > contentHei - 400){
			var getscrollData;
			$.ajax({
				url: '',
				data: {'layout': layout},
				type: 'POST',
				dataType: 'json',
				beforeSend: function(xhr, settings){
					
				},
				success: function(data){
					if(layout === 'layout1'){
						getscrollData = loadNewContent(data);
					}
					else{
						getscrollData //= loadNewContent(data);
					}
					$('.personalisationPan').append(getscrollData);
				},
				error: function(xhr, errorType, error){
					console.log('xhr ' + xhr + ' errorType ' + errorType + ' error ' + error);
				}
			});
		}
	});*/
});