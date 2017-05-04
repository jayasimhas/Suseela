(function () {
        var RecomendedContent = {
            AjaxData: function (url, type, data, SuccessCallback) {
                console.log(JSON.stringify(data));
                $.ajax({
                    url: url,
                    data: JSON.stringify(data),
                    dataType: 'json',
                    contentType: "application/json",
                    type: type,
                    cache: false,
                    success: function (data) {
                        //if (data.articles && typeof data.articles === "object" && data.articles.length >= 3) {
                        SuccessCallback(data);
                        //} 
                    }
                });
            },
            RecomendedTemplate: function (data) {
                var Template = '';
                var HeadingAnalytics = $('.ContentRecomm-ReadNext').find('h2').text();
                if (data.articles.length > 0) {
                    for (var i = 0; i < 3; i++) {
						var addCls = (data.articles[i].isSonsoredBy) ? 'sponsored_cont' : '';
							var sponsoredByLogo = (data.articles[i].sponsoredLink && data.articles[i].sponsoredByLogo) ? '<li><a href="'+data.articles[i].sponsoredLink+'"><img src="'+data.articles[i].sponsoredByLogo+'"></a></li>' : ((data.articles[i].sponsoredLink == null || data.articles[i].sponsoredLink == undefined) && data.articles[i].sponsoredByLogo) ? '<img src="'+data.articles[i].sponsoredByLogo+'">' : '',
								sponsoredByTitle = (data.articles[i].sponsoredByTitle) ? '<li><time class="article-metadata__date sponsored_title">'+data.articles[i].sponsoredByTitle+'</time></li>' : '',
								listablePublication = (data.articles[i].listablePublication) ? '<span class="article-related-content__category"> ' + data.articles[i].listablePublication + ' </span>' : '',
								listableImage = (data.articles[i].listableImage) ? '<img class="article-related-content__img" src="' + data.articles[i].listableImage + '">' : '',
								listableDate = (data.articles[i].listableDate) ? '<time class="article-related-content__date">' + data.articles[i].listableDate + '</time>' : '',
								listableTitle = (data.articles[i].listableTitle) ? '<h5><a class="click-utag" data-info=\'{"event_name":"article_click_through,recommendation_content","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[0].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"What to read next","ga_eventAction":"' + HeadingAnalytics + '","ga_eventLabel":"' + data.articles[i].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '","recommendation_category":"What to read next"}\' href="' + data.articles[i].linkableUrl + '">' + data.articles[i].listableTitle + '</a></h5>' : '',
								articlemeta = (sponsoredByTitle == '' && sponsoredByLogo == '') ? '' : '<div class="article-metadata"><ul>'+ sponsoredByTitle + sponsoredByLogo +'</ul></div>';
								;
							if(data.articles[i].listableImage == null) { 
								Template += '<div class="section-article '+addCls+'">' +
												articlemeta + listablePublication + listableTitle + listableDate +
											'</div>';
                        } else {
								Template += '<div class="section-article '+addCls+'">' +
												articlemeta + listableImage +  listablePublication +  listableTitle + listableDate +
											'</div>';
                        }
                    }
                } else {
                    $('.ContentRecomm-ReadNext').hide();
                }

                Template += '</div>';
				
                $('.ContentRecomm-ReadNext').append(Template);

            },
            SuggestedTemplate: function (data) {
                var Template = '';
                var HeadingAnalytics = $('.suggested-article').find('h2').text();
                if (data.articles.length > 0) {
                    for (var i = 0; i < 3; i++) {
						var addCls = (data.articles[i].isSonsoredBy) ? 'sponsored_cont' : '',
							sponsoredByLogo = (data.articles[i].sponsoredLink && data.articles[i].sponsoredByLogo) ? '<li><a href="'+data.articles[i].sponsoredLink+'"><img src="'+data.articles[i].sponsoredByLogo+'"></a></li>' : ((data.articles[i].sponsoredLink == null || data.articles[i].sponsoredLink == undefined) && data.articles[i].sponsoredByLogo) ? '<img src="'+data.articles[i].sponsoredByLogo+'">' : '',
							sponsoredByTitle = (data.articles[i].sponsoredByTitle) ? '<li><time class="article-metadata__date sponsored_title">'+data.articles[i].sponsoredByTitle+'</time></li>' : '',
							listablePublication = (data.articles[i].listablePublication) ? '<span class="article-related-content__category"> ' + data.articles[i].listablePublication + ' </span>' : '',
							listableImage = (data.articles[i].listableImage) ? '<img class="article-related-content__img" src="' + data.articles[i].listableImage + '">' : '',
							listableDate = (data.articles[i].listableDate) ? '<time class="article-related-content__date">' + data.articles[i].listableDate + '</time>' : '',
							listableTitle = (data.articles[i].listableTitle) ? '<h5><a class="click-utag" data-info=\'{"event_name":"article_click_through,recommendation_content","page_name":"' + analytics_data["page_name"] + '","click_through_destination":"' + data.articles[0].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","ga_eventCategory":"Suggested for you","ga_eventAction":"' + HeadingAnalytics + '","ga_eventLabel":"' + data.articles[i].listableTitle.replace(/'/g, "").replace(/"/g, '') + '","publication_click":"' + analytics_data["publication"] + '","recommendation_category":"Suggested for you"}\' href="' + data.articles[i].linkableUrl + '">' + data.articles[i].listableTitle + '</a></h5>' : '',
							articlemeta = (sponsoredByTitle == '' && sponsoredByLogo == '') ? '' : '<div class="article-metadata"><ul>'+ sponsoredByTitle + sponsoredByLogo+ '</ul></div>';
							
                        if(data.articles[i].listableImage == null) {
							if(sponsoredByTitle && sponsoredByLogo && listablePublication && listableTitle && listableDate){
                            Template += '<div class="contentRecomm-article '+addCls+'">' +
											articlemeta + listablePublication + listableTitle + listableDate +
										'</div>';
							}
                        } else {
                        Template += '<div class="contentRecomm-article '+addCls+'">' +
										articlemeta + listableImage + listablePublication + listableTitle + listableDate +
									'</div>';
						}   
					}
                } else {
                    $('.suggested-article').hide();
                }

                Template += '</div>';

                $('.suggested-article').append(Template);
            },
            init: function () {
                var self = this;
                if ($('.ContentRecomm-ReadNext').length > 0) {
                    self.AjaxData('/api/articlesearch', 'POST', { 'TaxonomyIds': $('#hdnTaxonomyIds').val().split(","), 'PageNo': 1, 'PageSize': 3 }, self.RecomendedTemplate);
                }
                if ($('#hdnPreferanceIds').val()) {
                    self.AjaxData('/api/articlesearch', 'POST', { 'TaxonomyIds': $('#hdnPreferanceIds').val().split(","), 'PageNo': 1, 'PageSize': 3 }, self.SuggestedTemplate);
                }
            }
        }

        RecomendedContent.init();
    })();
