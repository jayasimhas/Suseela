using Glass.Mapper.Sc;
using Informa.Library.Search;
using Informa.Library.Search.Formatting;
using Informa.Library.Search.PredicateBuilders;
using Informa.Library.Search.Results;
using Informa.Library.Search.SearchIndex;
using Informa.Library.Site;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Factory;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;
using Velir.Search.Core.Facets;
using Velir.Search.Core.Page;
using Velir.Search.Core.Queries;
using Velir.Search.Core.Results;
using Velir.Search.Core.Results.Facets;
using Velir.Search.Models;
using Velir.Search.WebApi.Models;

namespace Informa.Web.Controllers.Search
{
    public class InformaSearchController : InformaBaseSearchController//VelirSearchController<InformaSearchResultItem>
	{
		private readonly ISearchPageParser _parser;
		private readonly IQueryFormatter _queryFormatter;
		private readonly IGlassInterfaceFactory _interfaceFactory;
		private readonly ICacheProvider _cacheProvider;


        public InformaSearchController(
			ISearchPageParser parser,
			IQueryFormatter queryFormatter,
		IGlassInterfaceFactory interfaceFactory,
		ICacheProvider cacheProvider, ISitecoreContext sitecoreContext, ISiteRootContext siterootContext, ISearchIndexNameService indexNameService)
						: base(siterootContext, sitecoreContext, indexNameService)
		{
			_parser = parser;
			_queryFormatter = queryFormatter;
			_interfaceFactory = interfaceFactory;
			_cacheProvider = cacheProvider;
		}

		public IQueryResults Get([ModelBinder(typeof(ApiSearchRequestModelBinder))]ApiSearchRequest request)
		{
            //If an improper request is passed in return nothing
            if (string.IsNullOrEmpty(request?.PageId))
			{
				return null;
			}

			var q = new SearchQuery<InformaSearchResultItem>(request, _parser);
			q.FilterPredicateBuilder = new InformaPredicateBuilder<InformaSearchResultItem>(_parser, request);
			q.QueryPredicateBuilder = new InformaQueryPredicateBuilder<InformaSearchResultItem>(_queryFormatter, request);

			var results = _searchManager.GetItems(q);

			var multiSelectFacetResults = GetMultiSelectFacetResults(request);

			return new QueryResults
			{
				Request = request,
				Results = results.Results,
				TotalResults = results.TotalResults,
				Facets = MergeFacets(results.Facets, multiSelectFacetResults)
			};
		}

		private IEnumerable<FacetGroup> MergeFacets(IEnumerable<FacetGroup> allFacets,
			IEnumerable<FacetGroup> multiSelectFacets)
		{
			return allFacets.Select(f => multiSelectFacets.FirstOrDefault(m => f.Id == m.Id) ?? f);
		}

		private IEnumerable<FacetGroup> GetMultiSelectFacetResults(ApiSearchRequest request)
		{
			var facets = _cacheProvider.GetFromCache($"GetMulitSelectFacets:ID:{request.PageId}", () => _parser.RefinementOptions.OfType<IFacet>().Where(f => f.Is_Multi_Value && !f.And_Filter).Select(f => f.Key).ToArray());

			if (!facets.Any()) return Enumerable.Empty<FacetGroup>();

			var newRequest = new ApiSearchRequest(_parser, _interfaceFactory)
			{
				PageId = request.PageId,
				Page = 1,
				PerPage = 0,
				QueryParameters = request.QueryParameters.ToDictionary(entry => entry.Key, entry => entry.Value)
			};

			//For each facet group get facets count without including the selections from this group to enforce ORing between the same groups
			IEnumerable<FacetGroup> facetsGroups = Enumerable.Empty<FacetGroup>();
			foreach (var facet in request.QueryParameters.Where(r => facets.Contains(r.Key)))
			{
				//reset the queryParameters to all selections
				newRequest.QueryParameters = request.QueryParameters.ToDictionary(entry => entry.Key, entry => entry.Value);
				//remove the current facet group for the parameters of the new request
				newRequest.QueryParameters.Remove(facet);

				if (request.QueryParameters.Count == newRequest.QueryParameters.Count) continue;

				//Do the search to get the results
				var qForCurrentGroup = new SearchQuery<InformaSearchResultItem>(request, _parser)
				{
					FilterPredicateBuilder = new InformaPredicateBuilder<InformaSearchResultItem>(_parser, newRequest),
					QueryPredicateBuilder = new InformaQueryPredicateBuilder<InformaSearchResultItem>(_queryFormatter, newRequest),
					FacetBuilder =
						new SearchFacetBuilder<InformaSearchResultItem>(
							request.GetRefinements().Where(r => facets.Contains(r.RefinementKey))),
					SortBuilder = null
				};

				var resultsForCurrentGroup = _searchManager.GetItems(qForCurrentGroup);

				//include only the current facets group's count into the facetsGroups list
				facetsGroups = facetsGroups.Concat(resultsForCurrentGroup.Facets.Where(w => w.Id == facet.Key));
			}

			return facetsGroups;
		}


		//private IEnumerable<FacetGroup> GetMultiSelectFacetResults(ApiSearchRequest request)
		//{
		//	var facets = _cacheProvider.GetFromCache($"GetMulitSelectFacets:ID:{request.PageId}", () => _parser.RefinementOptions.OfType<IFacet>().Where(f => f.Is_Multi_Value && !f.And_Filter).Select(f => f.Key).ToArray());

		//	if (!facets.Any()) return Enumerable.Empty<FacetGroup>();

		//	var newRequest = new ApiSearchRequest(_parser, _interfaceFactory)
		//	{
		//		PageId = request.PageId,
		//		Page = 1,
		//		PerPage = 0,
		//		QueryParameters = request.QueryParameters.ToDictionary(entry => entry.Key, entry => entry.Value)
		//	};

		//	foreach (var facet in facets)
		//	{
		//		newRequest.QueryParameters.Remove(facet);
		//	}

		//	if (request.QueryParameters.Count == newRequest.QueryParameters.Count) return Enumerable.Empty<FacetGroup>();

		//	var q = new SearchQuery<InformaSearchResultItem>(request, _parser)
		//	{
		//		FilterPredicateBuilder = new InformaPredicateBuilder<InformaSearchResultItem>(_parser, newRequest),
		//		QueryPredicateBuilder = new InformaQueryPredicateBuilder<InformaSearchResultItem>(_queryFormatter, newRequest),
		//		FacetBuilder =
		//			new SearchFacetBuilder<InformaSearchResultItem>(
		//				request.GetRefinements().Where(r => facets.Contains(r.RefinementKey))),
		//		SortBuilder = null
		//	};

		//	var results = _searchManager.GetItems(q);

		//	return results.Facets;
		//}
	}

}