function loadLayoutOneData(data, idx) {
	var editMyView = loadPreferanceId.EditMyViewButtonLableText ? '<a class="editView button--filled button--outline mobview" href="' + loadPreferanceId.MyViewSettingsPageLink + '">' + loadPreferanceId.EditMyViewButtonLableText + '</a>' : '';
	var seeAllTopics = data.loadMore && data.loadMore.seeAllLink ? '<a class="seeAllChannels button--filled button--outline mobview" href="' + data.loadMore.seeAllLink + loadPreferanceId["Sections"][idx]["ChannelName"] + '">' + data.loadMore.seeAllText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</a>' : '';

	var loadData = loadPreferanceId["Sections"][idx]["ChannelName"] ? '<div class="latestSubject clearfix" id="' + loadPreferanceId["Sections"][idx].ChannelId + '"><div class="articleloadInfo">'+data.loadMore.currentlyViewingText+'</div><div class="fllatestSub"><span class="sub">' + data.loadMore.latestFromText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</span></div><div class="frEditview">' + editMyView + seeAllTopics + '</div></div>' : '',
	    loadmoreLink = data.loadMore && data.loadMore.displayLoadMore ? data.loadMore.loadMoreLinkUrl : '#';
	loadData += '<div class="eachstoryMpan">';
	loadData += loadPreferanceId["Sections"][idx].ChannelId ? '<div class="eachstory layout1">' : '';
	loadData += createLayoutInner1(data);
	loadData += '</div>';
	loadData += data.loadMore && data.loadMore.displayLoadMore ? '<div class="loadmore"><span href="' + loadmoreLink + '">' + data.loadMore.loadMoreLinkText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</span></div>' : '';
	loadData += '</div>';

	//loadData += '<div class="googleAdd"><img src="/dist/img/google-add.gif"></div>';

	return loadData;
}

function createLayoutInner1(data) {
	var isArticleBookmarked = data.articles[0].isArticleBookmarked ? data.articles[0].bookmarkedText : data.articles[0].bookmarkText,
	    bookmarkTxt = data.articles[0].bookmarkText || data.articles[0].bookmarkedText ? '<span class="action-flag__label js-bookmark-label">' + isArticleBookmarked + '</span>' : '',
	    linkableUrl0 = data.articles[0].linkableUrl ? data.articles[0].linkableUrl : '#',
	    linkableUrl1 = data.articles[1].linkableUrl ? data.articles[1].linkableUrl : '#',
	    linkableUrl2 = data.articles[2].linkableUrl ? data.articles[2].linkableUrl : '#',
	    linkableUrl3 = data.articles[3].linkableUrl ? data.articles[3].linkableUrl : '#',
	    linkableUrl4 = data.articles[4].linkableUrl ? data.articles[4].linkableUrl : '#',
	    linkableUrl5 = data.articles[5].linkableUrl ? data.articles[5].linkableUrl : '#',
	    linkableUrl6 = data.articles[6].linkableUrl ? data.articles[6].linkableUrl : '#',
	    linkableUrl7 = data.articles[7].linkableUrl ? data.articles[7].linkableUrl : '#',
	    linkableUrl8 = data.articles[8].linkableUrl ? data.articles[8].linkableUrl : '#',
	    bookmarkInfo0 = data.articles[0].isArticleBookmarked ? data.articles[0].bookmarkedText : data.articles[0].bookmarkText,
	    bookmarkInfo1 = data.articles[1].isArticleBookmarked ? data.articles[1].bookmarkedText : data.articles[1].bookmarkText,
	    bookmarkInfo2 = data.articles[2].isArticleBookmarked ? data.articles[2].bookmarkedText : data.articles[2].bookmarkText,
	    bookmarkInfo6 = data.articles[6].isArticleBookmarked ? data.articles[6].bookmarkedText : data.articles[6].bookmarkText,
	    bookmarkInfo7 = data.articles[7].isArticleBookmarked ? data.articles[7].bookmarkedText : data.articles[7].bookmarkText,
	    bookmarkInfo8 = data.articles[8].isArticleBookmarked ? data.articles[8].bookmarkedText : data.articles[8].bookmarkText,
	    fbookmarkIcon0 = data.articles[0].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon0 = data.articles[0].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon1 = data.articles[1].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon1 = data.articles[1].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon2 = data.articles[2].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon2 = data.articles[2].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon6 = data.articles[6].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon6 = data.articles[6].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon7 = data.articles[7].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon7 = data.articles[7].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon8 = data.articles[8].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon8 = data.articles[8].isArticleBookmarked ? '' : 'is-visible';

	var articleData = ''; 
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[0].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[0].listableImage + '">' : '';
	articleData += data.articles[0].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[0].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[0].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[0].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[0].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[0].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[0].listableDate ? '<li><time class="article-metadata__date">' + data.articles[0].listableDate + '</time></li>' : '';
	articleData += data.articles[0].linkableText ? '<li><h6>' + data.articles[0].linkableText + '</h6></li>' : '';
	articleData += data.articles[0].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[0].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[0].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[0].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[0].bookmarkText + '" data-label-bookmarked="' + data.articles[0].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[0].listableSummary ? data.articles[0].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[0].listableTopics) {
		for (var i = 0; i < data.articles[0].listableTopics.length; i++) {
			var getlistLink1 = data.articles[0].listableTopics[i].linkableUrl ? data.articles[0].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[0].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[1].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[1].listableImage + '">' : '';
	articleData += data.articles[1].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[1].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[1].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[1].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[1].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[1].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[1].listableDate ? '<li><time class="article-metadata__date">' + data.articles[1].listableDate + '</time></li>' : '';
	articleData += data.articles[1].linkableText ? '<li><h6>' + data.articles[1].linkableText + '</h6></li>' : '';
	articleData += data.articles[1].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[1].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[1].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[1].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[1].bookmarkText + '" data-label-bookmarked="' + data.articles[1].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[1].listableSummary ? data.articles[1].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[1].listableTopics) {
		for (var i = 0; i < data.articles[1].listableTopics.length; i++) {
			var getlistLink1 = data.articles[1].listableTopics[i].linkableUrl ? data.articles[1].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[1].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[2].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[2].listableImage + '">' : '';
	articleData += data.articles[2].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[2].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[2].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[2].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[2].listableDate ? '<li><time class="article-metadata__date">' + data.articles[2].listableDate + '</time></li>' : '';
	articleData += data.articles[2].linkableText ? '<li><h6>' + data.articles[2].linkableText + '</h6></li>' : '';
	articleData += data.articles[2].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[2].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[2].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[2].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[2].bookmarkText + '" data-label-bookmarked="' + data.articles[2].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[2].listableSummary ? data.articles[2].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[2].listableTopics) {
		for (var i = 0; i < data.articles[2].listableTopics.length; i++) {
			var getlistLink1 = data.articles[2].listableTopics[i].linkableUrl ? data.articles[2].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[2].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[3].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[3].listableImage + '">' : '';
	articleData += data.articles[3].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[3].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[3].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[3].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[3].listableDate ? '<li><time class="article-metadata__date">' + data.articles[3].listableDate + '</time></li>' : '';
	articleData += data.articles[3].linkableText ? '<li><h6>' + data.articles[3].linkableText + '</h6></li>' : '';
	articleData += data.articles[3].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[3].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[3].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[3].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[3].bookmarkText + '" data-label-bookmarked="' + data.articles[3].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[3].listableSummary ? data.articles[3].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[3].listableTopics) {
		for (var i = 0; i < data.articles[3].listableTopics.length; i++) {
			var getlistLink1 = data.articles[3].listableTopics[i].linkableUrl ? data.articles[3].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[3].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[4].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[4].listableImage + '">' : '';
	articleData += data.articles[4].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[4].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[4].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[4].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[4].listableDate ? '<li><time class="article-metadata__date">' + data.articles[4].listableDate + '</time></li>' : '';
	articleData += data.articles[4].linkableText ? '<li><h6>' + data.articles[4].linkableText + '</h6></li>' : '';
	articleData += data.articles[4].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[4].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[4].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[4].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[4].bookmarkText + '" data-label-bookmarked="' + data.articles[4].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[4].listableSummary ? data.articles[4].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[4].listableTopics) {
		for (var i = 0; i < data.articles[4].listableTopics.length; i++) {
			var getlistLink1 = data.articles[4].listableTopics[i].linkableUrl ? data.articles[4].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[4].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[5].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[5].listableImage + '">' : '';
	articleData += data.articles[5].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[5].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[5].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[5].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[5].listableDate ? '<li><time class="article-metadata__date">' + data.articles[5].listableDate + '</time></li>' : '';
	articleData += data.articles[5].linkableText ? '<li><h6>' + data.articles[5].linkableText + '</h6></li>' : '';
	articleData += data.articles[5].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[5].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[5].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[5].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[5].bookmarkText + '" data-label-bookmarked="' + data.articles[5].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[5].listableSummary ? data.articles[5].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[5].listableTopics) {
		for (var i = 0; i < data.articles[5].listableTopics.length; i++) {
			var getlistLink1 = data.articles[5].listableTopics[i].linkableUrl ? data.articles[5].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[5].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[6].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[6].listableImage + '">' : '';
	articleData += data.articles[6].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[6].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[6].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[6].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[6].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[6].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[6].listableDate ? '<li><time class="article-metadata__date">' + data.articles[6].listableDate + '</time></li>' : '';
	articleData += data.articles[6].linkableText ? '<li><h6>' + data.articles[6].linkableText + '</h6></li>' : '';
	articleData += data.articles[6].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[6].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[6].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[6].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[6].bookmarkText + '" data-label-bookmarked="' + data.articles[6].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[6].listableSummary ? data.articles[6].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[6].listableTopics) {
		for (var i = 0; i < data.articles[6].listableTopics.length; i++) {
			var getlistLink1 = data.articles[6].listableTopics[i].linkableUrl ? data.articles[6].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[6].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[7].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[7].listableImage + '">' : '';
	articleData += data.articles[7].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[7].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[7].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[7].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[7].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[7].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[7].listableDate ? '<li><time class="article-metadata__date">' + data.articles[7].listableDate + '</time></li>' : '';
	articleData += data.articles[7].linkableText ? '<li><h6>' + data.articles[7].linkableText + '</h6></li>' : '';
	articleData += data.articles[7].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[7].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[7].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[7].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[7].bookmarkText + '" data-label-bookmarked="' + data.articles[7].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[7].listableSummary ? data.articles[7].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[7].listableTopics) {
		for (var i = 0; i < data.articles[7].listableTopics.length; i++) {
			var getlistLink1 = data.articles[7].listableTopics[i].linkableUrl ? data.articles[7].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[7].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[8].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[8].listableImage + '">' : '';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[8].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[8].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[8].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[8].bookmarkText + '" data-label-bookmarked="' + data.articles[8].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[8].listableDate ? '<li><time class="article-metadata__date">' + data.articles[8].listableDate + '</time></li>' : '';
	articleData += data.articles[8].linkableText ? '<li><h6>' + data.articles[8].linkableText + '</h6></li>' : '';
	articleData += data.articles[8].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += data.articles[8].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[8].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[8].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[8].listableTitle + '</a></h3>' : '';
	articleData += data.articles[8].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[8].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">' + data.articles[8].listableSummary ? data.articles[8].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[8].listableTopics) {
		for (var i = 0; i < data.articles[8].listableTopics.length; i++) {
			var getlistLink1 = data.articles[8].listableTopics[i].linkableUrl ? data.articles[8].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[8].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	
	articleData += '<section class="article-preview topic-featured-article gridViewCont">';
	articleData += data.articles[0].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[0].listableImage + '">' : '';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[0].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[0].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[0].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[0].bookmarkText + '" data-label-bookmarked="' + data.articles[0].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[0].listableDate ? '<li><time class="article-metadata__date">' + data.articles[0].listableDate + '</time></li>' : '';
	articleData += data.articles[0].linkableText ? '<li><h6>' + data.articles[0].linkableText + '</h6></li>' : '';
	articleData += data.articles[0].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += data.articles[0].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[0].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[0].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[0].listableTitle + '</a></h3>' : '';
	articleData += data.articles[0].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[0].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">' + data.articles[0].listableSummary ? data.articles[0].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[0].listableTopics) {
		for (var i = 0; i < data.articles[0].listableTopics.length; i++) {
			var getLink8 = data.articles[0].listableTopics[i].linkableUrl ? data.articles[0].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink8 + '">' + data.articles[0].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-preview--small mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[1].id + '" data-analytics="{"bookmark": "' + bookmarkInfo1 + '", "bookmark_title": "' + data.articles[1].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[1].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[1].bookmarkText + '" data-label-bookmarked="' + data.articles[1].bookmarkedText + '">' + bookmarkInfo1 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon1 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon1 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[1].listableDate ? '<li><time class="article-metadata__date">' + data.articles[1].listableDate + '</time></li>' : '';
	articleData += data.articles[1].linkableText ? '<li><h6>' + data.articles[1].linkableText + '</h6></li>' : '';
	articleData += data.articles[1].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[1].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl1 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[1].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[1].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[1].listableTitle + '</a></h1>' : '';
	articleData += data.articles[1].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[1].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">';
	articleData += data.articles[1].listableSummary ? data.articles[1].listableSummary : '';
	articleData += '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[1].listableTopics) {
		for (var i = 0; i < data.articles[1].listableTopics.length; i++) {
			var getLink1 = data.articles[1].listableTopics[i].linkableUrl ? data.articles[1].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink1 + '">' + data.articles[1].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-preview--small mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[2].id + '" data-analytics="{"bookmark": "' + bookmarkInfo2 + '", "bookmark_title": "' + data.articles[2].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[2].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[2].bookmarkText + '" data-label-bookmarked="' + data.articles[2].bookmarkedText + '">' + bookmarkInfo2 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon2 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon2 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[2].listableDate ? '<li><time class="article-metadata__date">' + data.articles[2].listableDate + '</time></li>' : '';
	articleData += data.articles[2].linkableText ? '<li><h6>' + data.articles[2].linkableText + '</h6></li>' : '';
	articleData += data.articles[2].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[2].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl2 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[2].listableTitle + '</a></h1>' : '';
	articleData += data.articles[2].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[2].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">' + data.articles[2].listableSummary ? data.articles[2].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[2].listableTopics) {
		for (var i = 0; i < data.articles[2].listableTopics.length; i++) {
			var getLink2 = data.articles[2].listableTopics[i].linkableUrl ? data.articles[2].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink2 + '">' + data.articles[2].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-preview--small topics gridViewCont">';
	articleData += data.articles[3].linkableText ? '<h6>&nbsp;</h6>' : '';

	articleData += data.articles[3].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl3 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[3].listableTitle + '</a></h1>' : '';

	articleData += data.articles[4].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl4 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[4].listableTitle + '</a></h1>' : '';

	articleData += data.articles[5].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl5 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[5].listableTitle + '</a></h1>' : '';

	articleData += '</section>';
	articleData += '</div>';

	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-small-preview mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[6].id + '" data-analytics="{"bookmark": "' + bookmarkInfo6 + '", "bookmark_title": "' + data.articles[6].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[6].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[6].bookmarkText + '" data-label-bookmarked="' + data.articles[6].bookmarkedText + '">' + bookmarkInfo6 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked  ' + fbookmarkIcon6 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon6 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[6].listableDate ? '<li><time class="article-metadata__date">' + data.articles[6].listableDate + '</time></li>' : '';
	articleData += data.articles[6].linkableText ? '<li><h6>' + data.articles[6].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[6].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl6 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[6].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[6].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[6].listableTitle + '</a></h1>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[6].listableTopics) {
		for (var i = 0; i < data.articles[6].listableTopics.length; i++) {
			var getLink6 = data.articles[6].listableTopics[i].linkableUrl ? data.articles[6].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink6 + '">' + data.articles[6].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-small-preview mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[7].id + '" data-analytics="{"bookmark": "' + bookmarkInfo7 + '", "bookmark_title": "' + data.articles[7].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[7].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[7].bookmarkText + '" data-label-bookmarked="' + data.articles[7].bookmarkedText + '">' + bookmarkInfo7 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon7 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon7 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[7].listableDate ? '<li><time class="article-metadata__date">' + data.articles[7].listableDate + '</time></li>' : '';
	articleData += data.articles[7].linkableText ? '<li><h6>' + data.articles[7].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[7].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl7 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[7].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[7].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[7].listableTitle + '</a></h1>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[7].listableTopics) {
		for (var i = 0; i < data.articles[7].listableTopics.length; i++) {
			var getLink7 = data.articles[7].listableTopics[i].linkableUrl ? data.articles[7].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink7 + '">' + data.articles[7].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-small-preview mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[8].id + '" data-analytics="{"bookmark": "' + bookmarkInfo8 + '", "bookmark_title": "' + data.articles[8].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[8].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[8].bookmarkText + '" data-label-bookmarked="' + data.articles[8].bookmarkedText + '">' + bookmarkInfo8 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon8 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon8 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[8].listableDate ? '<li><time class="article-metadata__date">' + data.articles[8].listableDate + '</time></li>' : '';
	articleData += data.articles[8].linkableText ? '<li><h6>' + data.articles[8].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[8].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl8 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[8].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[8].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[8].listableTitle + '</a></h1>' : '';
	articleData += data.articles[8].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[8].listableAuthorByLine + '</span>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[8].listableTopics) {
		for (var i = 0; i < data.articles[8].listableTopics.length; i++) {
			var getLink8 = data.articles[8].listableTopics[i].linkableUrl ? data.articles[8].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink8 + '">' + data.articles[8].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	articleData += '</div>';

	articleData += '<input type="hidden" class="getPaginationNum" data-pageSize="' + data.loadMore.pageSize + '" data-pageNo="' + data.loadMore.pageNo + '" data-loadurl="' + data.loadMore.loadMoreLinkUrl + '" data-taxonomyIds="' + data.loadMore.taxonomyIds + '" />';

	return articleData;
}

function loadLayoutTwoData(data, idx) {
	var editMyView = loadPreferanceId.EditMyViewButtonLableText ? '<a class="editView button--filled button--outline mobview" href="' + loadPreferanceId.MyViewSettingsPageLink + '">' + loadPreferanceId.EditMyViewButtonLableText + '</a>' : '';
	var seeAllTopics = data.loadMore && data.loadMore.seeAllLink ? '<a class="seeAllChannels button--filled button--outline mobview" href="' + data.loadMore.seeAllLink + loadPreferanceId["Sections"][idx]["ChannelName"] + '">' + data.loadMore.seeAllText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</a>' : '';		
						
	var loadData = loadPreferanceId["Sections"][idx]["ChannelName"] ? '<div class="latestSubject clearfix" id="' + loadPreferanceId["Sections"][idx].ChannelId + '"><div class="articleloadInfo">'+data.loadMore.currentlyViewingText+'</div><div class="fllatestSub"><span class="sub">' + data.loadMore.latestFromText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</span></div><div class="frEditview">' + editMyView + seeAllTopics + '</div></div>' : '',
	    loadmoreLink = data.loadMore && data.loadMore.displayLoadMore && data.loadMore.displayLoadMore.loadMoreLinkUrl ? data.loadMore.displayLoadMore.loadMoreLinkUrl : '#';
	loadData += '<div class="eachstoryMpan">';
	loadData += loadPreferanceId["Sections"][idx].ChannelId ? '<div class="eachstory layout2">' : '';
	loadData += createLayoutInner2(data);
	loadData += '</div>';

	loadData += data.loadMore && data.loadMore.displayLoadMore ? '<div class="loadmore"><span href="' + loadmoreLink + '">' + data.loadMore.loadMoreLinkText + ' ' + loadPreferanceId["Sections"][idx]["ChannelName"] + '</span></div>' : '';

	loadData += '</div>';

	//loadData += '<div class="googleAdd"><img src="/dist/img/google-add.gif"></div>';

	return loadData;
}

function createLayoutInner2(data) {
	var linkableUrl0 = data.articles[0].linkableUrl ? data.articles[0].linkableUrl : '#',
	    linkableUrl1 = data.articles[1].linkableUrl ? data.articles[1].linkableUrl : '#',
	    linkableUrl2 = data.articles[2].linkableUrl ? data.articles[2].linkableUrl : '#',
	    linkableUrl3 = data.articles[3].linkableUrl ? data.articles[3].linkableUrl : '#',
	    linkableUrl4 = data.articles[4].linkableUrl ? data.articles[4].linkableUrl : '#',
	    linkableUrl5 = data.articles[5].linkableUrl ? data.articles[5].linkableUrl : '#',
	    linkableUrl6 = data.articles[6].linkableUrl ? data.articles[6].linkableUrl : '#',
	    linkableUrl7 = data.articles[7].linkableUrl ? data.articles[7].linkableUrl : '#',
	    linkableUrl8 = data.articles[8].linkableUrl ? data.articles[8].linkableUrl : '#',
	    bookmarkInfo0 = data.articles[0].isArticleBookmarked ? data.articles[0].bookmarkedText : data.articles[0].bookmarkText,
	    bookmarkInfo1 = data.articles[1].isArticleBookmarked ? data.articles[1].bookmarkedText : data.articles[1].bookmarkText,
	    bookmarkInfo5 = data.articles[5].isArticleBookmarked ? data.articles[5].bookmarkedText : data.articles[5].bookmarkText,
	    bookmarkInfo6 = data.articles[6].isArticleBookmarked ? data.articles[6].bookmarkedText : data.articles[6].bookmarkText,
	    bookmarkInfo7 = data.articles[7].isArticleBookmarked ? data.articles[7].bookmarkedText : data.articles[7].bookmarkText,
	    bookmarkInfo8 = data.articles[8].isArticleBookmarked ? data.articles[8].bookmarkedText : data.articles[8].bookmarkText,
	    fbookmarkIcon0 = data.articles[0].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon0 = data.articles[0].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon1 = data.articles[1].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon1 = data.articles[1].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon5 = data.articles[5].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon5 = data.articles[5].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon6 = data.articles[6].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon6 = data.articles[6].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon7 = data.articles[7].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon7 = data.articles[7].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon8 = data.articles[8].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon8 = data.articles[8].isArticleBookmarked ? '' : 'is-visible';

	var articleData = '';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[0].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[0].listableImage + '">' : '';
	articleData += data.articles[0].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[0].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[0].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[0].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[0].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[0].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[0].listableDate ? '<li><time class="article-metadata__date">' + data.articles[0].listableDate + '</time></li>' : '';
	articleData += data.articles[0].linkableText ? '<li><h6>' + data.articles[0].linkableText + '</h6></li>' : '';
	articleData += data.articles[0].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[0].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[0].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[0].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[0].bookmarkText + '" data-label-bookmarked="' + data.articles[0].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[0].listableSummary ? data.articles[0].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[0].listableTopics) {
		for (var i = 0; i < data.articles[0].listableTopics.length; i++) {
			var getlistLink1 = data.articles[0].listableTopics[i].linkableUrl ? data.articles[0].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[0].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[1].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[1].listableImage + '">' : '';
	articleData += data.articles[1].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[1].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[1].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[1].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[1].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[1].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[1].listableDate ? '<li><time class="article-metadata__date">' + data.articles[1].listableDate + '</time></li>' : '';
	articleData += data.articles[1].linkableText ? '<li><h6>' + data.articles[1].linkableText + '</h6></li>' : '';
	articleData += data.articles[1].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[1].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[1].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[1].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[1].bookmarkText + '" data-label-bookmarked="' + data.articles[1].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[1].listableSummary ? data.articles[1].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[1].listableTopics) {
		for (var i = 0; i < data.articles[1].listableTopics.length; i++) {
			var getlistLink1 = data.articles[1].listableTopics[i].linkableUrl ? data.articles[1].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[1].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[2].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[2].listableImage + '">' : '';
	articleData += data.articles[2].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[2].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[2].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[2].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[2].listableDate ? '<li><time class="article-metadata__date">' + data.articles[2].listableDate + '</time></li>' : '';
	articleData += data.articles[2].linkableText ? '<li><h6>' + data.articles[2].linkableText + '</h6></li>' : '';
	articleData += data.articles[2].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[2].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[2].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[2].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[2].bookmarkText + '" data-label-bookmarked="' + data.articles[2].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[2].listableSummary ? data.articles[2].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[2].listableTopics) {
		for (var i = 0; i < data.articles[2].listableTopics.length; i++) {
			var getlistLink1 = data.articles[2].listableTopics[i].linkableUrl ? data.articles[2].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[2].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[3].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[3].listableImage + '">' : '';
	articleData += data.articles[3].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[3].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[3].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[3].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[3].listableDate ? '<li><time class="article-metadata__date">' + data.articles[3].listableDate + '</time></li>' : '';
	articleData += data.articles[3].linkableText ? '<li><h6>' + data.articles[3].linkableText + '</h6></li>' : '';
	articleData += data.articles[3].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[3].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[3].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[3].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[3].bookmarkText + '" data-label-bookmarked="' + data.articles[3].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[3].listableSummary ? data.articles[3].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[3].listableTopics) {
		for (var i = 0; i < data.articles[3].listableTopics.length; i++) {
			var getlistLink1 = data.articles[3].listableTopics[i].linkableUrl ? data.articles[3].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[3].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[4].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[4].listableImage + '">' : '';
	articleData += data.articles[4].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[4].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[4].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[4].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[4].listableDate ? '<li><time class="article-metadata__date">' + data.articles[4].listableDate + '</time></li>' : '';
	articleData += data.articles[4].linkableText ? '<li><h6>' + data.articles[4].linkableText + '</h6></li>' : '';
	articleData += data.articles[4].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[4].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[4].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[4].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[4].bookmarkText + '" data-label-bookmarked="' + data.articles[4].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[4].listableSummary ? data.articles[4].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[4].listableTopics) {
		for (var i = 0; i < data.articles[4].listableTopics.length; i++) {
			var getlistLink1 = data.articles[4].listableTopics[i].linkableUrl ? data.articles[4].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[4].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[5].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[5].listableImage + '">' : '';
	articleData += data.articles[5].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[5].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[5].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[5].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[5].listableDate ? '<li><time class="article-metadata__date">' + data.articles[5].listableDate + '</time></li>' : '';
	articleData += data.articles[5].linkableText ? '<li><h6>' + data.articles[5].linkableText + '</h6></li>' : '';
	articleData += data.articles[5].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[5].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[5].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[5].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[5].bookmarkText + '" data-label-bookmarked="' + data.articles[5].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[5].listableSummary ? data.articles[5].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[5].listableTopics) {
		for (var i = 0; i < data.articles[5].listableTopics.length; i++) {
			var getlistLink1 = data.articles[5].listableTopics[i].linkableUrl ? data.articles[5].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[5].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[6].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[6].listableImage + '">' : '';
	articleData += data.articles[6].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[6].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[6].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[6].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[6].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[6].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[6].listableDate ? '<li><time class="article-metadata__date">' + data.articles[6].listableDate + '</time></li>' : '';
	articleData += data.articles[6].linkableText ? '<li><h6>' + data.articles[6].linkableText + '</h6></li>' : '';
	articleData += data.articles[6].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[6].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[6].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[6].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[6].bookmarkText + '" data-label-bookmarked="' + data.articles[6].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[6].listableSummary ? data.articles[6].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[6].listableTopics) {
		for (var i = 0; i < data.articles[6].listableTopics.length; i++) {
			var getlistLink1 = data.articles[6].listableTopics[i].linkableUrl ? data.articles[6].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[6].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[7].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[7].listableImage + '">' : '';
	articleData += data.articles[7].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[7].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[7].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[7].listableTitle + '</a></h3>' : '';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="article-preview__byline">';
	articleData += data.articles[7].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[7].listableAuthorByLine + '</div>' : '';
	articleData += '<ul>'; 
	articleData += data.articles[7].listableDate ? '<li><time class="article-metadata__date">' + data.articles[7].listableDate + '</time></li>' : '';
	articleData += data.articles[7].linkableText ? '<li><h6>' + data.articles[7].linkableText + '</h6></li>' : '';
	articleData += data.articles[7].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[7].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[7].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[7].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[7].bookmarkText + '" data-label-bookmarked="' + data.articles[7].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
	articleData += '</ul>'; 
	articleData += '</div>'; 
	articleData += '</div>'; 
	articleData += '<div class="article-summary">' + data.articles[7].listableSummary ? data.articles[7].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[7].listableTopics) {
		for (var i = 0; i < data.articles[7].listableTopics.length; i++) {
			var getlistLink1 = data.articles[7].listableTopics[i].linkableUrl ? data.articles[7].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[7].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	articleData += '<section class="article-preview list-featured-article listViewCont">';
	articleData += data.articles[8].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[8].listableImage + '">' : '';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[8].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[8].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[8].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[8].bookmarkText + '" data-label-bookmarked="' + data.articles[8].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[8].listableDate ? '<li><time class="article-metadata__date">' + data.articles[8].listableDate + '</time></li>' : '';
	articleData += data.articles[8].linkableText ? '<li><h6>' + data.articles[8].linkableText + '</h6></li>' : '';
	articleData += data.articles[8].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="topic-featured-article__inner-wrapper">';
	articleData += data.articles[8].listableTitle ? '<h3 class="topic-featured-article__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[8].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[8].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[8].listableTitle + '</a></h3>' : '';
	articleData += data.articles[8].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[8].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">' + data.articles[8].listableSummary ? data.articles[8].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[8].listableTopics) {
		for (var i = 0; i < data.articles[8].listableTopics.length; i++) {
			var getlistLink1 = data.articles[8].listableTopics[i].linkableUrl ? data.articles[8].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getlistLink1 + '">' + data.articles[8].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';
	
	
	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-preview--small preview2 gridViewCont">';
	articleData += data.articles[0].listableImage ? '<img class="topic-featured-article__image2 hidden-lg" src="' + data.articles[0].listableImage + '">' : '';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[0].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[0].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[0].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[0].bookmarkText + '" data-label-bookmarked="' + data.articles[0].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[0].listableDate ? '<li><time class="article-metadata__date">' + data.articles[0].listableDate + '</time></li>' : '';
	articleData += data.articles[0].linkableText ? '<li><h6>' + data.articles[0].linkableText + '</h6></li>' : '';
	articleData += data.articles[0].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += data.articles[0].listableImage ? '<img class="topic-featured-article__image2 hidden-xs" src="' + data.articles[0].listableImage + '">' : '';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[0].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl0 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[0].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[0].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[0].listableTitle + '</a></h1>' : '';
	articleData += data.articles[0].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[0].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">' + data.articles[0].listableSummary ? data.articles[0].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[0].listableTopics) {
		for (var i = 0; i < data.articles[0].listableTopics.length; i++) {
			var getLink0 = data.articles[0].listableTopics[i].linkableUrl ? data.articles[0].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink0 + '">' + data.articles[0].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-preview--small mobview artheight gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[1].id + '" data-analytics="{"bookmark": "' + bookmarkInfo1 + '", "bookmark_title": "' + data.articles[1].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[1].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[1].bookmarkText + '" data-label-bookmarked="' + data.articles[1].bookmarkedText + '">' + bookmarkInfo1 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon1 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon1 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[1].listableDate ? '<li><time class="article-metadata__date">' + data.articles[1].listableDate + '</time></li>' : '';
	articleData += data.articles[1].linkableText ? '<li><h6>' + data.articles[1].linkableText + '</h6></li>' : '';
	articleData += data.articles[1].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#chart"></use></svg></span></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[1].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl1 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[1].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[1].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[1].listableTitle + '</a></h1>' : '';
	articleData += data.articles[1].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[1].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">' + data.articles[1].listableSummary ? data.articles[1].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[1].listableTopics) {
		for (var i = 0; i < data.articles[1].listableTopics.length; i++) {
			var getLink1 = data.articles[1].listableTopics[i].linkableUrl ? data.articles[1].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink1 + '">' + data.articles[1].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-preview--small artheight topics gridViewCont">';
	articleData += data.articles[2].linkableText ? '<h6>&nbsp;</h6>' : '';

	articleData += data.articles[2].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl2 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[2].listableTitle + '</a></h1>' : '';

	articleData += data.articles[3].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl3 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[3].listableTitle + '</a></h1>' : '';

	articleData += data.articles[4].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl4 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[4].listableTitle + '</a></h1>' : '';
	articleData += '</section>';
	articleData += '</div>';

	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-small-preview mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[5].id + '" data-analytics="{"bookmark": "' + bookmarkInfo5 + '", "bookmark_title": "' + data.articles[5].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[5].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[5].bookmarkText + '" data-label-bookmarked="' + data.articles[5].bookmarkedText + '">' + bookmarkInfo5 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon5 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon5 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[5].listableDate ? '<li><time class="article-metadata__date">' + data.articles[5].listableDate + '</time></li>' : '';
	articleData += data.articles[5].linkableText ? '<li><h6>' + data.articles[5].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[5].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl5 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[5].listableTitle + '</a></h1>' : '';
	articleData += data.articles[1].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[1].listableAuthorByLine + '</span>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[5].listableTopics) {
		for (var i = 0; i < data.articles[5].listableTopics.length; i++) {
			var getLink5 = data.articles[5].listableTopics[i].linkableUrl ? data.articles[5].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink5 + '">' + data.articles[5].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-preview--small artheight mobview mtop gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[6].id + '" data-analytics="{"bookmark": "' + bookmarkInfo6 + '", "bookmark_title": "' + data.articles[6].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[6].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[6].bookmarkText + '" data-label-bookmarked="' + data.articles[6].bookmarkedText + '">' + bookmarkInfo6 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon6 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon6 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[6].listableDate ? '<li><time class="article-metadata__date">' + data.articles[6].listableDate + '</time></li>' : '';
	articleData += data.articles[6].linkableText ? '<li><h6>' + data.articles[6].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper">';
	articleData += data.articles[6].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl6 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[6].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[6].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[6].listableTitle + '</a></h1>' : '';
	articleData += data.articles[6].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[6].listableAuthorByLine + '</span>' : '';
	articleData += '<div class="article-summary">' + data.articles[6].listableSummary ? data.articles[6].listableSummary : '' + '</div>';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[6].listableTopics) {
		for (var i = 0; i < data.articles[6].listableTopics.length; i++) {
			var getLink6 = data.articles[6].listableTopics[i].linkableUrl ? data.articles[6].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink6 + '">' + data.articles[6].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="article-preview article-small-preview sm-article sm-articles mtop gridViewCont">';
	articleData += '<section class="sm-article mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[7].id + '" data-analytics="{"bookmark": "' + bookmarkInfo7 + '", "bookmark_title": "' + data.articles[7].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[7].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[7].bookmarkText + '" data-label-bookmarked="' + data.articles[7].bookmarkedText + '">' + bookmarkInfo7 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon7 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon7 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[7].listableDate ? '<li><time class="article-metadata__date">' + data.articles[7].listableDate + '</time></li>' : '';
	articleData += data.articles[7].linkableText ? '<li><h6>' + data.articles[7].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[7].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl7 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[7].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[7].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[7].listableTitle + '</a></h1>' : '';
	articleData += data.articles[1].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[1].listableAuthorByLine + '</span>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[7].listableTopics) {
		for (var i = 0; i < data.articles[7].listableTopics.length; i++) {
			var getLink7 = data.articles[7].listableTopics[i].linkableUrl ? data.articles[7].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink7 + '">' + data.articles[7].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '<section class="sm-article mobview gridViewCont">';
	articleData += '<div class="article-metadata">';
	articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[8].id + '" data-analytics="{"bookmark": "' + bookmarkInfo8 + '", "bookmark_title": "' + data.articles[8].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[8].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[8].bookmarkText + '" data-label-bookmarked="' + data.articles[8].bookmarkedText + '">' + bookmarkInfo8 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon8 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon8 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
	articleData += '<ul>';
	articleData += data.articles[8].listableDate ? '<li><time class="article-metadata__date">' + data.articles[8].listableDate + '</time></li>' : '';
	articleData += data.articles[8].linkableText ? '<li><h6>' + data.articles[8].linkableText + '</h6></li>' : '';
	articleData += '</ul>';
	articleData += '</div>';
	articleData += '<div class="article-preview__inner-wrapper showarticle">';
	articleData += data.articles[8].listableTitle ? '<h1 class="article-preview__headline"><a href="' + linkableUrl8 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[8].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[8].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[8].listableTitle + '</a></h1>' : '';
	articleData += data.articles[1].listableAuthorByLine ? '<span class="article-preview__byline">' + data.articles[1].listableAuthorByLine + '</span>' : '';
	articleData += '</div>';
	articleData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[8].listableTopics) {
		for (var i = 0; i < data.articles[8].listableTopics.length; i++) {
			var getLink8 = data.articles[8].listableTopics[i].linkableUrl ? data.articles[8].listableTopics[i].linkableUrl : '#';
			articleData += '<a href="' + getLink8 + '">' + data.articles[8].listableTopics[i].linkableText + '</a>';
		}
	}
	articleData += '</div>';
	articleData += '</section>';

	articleData += '</section>';
	articleData += '</div>';

	articleData += '<input type="hidden" class="getPaginationNum" data-pageSize="' + data.loadMore.pageSize + '" data-pageNo="' + data.loadMore.pageNo + '" data-loadurl="' + data.loadMore.loadMoreLinkUrl + '" data-taxonomyIds="' + data.loadMore.taxonomyIds + '" />';

	return articleData;
}

$(function () {
	$('.view-mode').on('click', '.icon-tile-view', function(e){
		e.preventDefault();
		$('.view-mode li').removeClass('selected');
		$(this).parents('li').addClass('selected');
		if($('.personalisationhome') && $('.personalisationhome').length){
			$('.personalisationhome').removeClass('listView').addClass('gridView');
		}
	});
	$('.view-mode').on('click', '.icon-list-view', function(e){
		e.preventDefault();
		$('.view-mode li').removeClass('selected');
		$(this).parents('li').addClass('selected');
		if($('.personalisationhome') && $('.personalisationhome').length){
			$('.personalisationhome').removeClass('gridView').addClass('listView');
		}
	});
	var getLayoutInfo = $('#getLayoutInfo').val(),
	    layout1 = true,
	    loadLayoutData = '',
	    getLiIdx,
	    getArticleIdx;
	if (typeof loadPreferanceId !== "undefined") {
		var loadDynData = loadPreferanceId["Sections"].length < loadPreferanceId.DefaultSectionLoadCount ? loadPreferanceId["Sections"].length : loadPreferanceId.DefaultSectionLoadCount,
		    getArticalIdx = 0,
		    postedId = window.location.href.split('#')[1];

		if (postedId != '' && postedId != undefined) {
			for (var i = 0; i < loadPreferanceId["Sections"].length; i++) {
				if (loadPreferanceId["Sections"][i]["ChannelId"] == postedId) {
					getArticalIdx = i + 1;
					break;
				}
			}
			loadDynData = getArticalIdx;
		}
		getLiIdx = loadDynData;
		getArticleIdx = loadDynData;
		for (var i = 0; i < loadDynData; i++) {
			var setId = loadPreferanceId["Sections"];
			if (setId.length) {
				(function (idx) {
					if (idx < loadDynData) {
						$.ajax({
							url: '/api/articlesearch',
							data: JSON.stringify({ 'TaxonomyIds': setId[idx]["TaxonomyIds"], 'ChannelId': setId[idx]["ChannelId"], 'PageNo': 1, 'PageSize': 9 }),
							dataType: 'json',
							contentType: "application/json",
							type: 'POST',
							cache: false,
							async: false,
							beforeSend: function beforeSend() {
								$('.spinnerIcon').removeClass('hidespin');
							},
							success: function success(data) {
								if (data.articles && typeof data.articles === "object" && data.articles.length >= 9) {
									if (layout1) {
										layout1 = false;
										loadLayoutData = loadLayoutOneData(data, idx);
										$('.spinnerIcon').addClass('hidespin');
										$('.personalisationPan').append(loadLayoutData);
										window.findTooltips();
									} else {
										layout1 = true;
										loadLayoutData = loadLayoutTwoData(data, idx);
										$('.spinnerIcon').addClass('hidespin');
										$('.personalisationPan').append(loadLayoutData);
										window.findTooltips();
									}
								}
							},
							error: function error(xhr, errorType, _error) {
								console.log('err ' + _error);
							}
						});
					}
				})(i);
			}
		}
	}
	$('.personalisationPan').on('click', '.loadmore', function () {
		var $this = $(this),
		    eachstoryMpan = $this.closest('.eachstoryMpan'),
		    eachstory = eachstoryMpan.find('.eachstory'),
		    eachstoryId = eachstory.attr('id'),
		    layoutCls = eachstory.attr('class'),
			channelId = $this.closest('.eachstoryMpan').prev('.latestSubject').attr('id'),
		    loadLayoutData;

		var layout = layoutCls.indexOf('layout1') !== -1 ? 'layout1' : 'layout2';
		var setId = loadPreferanceId["Sections"],
		    sendtaxonomyIdsArr = $this.closest('.eachstoryMpan').find('.getPaginationNum').attr('data-taxonomyIds').split(',');

		$.ajax({
			url: $this.closest('.eachstoryMpan').find('.getPaginationNum').attr('data-loadurl'),
			dataType: 'json',
			type: 'POST',
			data: JSON.stringify({ 'TaxonomyIds': sendtaxonomyIdsArr, 'ChannelId': channelId, 'PageNo': $this.closest('.eachstoryMpan').find('.getPaginationNum').attr('data-pageNo'), 'PageSize': $this.closest('.eachstoryMpan').find('.getPaginationNum').attr('data-pageSize') }),
			contentType: "application/json",
			success: function success(data) {
				if (data.articles && typeof data.articles === "object" && data.articles.length >= 9) {
					$this.closest('.eachstoryMpan').find('.getPaginationNum').attr({ 'data-taxonomyIds': data.loadMore.taxonomyIds, 'data-loadurl': data.loadMore.loadMoreLinkUrl, 'data-pageNo': data.loadMore.pageNo, 'data-pageSize': data.loadMore.pageSize });
					if (layout == 'layout1') {
						loadLayoutData = createLayoutInner1(data);
						$(eachstory).append(loadLayoutData);
						window.findTooltips();
						if (data.loadMore && !data.loadMore.displayLoadMore) {
							$this.closest('.eachstoryMpan').find('.loadmore').css('display', 'none');
						}
					} else {
						loadLayoutData = createLayoutInner2(data);
						$(eachstory).append(loadLayoutData);
						window.findTooltips();
						if (data.loadMore && !data.loadMore.displayLoadMore) {
							$this.closest('.eachstoryMpan').find('.loadmore').css('display', 'none');
						}
					}
				}
			},
			error: function error(xhr, errorType, _error2) {
				console.log('err ' + _error2);
			}
		});
	});

	var eachstoryLength = typeof loadPreferanceId !== 'undefined' && loadPreferanceId.DefaultSectionLoadCount ? loadPreferanceId.DefaultSectionLoadCount : 0;
	$(window).scroll(function () {
		var eachstoryMpan = $('.personalisationPan .eachstoryMpan'),
		    eachstoryMpanLast = eachstoryMpan.last(),
		    layoutCls = eachstoryMpan.find('.eachstory').attr('class'),
		    contentHei = $('.personalisationPan').height(),
		    loadsection,
			getChannelId,
		    texonomyId;

		if ($(window).scrollTop() > contentHei - 400) {
			var getscrollData;

			if (typeof loadPreferanceId !== "undefined") {
				if (getArticleIdx < loadPreferanceId["Sections"].length) {
					getLiIdx = getArticleIdx;
					loadsection = getArticleIdx;
					texonomyId = loadPreferanceId["Sections"][loadsection]["TaxonomyIds"];
					getChannelId = loadPreferanceId["Sections"][loadsection]["ChannelId"];
					getArticleIdx++;
				} else {
					return;
				}
			} else {
				return;
			}

			$.ajax({
				url: '/api/articlesearch',
				data: JSON.stringify({ 'TaxonomyIds': texonomyId, 'ChannelId': getChannelId, 'PageNo': 1, 'PageSize': 9 }),
				type: 'POST',
				contentType: "application/json",
				cache: false,
				async: false,
				dataType: 'json',
				beforeSend: function beforeSend() {
					$('.spinnerIcon').removeClass('hidespin');
				},
				success: function success(data) {
					if (data.articles && typeof data.articles === "object" && data.articles.length >= 9) {
						if ($('.eachstoryMpan', '.personalisationPan').length % 2 == 0) {
							getscrollData = loadLayoutOneData(data, loadsection);
							$('.spinnerIcon').addClass('hidespin');
							$('.personalisationPan').append(getscrollData);
							window.findTooltips();
						} else {
							getscrollData = loadLayoutTwoData(data, loadsection);
							$('.spinnerIcon').addClass('hidespin');
							$('.personalisationPan').append(getscrollData);
							window.findTooltips();
						}
					}
				},
				error: function error(xhr, errorType, _error3) {
					console.log('xhr ' + xhr + ' errorType ' + errorType + ' error ' + _error3);
				}
			});
		}
	});

	$('.main-menu__hoverable a.myviewLink').click(function (e) {
		if ($('#hdnMyViewPage') && $('#hdnMyViewPage').val() == "true") {
			e.preventDefault();
			var $this = $(this),
			    name = $this.attr('name'),
			    getPos = $('#' + name).position(),
			    latestSubject = $('#' + name).closest('.eachstoryMpan').prev('.latestSubject'),
			    subjectHei = latestSubject.height(),
			    allstoriesLen = $('.personalisationPan .eachstoryMpan').length,
			    liIdx = $this.closest('li').index();
			setTimeout(function () {
				if ($('.js-menu-toggle-button, .js-full-menu-toggle').hasClass('is-active')) {
					$('.js-menu-toggle-button, .js-full-menu-toggle').removeClass('is-active');
				}
			}, 5);

			if (typeof loadPreferanceId !== 'undefined' && $('#' + name) && $('#' + name).length) {
				$(window).scrollTop(getPos.top - subjectHei * 3);
			} else {
				if (typeof loadPreferanceId !== "undefined") {
					getLiIdx = getArticleIdx;
					for (var i = getLiIdx; i <= liIdx; i++) {
						var setId = loadPreferanceId["Sections"];
						getArticleIdx++;
						(function (idx) {
							$.ajax({
								url: '/api/articlesearch',
								dataType: 'json',
								contentType: "application/json",
								data: JSON.stringify({ 'TaxonomyIds': loadPreferanceId["Sections"][idx]["TaxonomyIds"], 'ChannelId': loadPreferanceId["Sections"][idx]["ChannelId"], 'PageNo': 1, 'PageSize': 9 }),
								type: 'POST',
								cache: false,
								async: false,
								beforeSend: function beforeSend() {
									$('.spinnerIcon').removeClass('hidespin');
								},
								success: function success(data) {
									if (data.articles && typeof data.articles === "object" && data.articles.length >= 9) {
										if ($('.eachstoryMpan', '.personalisationPan').length % 2 == 0) {
											loadLayoutData = loadLayoutOneData(data, idx);
											$('.personalisationPan').append(loadLayoutData);
											window.findTooltips();
										} else {
											loadLayoutData = loadLayoutTwoData(data, idx);
											$('.personalisationPan').append(loadLayoutData);
											window.findTooltips();
										}
									}
								},
								error: function error(xhr, errorType, _error4) {
									console.log('err ' + _error4);
								},
								complete: function complete(xhr, status) {
									if (status == "success" && $('#' + name).length) {
										setTimeout(function () {
											var getlatestPos = $('#' + name).position();
											if (getlatestPos) {
												$('.spinnerIcon').addClass('hidespin');
												$(window).scrollTop(getlatestPos.top - 120);
											}
										}, 5);
									}
								}
							});
						})(i);
					}
				}
			}
		} else {
			if ($('#validatePreference').val() != 1) {
				e.preventDefault();
				var $this = $(this),
				    href = $this.attr('href'),
				    id = $this.attr('name');
				window.location.href = href + '#' + id;
			}
		}
	});
	
	var latestSubject = $('.latestSubject');
	if (window.matchMedia("(min-width: 768px)").matches) {
		for(var i = 0; i < latestSubject.length; i++){
			var getFullwidth = $(latestSubject[i]).width(), frEditviewWid = $(latestSubject[i]).find('.frEditview').width(),
			setEditViewWidth = Math.ceil(frEditviewWid / getFullwidth * 100), setLatestSubWid = 100 - setEditViewWidth;
			$(latestSubject[i]).find('.frEditview').css('width', setEditViewWidth +'%');
			$(latestSubject[i]).find('.fllatestSub').css('width', setLatestSubWid - 2 + '%');
		}
	}
});