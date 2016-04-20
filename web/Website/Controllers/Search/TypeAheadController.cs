using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Informa.Library.Search;
using Informa.Library.Search.FacetBuilders;
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
		
		public TypeAheadController(ISearchManager<InformaSearchResultItem> searchManager, ISearchPageParser parser)
		{
			_searchManager = searchManager;
			_parser = parser;
		}

		public IEnumerable<CompanyTypeAheadResponseItem> GetCompanies([ModelBinder(typeof(ApiSearchRequestModelBinder))] ApiSearchRequest request)
		{
			var q = new SearchQuery<InformaSearchResultItem>(request, _parser)
			{
				Take = 0,
				Skip = 0
			};
			q.FilterPredicateBuilder = new InformaPredicateBuilder<InformaSearchResultItem>(_parser, request);
			q.QueryPredicateBuilder = new InformaQueryPredicateBuilder<InformaSearchResultItem>(request);
			q.FacetBuilder = new CompaniesFacetBuilder();
			q.SortBuilder = null;

			return _searchManager.GetItems(q).Facets.FirstOrDefault()?.Values.Select(f => new CompanyTypeAheadResponseItem(f.Name)) ?? Enumerable.Empty<CompanyTypeAheadResponseItem>();
		}
	}
}