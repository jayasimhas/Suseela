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
		bookmarkInfo3 = data.articles[3].isArticleBookmarked ? data.articles[3].bookmarkedText : data.articles[3].bookmarkText,
	    bookmarkInfo4 = data.articles[4].isArticleBookmarked ? data.articles[4].bookmarkedText : data.articles[4].bookmarkText,
	    bookmarkInfo5 = data.articles[5].isArticleBookmarked ? data.articles[5].bookmarkedText : data.articles[5].bookmarkText,
	    bookmarkInfo6 = data.articles[6].isArticleBookmarked ? data.articles[6].bookmarkedText : data.articles[6].bookmarkText,
	    bookmarkInfo7 = data.articles[7].isArticleBookmarked ? data.articles[7].bookmarkedText : data.articles[7].bookmarkText,
	    bookmarkInfo8 = data.articles[8].isArticleBookmarked ? data.articles[8].bookmarkedText : data.articles[8].bookmarkText,
	    fbookmarkIcon0 = data.articles[0].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon0 = data.articles[0].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon1 = data.articles[1].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon1 = data.articles[1].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon2 = data.articles[2].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon2 = data.articles[2].isArticleBookmarked ? '' : 'is-visible',
		fbookmarkIcon3 = data.articles[3].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon3 = data.articles[3].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon4 = data.articles[4].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon4 = data.articles[4].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon5 = data.articles[5].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon5 = data.articles[5].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon6 = data.articles[6].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon6 = data.articles[6].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon7 = data.articles[7].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon7 = data.articles[7].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon8 = data.articles[8].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon8 = data.articles[8].isArticleBookmarked ? '' : 'is-visible';
		sponsored_cont0 = data.articles[0].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont1 = data.articles[1].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont2 = data.articles[2].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont3 = data.articles[3].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont4 = data.articles[4].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont5 = data.articles[5].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont6 = data.articles[6].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont7 = data.articles[7].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont8 = data.articles[8].isSonsoredBy ? 'sponsored_cont' : '';

	var articleData = ''; 
	articleData += getListViewData(0, data, linkableUrl0, bookmarkInfo0, fbookmarkIcon0, sbookmarkIcon0);
	articleData += getListViewData(1, data, linkableUrl1, bookmarkInfo1, fbookmarkIcon1, sbookmarkIcon1);
	articleData += getListViewData(2, data, linkableUrl2, bookmarkInfo2, fbookmarkIcon2, sbookmarkIcon2);
	articleData += getListViewData(3, data, linkableUrl3, bookmarkInfo3, fbookmarkIcon3, sbookmarkIcon3);
	articleData += getListViewData(4, data, linkableUrl4, bookmarkInfo4, fbookmarkIcon4, sbookmarkIcon4);
	articleData += getListViewData(5, data, linkableUrl5, bookmarkInfo5, fbookmarkIcon5, sbookmarkIcon5);
	articleData += getListViewData(6, data, linkableUrl6, bookmarkInfo6, fbookmarkIcon6, sbookmarkIcon6);
	articleData += getListViewData(7, data, linkableUrl7, bookmarkInfo7, fbookmarkIcon7, sbookmarkIcon7);
	articleData += getListViewData(8, data, linkableUrl8, bookmarkInfo8, fbookmarkIcon8, sbookmarkIcon8);
	
	articleData += '<section class="article-preview topic-featured-article gridViewCont '+sponsored_cont0+'">';
	articleData += data.articles[0].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[0].listableImage + '">' : '';
	articleData += '<div class="article-metadata">';
	if(data.articles[0].isSonsoredBy){
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[0].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[0].sponsoredByLogo+'"></li></ul>';
	}	
	else{
		articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[0].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[0].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[0].isArticleBookmarked + '"><span class="action-flag__label js-bookmark-label" data-label-bookmark="' + data.articles[0].bookmarkText + '" data-label-bookmarked="' + data.articles[0].bookmarkedText + '">' + bookmarkInfo0 + '</span><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
		articleData += '<ul>';
		articleData += data.articles[0].listableDate ? '<li><time class="article-metadata__date">' + data.articles[0].listableDate + '</time></li>' : '';
		articleData += data.articles[0].linkableText ? '<li><h6>' + data.articles[0].linkableText + '</h6></li>' : '';
		articleData += data.articles[0].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type" style="width: 0px; height: 0px;"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="'+ data.articles[0].listableType + '"></use></svg><img src="'+data.articles[0].listableType+'" width="25" /></span></li>' : '';
		articleData += '</ul>';
	} 
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
	articleData += '<section class="article-preview article-preview--small mobview gridViewCont '+sponsored_cont1+'">';
	articleData += '<div class="article-metadata">';
	
	if(data.articles[1].isSonsoredBy){
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[1].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[1].sponsoredByLogo+'"></li></ul>';
	}
	else{
		articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[1].id + '" data-analytics="{"bookmark": "' + bookmarkInfo1 + '", "bookmark_title": "' + data.articles[1].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[1].isArticleBookmarked + '"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon1 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon1 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
		articleData += '<ul>';
		articleData += data.articles[1].listableDate ? '<li><time class="article-metadata__date">' + data.articles[1].listableDate + '</time></li>' : '';
		articleData += data.articles[1].linkableText ? '<li><h6>' + data.articles[1].linkableText + '</h6></li>' : '';
		articleData += data.articles[1].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type" style="width: 0px; height: 0px;"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#'+data.articles[1].listableType+'"></use></svg><img src="'+data.articles[1].listableType+'" width="25" /></span></li>' : '';
		articleData += '</ul>';
	} 
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

	articleData += '<section class="article-preview article-preview--small mobview gridViewCont '+sponsored_cont2+'">';
	articleData += '<div class="article-metadata">';
	
	if(data.articles[2].isSonsoredBy){
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[2].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[2].sponsoredByLogo+'"></li></ul>';
	}
	else{
		articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[2].id + '" data-analytics="{"bookmark": "' + bookmarkInfo2 + '", "bookmark_title": "' + data.articles[2].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[2].isArticleBookmarked + '"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon2 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon2 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
		articleData += '<ul>';
		articleData += data.articles[2].listableDate ? '<li><time class="article-metadata__date">' + data.articles[2].listableDate + '</time></li>' : '';
		articleData += data.articles[2].linkableText ? '<li><h6>' + data.articles[2].linkableText + '</h6></li>' : '';
		articleData += data.articles[2].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type" style="width: 0px; height: 0px;"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#'+data.articles[2].listableType+'"></use></svg><img src="'+data.articles[2].listableType+'" width="25" /></span></li>' : '';
		articleData += '</ul>';
	}
	
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

	articleData += '<section class="article-preview article-preview--small topics gridViewCont '+sponsored_cont3+'">';
	//articleData += data.articles[3].linkableText ? '<h6>&nbsp;</h6>' : '';
	
	if(data.articles[3].isSonsoredBy){
		articleData += '<div class="article-metadata sponsored_cont">';
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[3].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[3].sponsoredByLogo+'"></li></ul>';
		articleData += data.articles[3].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl3 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[3].listableTitle + '</a></h1>' : '';
		articleData += '</div>';
	}
	else{
		articleData += '<div class="article-metadata nosponsored_cont">';
		articleData += data.articles[3].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl3 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[3].listableTitle + '</a></h1>' : '';
		articleData += '</div>';
	}
	if(data.articles[4].isSonsoredBy){
		articleData += '<div class="article-metadata sponsored_cont">';
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[4].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[4].sponsoredByLogo+'"></li></ul>';
		articleData += data.articles[4].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl4 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[4].listableTitle + '</a></h1>' : '';
		articleData += '</div>';
	}
	else{
		articleData += '<div class="article-metadata nosponsored_cont">';
		articleData += data.articles[4].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl3 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[4].listableTitle + '</a></h1>' : '';
		articleData += '</div>';
	}

	if(data.articles[5].isSonsoredBy){
		articleData += '<div class="article-metadata sponsored_cont">';
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[5].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[5].sponsoredByLogo+'"></li></ul>';
		articleData += data.articles[5].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl5 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[5].listableTitle + '</a></h1>' : '';
		articleData += '</div>';
	}
	else{
		articleData += '<div class="article-metadata nosponsored_cont">';
		articleData += data.articles[5].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl3 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[5].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[5].listableTitle + '</a></h1>' : '';
		articleData += '</div>';
	}

	articleData += '</section>';
	articleData += '</div>';

	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-small-preview mobview gridViewCont '+sponsored_cont6+'">';
	articleData += '<div class="article-metadata">';
	
	if(data.articles[6].isSonsoredBy){
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[6].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[6].sponsoredByLogo+'"></li></ul>';
	}
	else{
		articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[6].id + '" data-analytics="{"bookmark": "' + bookmarkInfo6 + '", "bookmark_title": "' + data.articles[6].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[6].isArticleBookmarked + '"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked  ' + fbookmarkIcon6 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon6 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
		articleData += '<ul>';
		articleData += data.articles[6].listableDate ? '<li><time class="article-metadata__date">' + data.articles[6].listableDate + '</time></li>' : '';
		articleData += data.articles[6].linkableText ? '<li><h6>' + data.articles[6].linkableText + '</h6></li>' : '';
		articleData += '</ul>';
	}
	
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

	articleData += '<section class="article-preview article-small-preview mobview gridViewCont '+sponsored_cont7+'">';
	articleData += '<div class="article-metadata">';
	if(data.articles[7].isSonsoredBy){
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[7].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[7].sponsoredByLogo+'"></li></ul>';
	}
	else{
		articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[7].id + '" data-analytics="{"bookmark": "' + bookmarkInfo7 + '", "bookmark_title": "' + data.articles[7].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[7].isArticleBookmarked + '"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon7 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon7 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
		articleData += '<ul>';
		articleData += data.articles[7].listableDate ? '<li><time class="article-metadata__date">' + data.articles[7].listableDate + '</time></li>' : '';
		articleData += data.articles[7].linkableText ? '<li><h6>' + data.articles[7].linkableText + '</h6></li>' : '';
		articleData += '</ul>';
	}
	
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

	articleData += '<section class="article-preview article-small-preview mobview gridViewCont '+sponsored_cont8+'">';
	articleData += '<div class="article-metadata">';
	
	if(data.articles[8].isSonsoredBy){
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[8].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[8].sponsoredByLogo+'"></li></ul>';
	}
	else{
		articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[8].id + '" data-analytics="{"bookmark": "' + bookmarkInfo8 + '", "bookmark_title": "' + data.articles[8].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[8].isArticleBookmarked + '"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon8 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon8 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
		articleData += '<ul>';
		articleData += data.articles[8].listableDate ? '<li><time class="article-metadata__date">' + data.articles[8].listableDate + '</time></li>' : '';
		articleData += data.articles[8].linkableText ? '<li><h6>' + data.articles[8].linkableText + '</h6></li>' : '';
		articleData += '</ul>';
	}
	
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

function getListViewData(idx, data, linkableUrl, bookmarkInfo, fbookmarkIcon, sbookmarkIcon){
	var sectionData = '';
	sectionData += '<section class="article-preview list-featured-article listViewCont">';
	sectionData += '<div class="topic-article-image_pan">';
	sectionData += data.articles[idx].listableImage ? '<img class="topic-featured-article__image" src="' + data.articles[idx].listableImage + '">' : '';
	sectionData += '</div>';
	sectionData += data.articles[idx].listableTitle ? '<div class="topic-article-rig_pan"><h3 class="topic-featured-article__headline"><a href="' + linkableUrl + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[idx].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[idx].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[idx].listableTitle + '</a></h3>' : '';
	sectionData += '<div class="topic-featured-article__inner-wrapper">';
	sectionData += '<div class="article-metadata">';
	
	if(data.articles[idx].isSonsoredBy){
		sectionData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[idx].sponsoredByTitle+'</time></li>';
		sectionData += '<li><img src="'+data.articles[idx].sponsoredByLogo+'"></li></ul>';
	}
	else{
		sectionData += '<div class="article-preview__byline">';
		sectionData += data.articles[idx].listableAuthorByLine ? '<div class="authorTitle">' + data.articles[idx].listableAuthorByLine + '</div>' : '';
		sectionData += '<ul>'; 
		sectionData += data.articles[idx].listableDate ? '<li><time class="article-metadata__date">' + data.articles[idx].listableDate + '</time></li>' : '';
		sectionData += data.articles[idx].linkableText ? '<li><h6>' + data.articles[idx].linkableText + '</h6></li>' : '';
		sectionData += data.articles[idx].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type" style="width: 0px; height: 0px;"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#'+data.articles[idx].listableType+'"></use></svg><img src="'+data.articles[idx].listableType+'" width="25" /></span></li>' : '';
		sectionData += '<li><div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[idx].id + '" data-analytics="{"bookmark": "' + bookmarkInfo + '", "bookmark_title": "' + data.articles[idx].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[idx].isArticleBookmarked + '"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div></li>';
		sectionData += '</ul>'; 
		sectionData += '</div>'; 
	}
	sectionData += '</div>'; 
	sectionData += data.articles[idx].listableSummary ? '<div class="article-summary">' +  data.articles[idx].listableSummary + '</div>' : '';
	sectionData += '</div>';
	sectionData += '<div class="article-preview__tags bar-separated-link-list">';
	if (data.articles[idx].listableTopics) {
		for (var i = 0; i < data.articles[idx].listableTopics.length; i++) {
			var getlistLink1 = data.articles[idx].listableTopics[i].linkableUrl ? data.articles[idx].listableTopics[i].linkableUrl : '#';
			sectionData += '<a href="' + getlistLink1 + '">' + data.articles[idx].listableTopics[i].linkableText + '</a>';
		}
	}
	sectionData += '</div>';
	sectionData += '</div>';
	sectionData += '</section>';
	
	return sectionData;
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
		bookmarkInfo2 = data.articles[2].isArticleBookmarked ? data.articles[2].bookmarkedText : data.articles[2].bookmarkText,
	    bookmarkInfo3 = data.articles[3].isArticleBookmarked ? data.articles[3].bookmarkedText : data.articles[3].bookmarkText,
		bookmarkInfo4 = data.articles[4].isArticleBookmarked ? data.articles[4].bookmarkedText : data.articles[4].bookmarkText,
	    bookmarkInfo5 = data.articles[5].isArticleBookmarked ? data.articles[5].bookmarkedText : data.articles[5].bookmarkText,
	    bookmarkInfo6 = data.articles[6].isArticleBookmarked ? data.articles[6].bookmarkedText : data.articles[6].bookmarkText,
	    bookmarkInfo7 = data.articles[7].isArticleBookmarked ? data.articles[7].bookmarkedText : data.articles[7].bookmarkText,
	    bookmarkInfo8 = data.articles[8].isArticleBookmarked ? data.articles[8].bookmarkedText : data.articles[8].bookmarkText,
	    fbookmarkIcon0 = data.articles[0].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon0 = data.articles[0].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon1 = data.articles[1].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon1 = data.articles[1].isArticleBookmarked ? '' : 'is-visible',
		fbookmarkIcon2 = data.articles[2].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon2 = data.articles[2].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon3 = data.articles[3].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon3 = data.articles[3].isArticleBookmarked ? '' : 'is-visible',
		fbookmarkIcon4 = data.articles[4].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon4 = data.articles[4].isArticleBookmarked ? '' : 'is-visible',
		fbookmarkIcon5 = data.articles[5].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon5 = data.articles[5].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon6 = data.articles[6].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon6 = data.articles[6].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon7 = data.articles[7].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon7 = data.articles[7].isArticleBookmarked ? '' : 'is-visible',
	    fbookmarkIcon8 = data.articles[8].isArticleBookmarked ? 'is-visible' : '',
	    sbookmarkIcon8 = data.articles[8].isArticleBookmarked ? '' : 'is-visible';
		sponsored_cont0 = data.articles[0].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont1 = data.articles[1].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont2 = data.articles[2].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont3 = data.articles[3].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont4 = data.articles[4].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont5 = data.articles[5].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont6 = data.articles[6].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont7 = data.articles[7].isSonsoredBy ? 'sponsored_cont' : '';
		sponsored_cont8 = data.articles[8].isSonsoredBy ? 'sponsored_cont' : '';
		
	var articleData = '';
	
	articleData += getListViewData(0, data, linkableUrl0, bookmarkInfo0, fbookmarkIcon0, sbookmarkIcon0);
	articleData += getListViewData(1, data, linkableUrl1, bookmarkInfo1, fbookmarkIcon1, sbookmarkIcon1);
	articleData += getListViewData(2, data, linkableUrl2, bookmarkInfo2, fbookmarkIcon2, sbookmarkIcon2);
	articleData += getListViewData(3, data, linkableUrl3, bookmarkInfo3, fbookmarkIcon3, sbookmarkIcon3);
	articleData += getListViewData(4, data, linkableUrl4, bookmarkInfo4, fbookmarkIcon4, sbookmarkIcon4);
	articleData += getListViewData(5, data, linkableUrl5, bookmarkInfo5, fbookmarkIcon5, sbookmarkIcon5);
	articleData += getListViewData(6, data, linkableUrl6, bookmarkInfo6, fbookmarkIcon6, sbookmarkIcon6);
	articleData += getListViewData(7, data, linkableUrl7, bookmarkInfo7, fbookmarkIcon7, sbookmarkIcon7);
	articleData += getListViewData(8, data, linkableUrl8, bookmarkInfo8, fbookmarkIcon8, sbookmarkIcon8);
	
	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-preview--small preview2 gridViewCont '+sponsored_cont0+'">';
	articleData += data.articles[0].listableImage ? '<img class="topic-featured-article__image2 hidden-lg" src="' + data.articles[0].listableImage + '">' : '';
	articleData += '<div class="article-metadata">';
	
	if(data.articles[0].isSonsoredBy){
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[0].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[0].sponsoredByLogo+'"></li></ul>';
	}
	else{
		articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[0].id + '" data-analytics="{"bookmark": "' + bookmarkInfo0 + '", "bookmark_title": "' + data.articles[0].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[0].isArticleBookmarked + '"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon0 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
		articleData += '<ul>';
		articleData += data.articles[0].listableDate ? '<li><time class="article-metadata__date">' + data.articles[0].listableDate + '</time></li>' : '';
		articleData += data.articles[0].linkableText ? '<li><h6>' + data.articles[0].linkableText + '</h6></li>' : '';
		articleData += data.articles[0].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type" style="width: 0px; height: 0px;"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#'+data.articles[0].listableType+'"></use></svg><img src="'+data.articles[0].listableType+'" width="25" /></span></li>' : '';
		articleData += '</ul>';
	}
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

	articleData += '<section class="article-preview article-preview--small mobview artheight gridViewCont '+sponsored_cont1+'">';
	articleData += '<div class="article-metadata">';
	
	if(data.articles[1].isSonsoredBy){
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[1].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[1].sponsoredByLogo+'"></li></ul>';
	}
	else{
		articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[1].id + '" data-analytics="{"bookmark": "' + bookmarkInfo1 + '", "bookmark_title": "' + data.articles[1].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[1].isArticleBookmarked + '"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon1 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon1 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
		articleData += '<ul>';
		articleData += data.articles[1].listableDate ? '<li><time class="article-metadata__date">' + data.articles[1].listableDate + '</time></li>' : '';
		articleData += data.articles[1].linkableText ? '<li><h6>' + data.articles[1].linkableText + '</h6></li>' : '';
		articleData += data.articles[1].listableType ? '<li><span class="js-toggle-tooltip" data-tooltip-text="This article includes data."><svg class="article-metadata__media-type" style="width: 0px; height: 0px;"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#'+data.articles[1].listableType+'"></use></svg><img src="'+data.articles[1].listableType+'" width="25" /></span></li>' : '';
		articleData += '</ul>';
	}
	
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
	//articleData += data.articles[2].linkableText ? '<h6>&nbsp;</h6>' : '';
	
	
	if(data.articles[2].isSonsoredBy){
		articleData += '<div class="article-metadata sponsored_cont">';
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[2].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[2].sponsoredByLogo+'"></li></ul>';
		articleData += data.articles[2].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl2 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[2].listableTitle + '</a></h1>' : '';
		articleData += '</div>';
	}
	else{
		articleData += '<div class="article-metadata nosponsored_cont">';
		articleData += data.articles[2].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl2 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[2].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[2].listableTitle + '</a></h1>' : '';
		articleData += '</div>';
	}
	
	if(data.articles[3].isSonsoredBy){
		articleData += '<div class="article-metadata sponsored_cont">';
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[3].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[3].sponsoredByLogo+'"></li></ul>';
		articleData += data.articles[3].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl3 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[3].listableTitle + '</a></h1>' : '';
		articleData += '</div>';
	}
	else{
		articleData += '<div class="article-metadata nosponsored_cont">';
		articleData += data.articles[3].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl3 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[3].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[3].listableTitle + '</a></h1>' : '';
		articleData += '</div>';
	}
	
	
	if(data.articles[4].isSonsoredBy){
		articleData += '<div class="article-metadata sponsored_cont">';
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[4].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[4].sponsoredByLogo+'"></li></ul>';
		articleData += data.articles[4].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl4 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[4].listableTitle + '</a></h1>' : '';
		articleData += '</div>';
	}
	else{
		articleData += '<div class="article-metadata nosponsored_cont">';
		articleData += data.articles[4].listableTitle ? '<h1 class="article-preview_rheadline"><a href="' + linkableUrl4 + '" class="click-utag" data-info=\'{"event_name":"article_click_through","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"My View Page Articles","ga_eventAction":"' + analytics_data["publication"] + '","ga_eventLabel":"' + data.articles[4].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '"}\'>' + data.articles[4].listableTitle + '</a></h1>' : '';
		articleData += '</div>';
	}
	articleData += '</section>';
	articleData += '</div>';

	articleData += '<div class="latest-news__articles">';
	articleData += '<section class="article-preview article-small-preview mobview gridViewCont '+sponsored_cont5+'">';
	articleData += '<div class="article-metadata">';
	
	if(data.articles[5].isSonsoredBy){
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[5].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[5].sponsoredByLogo+'"></li></ul>';
	}
	else{
		articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[5].id + '" data-analytics="{"bookmark": "' + bookmarkInfo5 + '", "bookmark_title": "' + data.articles[5].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[5].isArticleBookmarked + '"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon5 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon5 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
		articleData += '<ul>';
		articleData += data.articles[5].listableDate ? '<li><time class="article-metadata__date">' + data.articles[5].listableDate + '</time></li>' : '';
		articleData += data.articles[5].linkableText ? '<li><h6>' + data.articles[5].linkableText + '</h6></li>' : '';
		articleData += '</ul>';
	}
	
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

	articleData += '<section class="article-preview article-preview--small artheight mobview mtop gridViewCont '+sponsored_cont6+'">';
	articleData += '<div class="article-metadata">';
	
	if(data.articles[6].isSonsoredBy){
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[6].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[6].sponsoredByLogo+'"></li></ul>';
	}
	else{
		articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[6].id + '" data-analytics="{"bookmark": "' + bookmarkInfo6 + '", "bookmark_title": "' + data.articles[6].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[6].isArticleBookmarked + '"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon6 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon6 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
		articleData += '<ul>';
		articleData += data.articles[6].listableDate ? '<li><time class="article-metadata__date">' + data.articles[6].listableDate + '</time></li>' : '';
		articleData += data.articles[6].linkableText ? '<li><h6>' + data.articles[6].linkableText + '</h6></li>' : '';
		articleData += '</ul>';
	}
	
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
	articleData += '<section class="sm-article mobview gridViewCont '+sponsored_cont7+'">';
	articleData += '<div class="article-metadata">';
	
	if(data.articles[7].isSonsoredBy){
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[7].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[7].sponsoredByLogo+'"></li></ul>';
	}
	else{
		articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[7].id + '" data-analytics="{"bookmark": "' + bookmarkInfo7 + '", "bookmark_title": "' + data.articles[7].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[7].isArticleBookmarked + '"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon7 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon7 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
		articleData += '<ul>';
		articleData += data.articles[7].listableDate ? '<li><time class="article-metadata__date">' + data.articles[7].listableDate + '</time></li>' : '';
		articleData += data.articles[7].linkableText ? '<li><h6>' + data.articles[7].linkableText + '</h6></li>' : '';
		articleData += '</ul>';
	}
	
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

	articleData += '<section class="sm-article mobview gridViewCont '+sponsored_cont8+'">';
	articleData += '<div class="article-metadata">';
	
	if(data.articles[8].isSonsoredBy){
		articleData += '<ul><li><time class="article-metadata__date sponsored_title">'+data.articles[8].sponsoredByTitle+'</time></li>';
		articleData += '<li><img src="'+data.articles[8].sponsoredByLogo+'"></li></ul>';
	}
	else{
		articleData += '<div class="action-flag article-preview__bookmarker pop-out__trigger js-bookmark-article" data-pop-out-type="sign-in" data-pop-out-align="right" data-bookmark-id="' + data.articles[8].id + '" data-analytics="{"bookmark": "' + bookmarkInfo8 + '", "bookmark_title": "' + data.articles[8].listableTitle + '", "bookmark_publication": "Commodities"}" data-is-bookmarked="' + data.articles[8].isArticleBookmarked + '"><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark article-bookmark__bookmarked ' + fbookmarkIcon8 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmarked"></use></svg><svg class="action-flag__icon action-flag__icon--bookmark article-bookmark ' + sbookmarkIcon8 + '"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#bookmark"></use></svg></div>';
		articleData += '<ul>';
		articleData += data.articles[8].listableDate ? '<li><time class="article-metadata__date">' + data.articles[8].listableDate + '</time></li>' : '';
		articleData += data.articles[8].linkableText ? '<li><h6>' + data.articles[8].linkableText + '</h6></li>' : '';
		articleData += '</ul>';
	}
	
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

function getMyviewCookie(cookie){
	var getAllcookies = cookie.split(';');
	for(var i=0; i<getAllcookies.length; i++){
		if($.trim(getAllcookies[i]).indexOf('myViewCookieName=') == 0){
			var myViewCookie = getAllcookies[i],
				selectedCookie = myViewCookie.split('=')[1];
				if(selectedCookie == 'listView'){
					setTimeout(function(){
						$('.view-mode .icon-list-view').trigger('click');
					}, 5);
				}
				else{
					setTimeout(function(){
						$('.view-mode .icon-tile-view').trigger('click');
					}, 5); 
				}
			break;
		}
	}
}

function setImgHeightListview(){
	if($('.personalisationhome').hasClass('listView')){
		var lpan = $('.topic-article-image_pan'), rpan = $('.topic-article-rig_pan');
		for(var i=0; i<lpan.length; i++){
			if($(rpan[i]).height() > 190){
				$(lpan[i]).css('height', '220px');
			}
		}
	}
}

$(function () {
	if($('.personalisationhome') && $('.personalisationhome').length){
		getMyviewCookie(document.cookie);
	}
	$('.view-mode').on('click', '.icon-tile-view', function(e){
		e.preventDefault();
		$('.view-mode li').removeClass('selected');
		$(this).parents('li').addClass('selected');
		if($('.personalisationhome') && $('.personalisationhome').length){
			$('.personalisationhome').removeClass('listView').addClass('gridView');
			document.cookie = "myViewCookieName=gridView;"
		}
	});
	$('.view-mode').on('click', '.icon-list-view', function(e){
		e.preventDefault();
		$('.view-mode li').removeClass('selected');
		$(this).parents('li').addClass('selected');
		if($('.personalisationhome') && $('.personalisationhome').length){
			$('.personalisationhome').removeClass('gridView').addClass('listView');
			document.cookie = "myViewCookieName=listView;"
		}
		setImgHeightListview();
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
						setImgHeightListview();
						window.findTooltips();
						if (data.loadMore && !data.loadMore.displayLoadMore) {
							$this.closest('.eachstoryMpan').find('.loadmore').css('display', 'none');
						}
					} else {
						loadLayoutData = createLayoutInner2(data);
						$(eachstory).append(loadLayoutData);
						setImgHeightListview();
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
							setImgHeightListview();
							window.findTooltips();
						} else {
							getscrollData = loadLayoutTwoData(data, loadsection);
							$('.spinnerIcon').addClass('hidespin');
							$('.personalisationPan').append(getscrollData);
							setImgHeightListview();
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
	if (window.matchMedia("(min-width: 1025px)").matches) {
		for(var i = 0; i < latestSubject.length; i++){
			var getFullwidth = $(latestSubject[i]).width(), frEditviewWid = $(latestSubject[i]).find('.frEditview').width(),
			setEditViewWidth = Math.ceil(frEditviewWid / getFullwidth * 100), setLatestSubWid = 100 - setEditViewWidth;
			$(latestSubject[i]).find('.frEditview').css('width', setEditViewWidth +'%');
			$(latestSubject[i]).find('.fllatestSub').css('width', setLatestSubWid - 2 + '%');
		}
	}
	
	if($('.personalisationhome').hasClass('listView')){
		setImgHeightListview();
	}
}); 