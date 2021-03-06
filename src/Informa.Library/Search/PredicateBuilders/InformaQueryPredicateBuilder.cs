﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Informa.Library.Search.Formatting;
using Informa.Library.Search.Results;
using Informa.Library.Utilities.References;
using Sitecore.Configuration;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Velir.Search.Core.Models;
using Velir.Search.Core.PredicateBuilders;

namespace Informa.Library.Search.PredicateBuilders
{
	public class InformaQueryPredicateBuilder<T> : SearchQueryPredicateBuilder<T> where T : InformaSearchResultItem
	{
		private const float bigSlopFactor = 100000; // See http://wiki.apache.org/solr/SolrRelevancyCookbook "Term Proximity"
		private readonly IQueryFormatter _formatter;
		private readonly ISearchRequest _request;

		public InformaQueryPredicateBuilder(IQueryFormatter queryFormatter, ISearchRequest request = null) : base(request)
		{
			_request = request;
			_formatter = queryFormatter;
		}

		public override Expression<Func<T, bool>> Build()
		{
			var predicate = base.Build();

			// date relevancy
			if (Settings.GetBoolSetting("Search.NewerArticlesBoosting.Enabled", false))
			{
				//Boost newer articles (See http://www.sitecoreblogger.com/2014/09/publication-date-boosting-in-sitecore-7.html)
				predicate = predicate.And(x => x.Val == Settings.GetSetting("Search.NewerArticlesBoosting.BoostFunction"));
			}

			if (_request.PageId == Constants.VWBSearchPageId)
			{
				//VWB:  Filter out non Article items
				predicate = predicate.And(x => x.TemplateName == Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.IArticleConstants.TemplateName);
			}
			else
			{
                //Include Search for authors
                var q = Velir.Search.Core.Reference.SiteSettings.QueryString.QueryKey;
				if (_request.QueryParameters.ContainsKey(q) && string.IsNullOrEmpty(_request.QueryParameters[q]) == false) {
                    var val = _request.QueryParameters[q];
                    if (_formatter.NeedsFormatting(val))
                        predicate = predicate.Or(x => x.Byline.MatchWildcard(val));
                    else 
                        predicate = predicate.Or(x => x.Byline.Contains(val));
                }
            }

			return predicate;
		}

		protected override Expression<Func<T, bool>> AddAllClause(string[] value)
		{
			var query = value.FirstOrDefault();

			if (string.IsNullOrEmpty(query)) return null;

			var searchHeadlinesValue = string.Empty;
			var searchHeadlines =
					_request.QueryParameters.TryGetValue(Constants.QueryString.SearchHeadlinesOnly, out searchHeadlinesValue) &&
					!string.IsNullOrEmpty(searchHeadlinesValue);

			if (_formatter.NeedsFormatting(query))
			{
				var formattedQuery = _formatter.FormatQuery(query);

                if (query.Contains('"')) {
                    if (searchHeadlines)
                        return item => item.ExactMatchTitle.MatchWildcard(formattedQuery);
                    else
                        return item => item.ExactMatchContent.MatchWildcard(formattedQuery);
                } else if (query.Contains("*")) {
                    if (searchHeadlines)
                        return item => item.WildcardTitle.MatchWildcard(formattedQuery);
                    else
                        return item => item.WildcardContent.MatchWildcard(formattedQuery);
                } else {
                    if (searchHeadlines)
                        return item => item.Title.MatchWildcard(formattedQuery);
                    else
                        return item => item.Content.MatchWildcard(formattedQuery);
                }
            }

			var quotedQuery = $"\"{query}\"";
            if (searchHeadlines)
                return item => item.Title.Like(quotedQuery, bigSlopFactor);
            else 
                return item => item.Content.Like(quotedQuery, bigSlopFactor);
		}
	}
}