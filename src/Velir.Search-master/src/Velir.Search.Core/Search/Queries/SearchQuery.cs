using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Models;
using Velir.Search.Core.Page;
using Velir.Search.Core.Rules.Parser;
using Velir.Search.Core.Search.Facets;
using Velir.Search.Core.Search.PredicateBuilders;
using Velir.Search.Core.Search.Sorts;

namespace Velir.Search.Core.Search.Queries
{
	public class SearchQuery<T> : AbstractSearchQuery<T> where T : SearchResultItem
	{
		public SearchQuery(ISearchRequest request, ISearchPageParser pageParser) : base(request)
		{
			PredicateBuilder = new SearchPredicateBuilder<T>(pageParser, request);
			SortBuilder = new SearchSortBuilder<T>(request.GetSorts());
			FacetBuilder = new SearchFacetBuilder<T>(request.GetRefinements());
		}
	}
}
