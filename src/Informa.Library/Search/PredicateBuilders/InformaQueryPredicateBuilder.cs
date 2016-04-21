using System;
using System.Linq;
using System.Linq.Expressions;
using Informa.Library.Search.Formatting;
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
		private readonly IQueryFormatter _formatter;

		public InformaQueryPredicateBuilder(IQueryFormatter queryFormatter, ISearchRequest request = null) : base(request)
		{
			_request = request;
			_formatter = queryFormatter;
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

			var searchHeadlinesValue = string.Empty;
			bool searchHeadlines =
				_request.QueryParameters.TryGetValue(Constants.QueryString.SearchHeadlinesOnly, out searchHeadlinesValue) &&
				!string.IsNullOrEmpty(searchHeadlinesValue);
			
			if (_formatter.NeedsFormatting(query))
			{
				var formattedQuery = _formatter.FormatQuery(query);
				if (searchHeadlines)
				{
					return item => item.Title.Contains(formattedQuery);
				}

				return item => item.Content.Contains(formattedQuery);
			}

			var quotedQuery = $"\"{query}\"";
      if (searchHeadlines)
			{
				return item => item.Title.Like(quotedQuery, bigSlopFactor);
			}

			return item => item.Content.Like(quotedQuery, bigSlopFactor);
		}
	}
}
