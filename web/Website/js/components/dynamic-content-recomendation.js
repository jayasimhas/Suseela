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
                if (data.articles.length > 0) {
                    for (var i = 0; i < 3; i++) {
                    	if(data.articles[i].listableImage == null) {
                    		Template += '<div class="section-article">' +
                                             '<span class="article-related-content__category"> ' + data.articles[i].listablePublication + ' </span>' +
                                            '<h5><a class="click-utag" href="' + data.articles[i].linkableUrl + '">' + data.articles[i].listableTitle + '</a></h5>' +
                                             '<time class="article-related-content__date">' + data.articles[i].listableDate + '</time>' +
                                      '</div>';
                                  } else {
                                  	Template += '<div class="section-article">' +
                                             '<img class="article-related-content__img" src="' + data.articles[i].listableImage + '">' +
                                             '<span class="article-related-content__category"> ' + data.articles[i].listablePublication + ' </span>' +
                                            '<h5><a class="click-utag" href="' + data.articles[i].linkableUrl + '">' + data.articles[i].listableTitle + '</a></h5>' +
                                             '<time class="article-related-content__date">' + data.articles[i].listableDate + '</time>' +
                                      '</div>';
                                  }
                        
                    }
                }

                Template += '</div>';

                $('.ContentRecomm-ReadNext').append(Template);

            },
            SuggestedTemplate: function (data) {
                var Template = '';
                if (data.articles.length > 0) {
                    for (var i = 0; i < 3; i++) {
                    	if(data.articles[i].listableImage == null) {
                    		Template += '<div class="contentRecomm-article">' +
                                             '<span class="article-related-content__category"> ' + data.articles[i].listablePublication + ' </span>' +
                                             '<h5><a class="click-utag" href="' + data.articles[i].linkableUrl + '">' + data.articles[i].listableTitle + '</a></h5>' +
                                             '<time class="article-related-content__date">' + data.articles[i].listableDate + '</time>' +
                                      '</div>';
                    	} else {
                        	Template += '<div class="contentRecomm-article">' +
                                             '<img class="article-related-content__img" src="' + data.articles[i].listableImage + '">' +
                                             '<span class="article-related-content__category"> ' + data.articles[i].listablePublication + ' </span>' +
                                             '<h5><a class="click-utag" href="' + data.articles[i].linkableUrl + '">' + data.articles[i].listableTitle + '</a></h5>' +
                                             '<time class="article-related-content__date">' + data.articles[i].listableDate + '</time>' +
                                      '</div>';
                        }
                    }
                }

                Template += '</div>';

                $('.suggested-article').append(Template);
            },
            init: function () {
                var self = this;
                if ($('.ContentRecomm-ReadNext').length > 0) {
                    self.AjaxData('/api/articlesearch', 'POST', { 'TaxonomyIds': $('#hdnTaxonomyIds').val().split(","), 'PageNo': 1, 'PageSize': 4 }, self.RecomendedTemplate);
                }
                if ($('#hdnPreferanceIds').val()) {
                    self.AjaxData('/api/articlesearch', 'POST', { 'TaxonomyIds': $('#hdnPreferanceIds').val().split(","), 'PageNo': 1, 'PageSize': 4 }, self.SuggestedTemplate);
                }
            }
        }

        RecomendedContent.init();
    })();