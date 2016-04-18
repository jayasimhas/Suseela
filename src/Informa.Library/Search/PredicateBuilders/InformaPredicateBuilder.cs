using System;
using System.Linq;
using System.Linq.Expressions;
using Informa.Library.Search.Results;
using Informa.Library.Utilities.References;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Velir.Search.Core.Models;
using Velir.Search.Core.Page;
using Velir.Search.Core.PredicateBuilders;

namespace Informa.Library.Search.PredicateBuilders
{
	public class InformaPredicateBuilder<T> : SearchPredicateBuilder<T> where T : InformaSearchResultItem
	{
		private const float bigSlopFactor = 100000; // See http://wiki.apache.org/solr/SolrRelevancyCookbook "Term Proximity"

		private readonly ISearchRequest _request;

		public InformaPredicateBuilder(ISearchPageParser pageParser, ISearchRequest request = null)
				: base(pageParser, request)
		{
			_request = request;
		}

		public override Expression<Func<T, bool>> Build()
		{
			var predicate = base.Build();

			predicate = predicate.And(x => x.IsSearchable);
			predicate = predicate.And(x => x.IsLatestVersion);

			// If the inprogress flag is available then add that as as filter, this is used in VWB
			if (_request.QueryParameters.ContainsKey(Constants.QueryString.InProgressKey))
			{
				if (_request.QueryParameters[Constants.QueryString.InProgressKey] == "1")
				{
					predicate = predicate.And(x => x.InProgress);
				}
			}

			// date relevancy
			predicate = predicate.And(x => x.Val == "recip(ms(NOW, searchdate_tdt), 3.16e-11, 100, 1.8)");

			return predicate;
		}

		protected override Expression<Func<T, bool>> AddAllClause(string[] value)
		{
			string query = value.FirstOrDefault();

			if (string.IsNullOrEmpty(query)) return null;

			var searchHeadlines = string.Empty;
			if (_request.QueryParameters.TryGetValue(Constants.QueryString.SearchHeadlinesOnly, out searchHeadlines) && !string.IsNullOrEmpty(searchHeadlines))
			{
				return x => x.SearchTitle == query;
			}

			// Necessary to surround with quotes for promximity logic to work.
			// (term1 term2~10000) doesn't work, but ("term1 term2"~100000) does.
			var quoted_q = string.Format("\"{0}\"", query);

			return item => item.Content.Like(quoted_q, bigSlopFactor);
		}
	}
}