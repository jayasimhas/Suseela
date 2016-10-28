function loadLayoutOneData(data, idx){
	var loadData = (loadPreferanceId["Sections"][idx]["ChannelName"]) ? '<div class="latestSubject clearfix"><span class="sub">'+ data.loadMore.latestFromText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"]+'</span><a class="editView mobview" href="'+loadPreferanceId.MyViewSettingsPageLink+'">EDIT MY VIEW</a></div>' : '',
	loadmoreLink = (data.loadMore && data.loadMore.displayLoadMore) ? data.loadMore.loadMoreLinkUrl : '#';
	loadData += '<div class="eachstoryMpan">';
	loadData += (loadPreferanceId["Sections"][idx].ChannelId) ? '<div class="eachstory layout1" id="'+loadPreferanceId["Sections"][idx].ChannelId+'">' : '';
	loadData += createLayoutInner1(data);
	loadData += '</div>';
	loadData += (data.loadMore && data.loadMore.displayLoadMore) ? '<div data-pageSize="'+data.loadMore.pageSize+'" data-pageNo="'+data.loadMore.pageNo+'" data-loadurl="'+data.loadMore.loadMoreLinkUrl+'" data-taxonomyIds="'+data.loadMore.taxonomyIds+'" class="loadmore"><span href="'+loadmoreLink+'">'+ data.loadMore.loadMoreLinkText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] +'</span></div>' : '';
	loadData += '</div>';
	
	loadData += '<div class="googleAdd"><img src="/dist/img/google-add.gif"></div>';
	
	return loadData;
} 

function createLayoutInner1(data){
	var isArticleBookmarked = (data.articles[0].isArticleBookmarked) ? data.articles[0].bookmarkedText : data.articles[0].bookmarkText,
	bookmarkTxt = (data.articles[0].bookmarkText || data.articles[0].bookmarkedText) ? '<span class="action-flag__label js-bookmark-label">'+ isArticleBookmarked +'</span>' : '',
	linkableUrl0 = (data.articles[0].linkableUrl) ? data.articles[0].linkableUrl : '#',
	linkableUrl1 = (data.articles[1].linkableUrl) ? data.articles[1].linkableUrl : '#',
	linkableUrl2 = (data.articles[2].linkableUrl) ? data.articles[2].linkableUrl : '#',
	linkableUrl3 = (data.articles[3].linkableUrl) ? data.articles[3].linkableUrl : '#',
	linkableUrl4 = (data.articles[4].linkableUrl) ? data.articles[4].linkableUrl : '#',
	linkableUrl5 = (data.articles[5].linkableUrl) ? data.articles[5].linkableUrl : '#',
	linkableUrl6 = (data.articles[6].linkableUrl) ? data.articles[6].linkableUrl : '#',
	linkableUrl7 = (data.articles[7].linkableUrl) ? data.articles[7].linkableUrl : '#',
	linkableUrl8 = (data.articles[8].linkableUrl) ? data.articles[8].linkableUrl : '#';
	
	var articleData = '';
	articleData = '<section class="article-preview topic-featured-article">';
	articleData += (data.articles[0].listableImage) ? '<img class="topic-featured-article__image" src="'+data.articles[0].listableImage+'">' : '';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article">' + bookmarkTxt + ' <svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += (data.articles[0].listableDate) ? '<li><time class="article-metadata__date">'+data.articles[0].listableDate+'</time></li>' : '';
	articleData += (data.articles[0].linkableText) ? '<li><h6>'+data.articles[0].linkableText+'</h6></li>' : '';
	articleData += (data.articles[0].listableType) ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += (data.articles[0].listableTitle) ? '<h3 class="topic-featured-article__headline"><a href="'+linkableUrl0+'" class="click-utag">'+data.articles[0].listableTitle+'</a></h3>' : '';
	articleData += (data.articles[0].listableAuthorByLine) ? '<span class="article-preview__byline">'+data.articles[0].listableAuthorByLine+'</span>' : '';
	articleData += '<div class="article-summary">' + (data.articles[0].listableSummary) ? data.articles[0].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if(data.articles[0].listableTopics){
		for(var i=0; i<data.articles[0].listableTopics.length; i++){
			var getLink8 = (data.articles[0].listableTopics[i].linkableUrl) ? data.articles[0].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="'+getLink8+'">'+data.articles[0].listableTopics[i].linkableText+'</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	
	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-preview--small mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article">';
	articleData += '<svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg>';
	articleData += '</div>';
	articleData += '<ul>';
	articleData += (data.articles[1].listableDate) ? '<li><time class="article-metadata__date">'+data.articles[1].listableDate+'</time></li>' : '';
	articleData += (data.articles[1].linkableText) ? '<li><h6>'+data.articles[1].linkableText+'</h6></li>' : '';
	articleData += (data.articles[1].listableType) ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += (data.articles[1].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+linkableUrl1+'" class="click-utag">'+data.articles[1].listableTitle+'</a></h1>' : '';
	articleData += (data.articles[1].listableAuthorByLine) ? '<span class="article-preview__byline">'+data.articles[1].listableAuthorByLine+'</span>' : '';
	articleData += '<div class="article-summary">';
	articleData += (data.articles[1].listableSummary) ? data.articles[1].listableSummary : '';
	articleData += '</div>';
	articleData += '</div>';	
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if(data.articles[1].listableTopics){
		for(var i=0; i<data.articles[1].listableTopics.length; i++){
			var getLink1 = (data.articles[1].listableTopics[i].linkableUrl) ? data.articles[1].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="'+getLink1+'">'+data.articles[1].listableTopics[i].linkableText+'</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	
	articleData += '<section class="article-preview article-preview--small mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article">';
	articleData += '<svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg>';
	articleData += '</div>';
	articleData += '<ul>';
	articleData += (data.articles[2].listableDate) ? '<li><time class="article-metadata__date">'+data.articles[2].listableDate+'</time></li>' : '';
	articleData += (data.articles[2].linkableText) ? '<li><h6>'+data.articles[2].linkableText+'</h6></li>' : '';
	articleData += (data.articles[2].listableType) ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += (data.articles[2].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+linkableUrl2+'" class="click-utag">'+data.articles[2].listableTitle+'</a></h1>' : '';
	articleData += (data.articles[2].listableAuthorByLine) ? '<span class="article-preview__byline">'+data.articles[2].listableAuthorByLine+'</span>' : '';
	articleData += '<div class="article-summary">'+ (data.articles[1].listableSummary) ? data.articles[1].listableSummary : '' + '</div>';
	articleData += '</div>';	
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if(data.articles[2].listableTopics){
		for(var i=0; i<data.articles[2].listableTopics.length; i++){
			var getLink2 = (data.articles[2].listableTopics[i].linkableUrl) ? data.articles[2].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="'+getLink2+'">'+data.articles[2].listableTopics[i].linkableText+'</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	
	articleData += '<section class="article-preview article-preview--small topics">';
	articleData += (data.articles[3].linkableText) ? '<h6>'+data.articles[3].linkableText+'</h6>' : '';
	
	articleData += (data.articles[3].listableTitle) ? '<h1 class="article-preview_rheadline"><a href="'+linkableUrl3+'" class="click-utag">'+data.articles[3].listableTitle+'</a></h1>' : '';
	
	articleData += (data.articles[4].listableTitle) ? '<h1 class="article-preview_rheadline"><a href="'+linkableUrl4+'" class="click-utag">'+data.articles[4].listableTitle+'</a></h1>' : '';
	
	articleData += (data.articles[5].listableTitle) ? '<h1 class="article-preview_rheadline"><a href="'+linkableUrl5+'" class="click-utag">'+data.articles[5].listableTitle+'</a></h1>' : '';
	
	articleData += '</section>';
	articleData += '</div>';
	
	
	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-small-preview mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article">';
	articleData += '<svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg>';
	articleData += '</div>';
	articleData += '<ul>';
	articleData += (data.articles[6].listableDate) ? '<li><time class="article-metadata__date">'+data.articles[6].listableDate+'</time></li>' : '';
	articleData += (data.articles[6].linkableText) ? '<li><h6>'+data.articles[6].linkableText+'</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += (data.articles[6].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+linkableUrl6+'" class="click-utag">'+data.articles[6].listableTitle+'</a></h1>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if(data.articles[6].listableTopics){
		for(var i=0; i<data.articles[6].listableTopics.length; i++){
			var getLink6 = (data.articles[6].listableTopics[i].linkableUrl) ? data.articles[6].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="'+getLink6+'">'+data.articles[6].listableTopics[i].linkableText+'</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	
	articleData += '<section class="article-preview article-small-preview mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article">';
	articleData += '<svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg>';
	articleData += '</div>';
	articleData += '<ul>';
	articleData += (data.articles[7].listableDate) ? '<li><time class="article-metadata__date">'+data.articles[7].listableDate+'</time></li>' : '';
	articleData += (data.articles[7].linkableText) ? '<li><h6>'+data.articles[7].linkableText+'</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += (data.articles[7].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+linkableUrl7+'" class="click-utag">'+data.articles[7].listableTitle+'</a></h1>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if(data.articles[7].listableTopics){
		for(var i=0; i<data.articles[7].listableTopics.length; i++){
			var getLink7 = (data.articles[7].listableTopics[i].linkableUrl) ? data.articles[7].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="'+getLink7+'">'+data.articles[7].listableTopics[i].linkableText+'</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	
	articleData += '<section class="article-preview article-small-preview mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article">';
	articleData += '<svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg>';
	articleData += '</div>';
	articleData += '<ul>';
	articleData += (data.articles[8].listableDate) ? '<li><time class="article-metadata__date">'+data.articles[8].listableDate+'</time></li>' : '';
	articleData += (data.articles[8].linkableText) ? '<li><h6>'+data.articles[8].linkableText+'</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += (data.articles[8].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+linkableUrl8+'" class="click-utag">'+data.articles[8].listableTitle+'</a></h1>' : '';
	articleData += (data.articles[8].listableAuthorByLine) ? '<span class="article-preview__byline">'+data.articles[8].listableAuthorByLine+'</span>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if(data.articles[8].listableTopics){
		for(var i=0; i<data.articles[8].listableTopics.length; i++){
			var getLink8 = (data.articles[8].listableTopics[i].linkableUrl) ? data.articles[8].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="'+getLink8+'">'+data.articles[8].listableTopics[i].linkableText+'</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	articleData += '</div>';
	
	return articleData;
}

function loadLayoutTwoData(data, idx){
	var loadData = (loadPreferanceId["Sections"][idx]["ChannelName"]) ? '<div class="latestSubject clearfix"><span class="sub">'+ data.loadMore.latestFromText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"]+'</span><a class="editView mobview"  href="'+loadPreferanceId.MyViewSettingsPageLink+'">EDIT MY VIEW</a></div>' : '',
	loadmoreLink = (data.loadMore && data.loadMore.displayLoadMore && data.loadMore.displayLoadMore.loadMoreLinkUrl) ? data.loadMore.displayLoadMore.loadMoreLinkUrl : '#';
	loadData += '<div class="eachstoryMpan">';
	loadData += (loadPreferanceId["Sections"][idx].ChannelId) ? '<div class="eachstory layout2" id="'+loadPreferanceId["Sections"][idx].ChannelId+'">' : '';
	loadData += createLayoutInner2(data);
	loadData += '</div>';
	
	loadData += (data.loadMore && data.loadMore.displayLoadMore) ? '<div data-pageSize="'+data.loadMore.pageSize+'" data-pageNo="'+data.loadMore.pageNo+'" data-loadurl="'+data.loadMore.loadMoreLinkUrl+'" data-taxonomyIds="'+data.loadMore.taxonomyIds+'" class="loadmore"><span href="'+loadmoreLink+'">'+ data.loadMore.loadMoreLinkText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] +'</span></div>' : '';
 
	loadData += '</div>';
	
	loadData += '<div class="googleAdd"><img src="/dist/img/google-add.gif"></div>';
	
	return loadData;
}

function createLayoutInner2(data){
	var linkableUrl0 = (data.articles[0].linkableUrl) ? data.articles[0].linkableUrl : '#',
	linkableUrl1 = (data.articles[1].linkableUrl) ? data.articles[1].linkableUrl : '#',
	linkableUrl2 = (data.articles[2].linkableUrl) ? data.articles[2].linkableUrl : '#',
	linkableUrl3 = (data.articles[3].linkableUrl) ? data.articles[3].linkableUrl : '#',
	linkableUrl4 = (data.articles[4].linkableUrl) ? data.articles[4].linkableUrl : '#',
	linkableUrl5 = (data.articles[5].linkableUrl) ? data.articles[5].linkableUrl : '#',
	linkableUrl6 = (data.articles[6].linkableUrl) ? data.articles[6].linkableUrl : '#',
	linkableUrl7 = (data.articles[7].linkableUrl) ? data.articles[7].linkableUrl : '#',
	linkableUrl8 = (data.articles[8].linkableUrl) ? data.articles[8].linkableUrl : '#';
	
	var articleData = '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-preview--small preview2">';
	articleData += (data.articles[0].listableImage) ? '<img class="topic-featured-article__image2 hidden-xs" src="'+data.articles[0].listableImage+'">' : '';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += (data.articles[0].listableDate) ? '<li><time class="article-metadata__date">'+data.articles[0].listableDate+'</time></li>' : '';
	articleData += (data.articles[0].linkableText) ? '<li><h6>'+data.articles[0].linkableText+'</h6></li>' : '';
	articleData += (data.articles[0].listableType) ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += (data.articles[0].listableImage) ? '<img class="topic-featured-article__image2 hidden-xs" src="'+data.articles[0].listableImage+'">'  : '';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += (data.articles[0].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+linkableUrl0+'" class="click-utag">'+data.articles[0].listableTitle+'</a></h1>' : '';
	articleData += (data.articles[0].listableAuthorByLine) ? '<span class="article-preview__byline">'+data.articles[0].listableAuthorByLine+'</span>' : '';
	articleData += '<div class="article-summary">' + (data.articles[0].listableSummary) ? data.articles[0].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if(data.articles[0].listableTopics){
		for(var i=0; i<data.articles[0].listableTopics.length; i++){
			var getLink0 = (data.articles[0].listableTopics[i].linkableUrl) ? data.articles[0].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="'+getLink0+'">'+data.articles[0].listableTopics[i].linkableText+'</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	
	articleData += '<section class="article-preview article-preview--small mobview artheight">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += (data.articles[1].listableDate) ? '<li><time class="article-metadata__date">'+data.articles[1].listableDate+'</time></li>' : '';
	articleData += (data.articles[1].linkableText) ? '<li><h6>'+data.articles[1].linkableText+'</h6></li>' : '';
	articleData += (data.articles[1].listableType) ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += (data.articles[1].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+linkableUrl1+'" class="click-utag">'+data.articles[1].listableTitle+'</a></h1>' : '';
	articleData += (data.articles[1].listableAuthorByLine) ? '<span class="article-preview__byline">'+data.articles[1].listableAuthorByLine+'</span>' : '';
	articleData += '<div class="article-summary">'+ (data.articles[1].listableSummary) ? data.articles[1].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if(data.articles[1].listableTopics){
		for(var i=0; i<data.articles[1].listableTopics.length; i++){
			var getLink1 = (data.articles[1].listableTopics[i].linkableUrl) ? data.articles[1].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="'+getLink1+'">'+data.articles[1].listableTopics[i].linkableText+'</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	
	articleData += '<section class="article-preview article-preview--small artheight topics">';
	articleData += (data.articles[2].linkableText) ? '<h6>'+data.articles[2].linkableText+'</h6>' : '';
	
	articleData += (data.articles[2].listableTitle) ? '<h1 class="article-preview_rheadline"><a href="'+linkableUrl2+'" class="click-utag">'+data.articles[2].listableTitle+'</a></h1>' : '';
	
	articleData += (data.articles[3].listableTitle) ? '<h1 class="article-preview_rheadline"><a href="'+linkableUrl3+'" class="click-utag">'+data.articles[3].listableTitle+'</a></h1>' : '';
	
	articleData += (data.articles[4].listableTitle) ? '<h1 class="article-preview_rheadline"><a href="'+linkableUrl4+'" class="click-utag">'+data.articles[4].listableTitle+'</a></h1>' : '';
	articleData += '</section>';
	articleData += '</div>';
	
	
	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-small-preview mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += (data.articles[5].listableDate) ? '<li><time class="article-metadata__date">'+data.articles[5].listableDate+'</time></li>' : '';
	articleData += (data.articles[5].linkableText) ? '<li><h6>'+data.articles[5].linkableText+'</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';	
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += (data.articles[5].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+linkableUrl5+'" class="click-utag">'+data.articles[5].listableTitle+'</a></h1>' : '';
	articleData += (data.articles[1].listableAuthorByLine) ? '<span class="article-preview__byline">'+data.articles[1].listableAuthorByLine+'</span>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if(data.articles[5].listableTopics){
		for(var i=0; i<data.articles[5].listableTopics.length; i++){
			var getLink5 = (data.articles[5].listableTopics[i].linkableUrl) ? data.articles[5].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="'+getLink5+'">'+data.articles[5].listableTopics[i].linkableText+'</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	
	articleData += '<section class="article-preview article-preview--small artheight mobview mtop">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += (data.articles[6].listableDate) ? '<li><time class="article-metadata__date">'+data.articles[6].listableDate+'</time></li>' : '';
	articleData += (data.articles[6].linkableText) ? '<li><h6>'+data.articles[6].linkableText+'</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += (data.articles[6].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+linkableUrl6+'" class="click-utag">'+data.articles[6].listableTitle+'</a></h1>' : '';
	articleData += (data.articles[6].listableAuthorByLine) ? '<span class="article-preview__byline">'+data.articles[6].listableAuthorByLine+'</span>' : '';
	articleData += '<div class="article-summary">'+ (data.articles[6].listableSummary) ? data.articles[6].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if(data.articles[6].listableTopics){
		for(var i=0; i<data.articles[6].listableTopics.length; i++){
			var getLink6 = (data.articles[6].listableTopics[i].linkableUrl) ? data.articles[6].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="'+getLink6+'">'+data.articles[6].listableTopics[i].linkableText+'</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
								
	articleData += '<section class="article-preview article-small-preview sm-article sm-articles mtop">';
	articleData += '<section class="sm-article mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><usexmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += (data.articles[7].listableDate) ? '<li><time class="article-metadata__date">'+data.articles[7].listableDate+'</time></li>' : '';
	articleData += (data.articles[7].linkableText) ? '<li><h6>'+data.articles[7].linkableText+'</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += (data.articles[7].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+linkableUrl7+'" class="click-utag">'+data.articles[7].listableTitle+'</a></h1>' : '';
	articleData += (data.articles[1].listableAuthorByLine) ? '<span class="article-preview__byline">'+data.articles[1].listableAuthorByLine+'</span>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if(data.articles[7].listableTopics){
		for(var i=0; i<data.articles[7].listableTopics.length; i++){
			var getLink7 = (data.articles[7].listableTopics[i].linkableUrl) ? data.articles[7].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="'+getLink7+'">'+data.articles[7].listableTopics[i].linkableText+'</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';


	articleData += '<section class="sm-article mobview">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark is-visible"><usexmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += (data.articles[8].listableDate) ? '<li><time class="article-metadata__date">'+data.articles[8].listableDate+'</time></li>' : '';
	articleData += (data.articles[8].linkableText) ? '<li><h6>'+data.articles[8].linkableText+'</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += (data.articles[8].listableTitle) ? '<h1 class="article-preview__headline"><a href="'+linkableUrl8+'" class="click-utag">'+data.articles[8].listableTitle+'</a></h1>' : '';
	articleData += (data.articles[1].listableAuthorByLine) ? '<span class="article-preview__byline">'+data.articles[1].listableAuthorByLine+'</span>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if(data.articles[8].listableTopics){
		for(var i=0; i<data.articles[8].listableTopics.length; i++){
			var getLink8 = (data.articles[8].listableTopics[i].linkableUrl) ? data.articles[8].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="'+getLink8+'">'+data.articles[8].listableTopics[i].linkableText+'</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '</section>';
	articleData += '</div>';
	
	return articleData;
}

$(function(){
	var getLayoutInfo = $('#getLayoutInfo').val(), layout1 = true, loadLayoutData = '';
	if(typeof loadPreferanceId !== "undefined"){
		var loadDynData = (loadPreferanceId["Sections"].length < loadPreferanceId.DefaultSectionLoadCount) ? loadPreferanceId["Sections"].length : loadPreferanceId.DefaultSectionLoadCount;
		for(var i=0; i<loadDynData; i++){
			var setId = loadPreferanceId["Sections"];
			if(setId.length){
				(function(idx){
					if(idx < loadPreferanceId["DefaultSectionLoadCount"]){
						$.ajax({
							//url: '/api/articlesearch?pId=980D26EA-7B85-482D-8D8C-E7F43D6955B2&pno=1&psize=9',
							//url: '/api/articlesearch?pId='+ setId[idx]["TaxonomyIds"] + '&pno=1&psize=9',
							url: '/api/articlesearch',
							data: JSON.stringify({'TaxonomyIds': setId[idx]["TaxonomyIds"], 'PageNo': 1, 'PageSize': 9 }),
							dataType: 'json',
							contentType: "application/json",
							type: 'POST',
							cache: false,
							async: false,
							beforeSend: function(){
								$('.spinnerIcon').removeClass('hidespin');
							},
							success: function(data){
								if(data.articles && typeof data.articles === "object" && data.articles.length){
									if(layout1){
										layout1 = false;
										loadLayoutData = loadLayoutOneData(data, idx);
										$('.spinnerIcon').addClass('hidespin');
										$('.personalisationPan').append(loadLayoutData);
									}
									else{
										layout1 = true;
										loadLayoutData = loadLayoutTwoData(data, idx);
										$('.spinnerIcon').addClass('hidespin');
										$('.personalisationPan').append(loadLayoutData);
									}
								}
							},
							error: function(xhr, errorType, error){
								console.log('err ' + err);
							}
						});
					}
				})(i);
			}
		}
	}
	$('.personalisationPan').on('click', '.loadmore', function(){
		var $this = $(this), eachstoryMpan = $this.closest('.eachstoryMpan'), eachstory = eachstoryMpan.find('.eachstory'), eachstoryId = eachstory.attr('id'), layoutCls = eachstory.attr('class'), loadLayoutData;
		
		var layout = (layoutCls.indexOf('layout1') !== -1) ? 'layout1' : 'layout2';
		var setId = loadPreferanceId["Sections"];

		$.ajax({
			//url: '/loaddata.json?pId='+ eachstoryId + '&pno='+pageNum+'&psize='+pageSize,
			url: $this.attr('data-loadurl'),
			dataType: 'json',
			type: 'POST',
			data: JSON.stringify({'TaxonomyIds': [$this.attr('data-taxonomyIds')], 'PageNo': $this.attr('data-pageNo'), 'PageSize': $this.attr('data-pageSize') }),
			contentType: "application/json",
			success: function(data){
				if(layout == 'layout1'){
					loadLayoutData = createLayoutInner1(data);
					$(eachstory).append(loadLayoutData);
				}
				else{
					loadLayoutData = createLayoutInner2(data);
					$(eachstory).append(loadLayoutData);
				}
			},
			error: function(xhr, errorType, error){
				console.log('err ' + err);
			}
		});
	});
	
	var layout1Flag = true, indx = 0, eachstoryLength = (loadPreferanceId && loadPreferanceId.DefaultSectionLoadCount) ? loadPreferanceId.DefaultSectionLoadCount : 0;
	$(window).scroll(function(){
		var eachstoryMpan = $('.personalisationPan .eachstoryMpan'), eachstoryMpanLast = eachstoryMpan.last(), layoutCls = eachstoryMpan.find('.eachstory').attr('class'), contentHei = $('.personalisationPan').height(), loadsection, texonomyId;
		 
		if($(window).scrollTop() > contentHei - 400){
			var getscrollData;
			
			if(typeof loadPreferanceId !== "undefined"){
				if(eachstoryLength < loadPreferanceId["Sections"].length){
					eachstoryLength++;
					loadsection = loadPreferanceId.DefaultSectionLoadCount + indx++;
					texonomyId = loadPreferanceId["Sections"][loadsection]["TaxonomyIds"];
				}
				else{
					return;
				}
			}
			else{
				return;
			}
			
			$.ajax({
				url: '/api/articlesearch', 
				data: JSON.stringify({'TaxonomyIds': texonomyId, 'PageNo': 1, 'PageSize': 9 }),
				type: 'POST',
				contentType: "application/json",
				cache: false,
				async: false,
				dataType: 'json',
				beforeSend: function(){
					$('.spinnerIcon').removeClass('hidespin');
				},
				success: function(data){
					if(data.articles && typeof data.articles === "object" && data.articles.length){
						if(eachstoryLength % 2 == 0 && layout1Flag){
							layout1Flag = false; 
							getscrollData = loadLayoutOneData(data, eachstoryLength);
							$('.spinnerIcon').addClass('hidespin');
							$('.personalisationPan').append(getscrollData);
						}
						else{
							layout1Flag = true;
							getscrollData = loadLayoutTwoData(data, eachstoryLength);
							$('.spinnerIcon').addClass('hidespin');
							$('.personalisationPan').append(getscrollData);
						}
					}
				},
				error: function(xhr, errorType, error){
					console.log('xhr ' + xhr + ' errorType ' + errorType + ' error ' + error);
				}
			});
		}
	});
	
	$('.main-menu__hoverable a', '.main-menu__section-wrapper').click(function(e){
		e.preventDefault();
		var $this = $(this), name = $this.attr('name'), getPos = $('#' + name).position(), latestSubject = $('#'+name).closest('.eachstoryMpan').prev('.latestSubject'), subjectHei = latestSubject.height(), allstoriesLen = $('.personalisationPan .eachstoryMpan').length, liIdx = $this.closest('li').index();
		
		
		if(liIdx < allstoriesLen){
			$(window).scrollTop(getPos.top - subjectHei * 3);
		}
		else{
			if(typeof loadPreferanceId !== "undefined"){
				for(var i=allstoriesLen; i<=liIdx; i++){
					var setId = loadPreferanceId["Sections"];
					(function(idx){
						$.ajax({
							url: '/loaddata.json',//?preferenceId='+ setId[idx].Id + '&pno=1&psize=9',
							dataType: 'json',
							data: {'id': setId[idx].Id},
							type: 'GET',
							cache: false,
							async: false,
							beforeSend: function(){
								$('.spinnerIcon').removeClass('hidespin');
							},
							success: function(data){
								if(idx % 2 == 0){
									loadLayoutData = loadLayoutOneData(data, idx);
									$('.personalisationPan').append(loadLayoutData);
								}
								else{
									loadLayoutData = loadLayoutTwoData(data, idx);
									$('.personalisationPan').append(loadLayoutData);
								}
							},
							error: function(xhr, errorType, error){
								console.log('err ' + err);
							},
							complete: function(xhr, status){
								if(status == "success" && $('#' + name).length){
									setTimeout(function(){
										var getlatestPos = $('#' + name).position();
										if(getlatestPos){
											$('.spinnerIcon').addClass('hidespin');
											$(window).scrollTop(getlatestPos.top - subjectHei);
										}
									}, 5);
								}
							}
						});
					})(i);
				}
			}
		}
	});
});