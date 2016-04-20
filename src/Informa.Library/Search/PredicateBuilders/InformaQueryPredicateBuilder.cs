using System;
using System.Linq;
using System.Linq.Expressions;
using Informa.Library.Search.Results;
using Informa.Library.Utilities.References;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Velir.Search.Core.Models;
using Velir.Search.Core.PredicateBuilders;

namespace Informa.Library.Search.PredicateBuilders
{
	public class InformaQueryPredicateBuilder<T> : SearchQueryPredicateBuilder<T> where T : InformaSearchResultItem
	{
		private const float bigSlopFactor = 100000; // See http://wiki.apache.org/solr/SolrRelevancyCookbook "Term Proximity"

		private readonly ISearchRequest _request;

		public InformaQueryPredicateBuilder(ISearchRequest request = null) : base(request)
		{
			_request = request;
		}

		public override Expression<Func<T, bool>> Build()
		{
			var predicate = base.Build();

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
				return x => x.Title == query;
			}

			// Necessary to surround with quotes for promximity logic to work.
			// (term1 term2~10000) doesn't work, but ("term1 term2"~100000) does.
			var quoted_q = string.Format("\"{0}\"", query);

			return item => item.Content.Like(quoted_q, bigSlopFactor);
		}
	}
}
