using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;
using Informa.Library.Search;
using Informa.Library.Search.Formatting;
using Informa.Library.Search.PredicateBuilders;
using Informa.Library.Search.Results;
using Informa.Library.User.Authentication;
using Informa.Library.User.Document;
using Informa.Library.Utilities.TokenMatcher;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Factory;
using Velir.Search.Core.Facets;
using Velir.Search.Core.Managers;
using Velir.Search.Core.Page;
using Velir.Search.Core.Queries;
using Velir.Search.Core.Results;
using Velir.Search.Core.Results.Facets;
using Velir.Search.Models;
using Velir.Search.WebApi.Controllers;
using Velir.Search.WebApi.Models;

namespace Informa.Web.Controllers.Search
{
	public class InformaSearchController : VelirSearchController<InformaSearchResultItem>
	{
		private readonly ISearchPageParser _parser;
		private readonly ISearchManager<InformaSearchResultItem> _searchManager;
		private readonly IQueryFormatter _queryFormatter;
		private readonly IGlassInterfaceFactory _interfaceFactory;
		private readonly ICacheProvider _cacheProvider;
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IIsSavedDocumentContext IsSavedDocumentContext;

		public InformaSearchController(
			ISearchManager<InformaSearchResultItem> searchManager,
			ISearchPageParser parser,
			IQueryFormatter queryFormatter,
		IGlassInterfaceFactory interfaceFactory,
		ICacheProvider cacheProvider,
			IAuthenticatedUserContext userContext,
			IIsSavedDocumentContext isSavedDocumentContext)
						: base(searchManager, parser)
		{
			_searchManager = searchManager;
			_parser = parser;
			_queryFormatter = queryFormatter;
			_interfaceFactory = interfaceFactory;
			_cacheProvider = cacheProvider;
			UserContext = userContext;
			IsSavedDocumentContext = isSavedDocumentContext;
		}

		public override IQueryResults Get([ModelBinder(typeof(ApiSearchRequestModelBinder))]ApiSearchRequest request)
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

			//Replace DCD tokens in the summary
			foreach (InformaSearchResultItem queryResult in results.Results)
			{
				queryResult.Summary = DCDTokenMatchers.ProcessDCDTokens(queryResult.Summary);

				if (UserContext.IsAuthenticated)
				{
					queryResult.IsUserAuthenticated = UserContext.IsAuthenticated;
					queryResult.IsArticleBookmarked = IsSavedDocumentContext.IsSaved(queryResult.ItemId.ToGuid());
				}
			}

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
			var facets = _cacheProvider.GetFromCache($"GetMulitSelectFacets:ID:{_parser.ListingConfiguration._Id}", () => _parser.RefinementOptions.OfType<IFacet>().Where(f => f.Is_Multi_Value && !f.And_Filter).Select(f => f.Key).ToArray());

			if (!facets.Any()) return Enumerable.Empty<FacetGroup>();

			var newRequest = new ApiSearchRequest(_parser, _interfaceFactory)
			{
				PageId = request.PageId,
				Page = 1,
				PerPage = 0,
				QueryParameters = request.QueryParameters.ToDictionary(entry => entry.Key, entry => entry.Value)
			};

			foreach (var facet in facets)
			{
				newRequest.QueryParameters.Remove(facet);
			}

			if (request.QueryParameters.Count == newRequest.QueryParameters.Count) return Enumerable.Empty<FacetGroup>();

			var q = new SearchQuery<InformaSearchResultItem>(request, _parser)
			{
				FilterPredicateBuilder = new InformaPredicateBuilder<InformaSearchResultItem>(_parser, newRequest),
				QueryPredicateBuilder = new InformaQueryPredicateBuilder<InformaSearchResultItem>(_queryFormatter, newRequest),
				FacetBuilder =
					new SearchFacetBuilder<InformaSearchResultItem>(
						request.GetRefinements().Where(r => facets.Contains(r.RefinementKey))),
				SortBuilder = null
			};

			var results = _searchManager.GetItems(q);

			return results.Facets;
		}
	}
}