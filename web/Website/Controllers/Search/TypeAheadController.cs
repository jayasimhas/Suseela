using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Informa.Library.Search;
using Informa.Library.Search.FacetBuilders;
using Informa.Library.Search.Formatting;
using Informa.Library.Search.PredicateBuilders;
using Informa.Library.Search.Results;
using Informa.Library.Search.TypeAhead;
using Velir.Search.Core.Managers;
using Velir.Search.Core.Page;
using Velir.Search.Core.Queries;
using Velir.Search.WebApi.Models;

namespace Informa.Web.Controllers.Search
{
	public class TypeAheadController : ApiController
	{
		private readonly ISearchPageParser _parser;
		private readonly ISearchManager<InformaSearchResultItem> _searchManager;
		private readonly IQueryFormatter _queryFormatter;

		public TypeAheadController(ISearchManager<InformaSearchResultItem> searchManager, ISearchPageParser parser, IQueryFormatter queryFormatter)
		{
			_searchManager = searchManager;
			_parser = parser;
			_queryFormatter = queryFormatter;
		}

		public IEnumerable<CompanyTypeAheadResponseItem> GetCompanies([ModelBinder(typeof(ApiSearchRequestModelBinder))] ApiSearchRequest request)
		{
			var q = new SearchQuery<InformaSearchResultItem>(request, _parser)
			{
				Take = 1,
				Skip = 0,
				FilterPredicateBuilder = new InformaPredicateBuilder<InformaSearchResultItem>(_parser, request),
				QueryPredicateBuilder = new InformaQueryPredicateBuilder<InformaSearchResultItem>(_queryFormatter, request),
				FacetBuilder = new CompaniesFacetBuilder(),
				SortBuilder = null
			};
			var results = _searchManager.GetItems(q);
			return results.Facets.FirstOrDefault()?.Values.Select(f => new CompanyTypeAheadResponseItem(f.Name)) ?? Enumerable.Empty<CompanyTypeAheadResponseItem>();
		}
	}
}