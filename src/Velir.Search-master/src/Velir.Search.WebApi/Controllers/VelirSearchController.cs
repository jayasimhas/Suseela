using System.Web.Http;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Page;
using Velir.Search.Core.Search.Factory;
using Velir.Search.Core.Search.Queries;
using Velir.Search.Core.Search.Results;
using Velir.Search.WebApi.Models;

namespace Velir.Search.WebApi.Controllers
{
	/// <summary>
	/// Web API Controller for querying search results.
	/// </summary>
	public abstract class VelirSearchController<T> : ApiController where T : SearchResultItem
	{
		private readonly ISearchManagerFactory _searchFactory;
		private readonly ISearchPageParser _searchPageParser;
		
		protected VelirSearchController(ISearchManagerFactory searchFactory, ISearchPageParser searchPageParser)
		{
			_searchFactory = searchFactory;
			_searchPageParser = searchPageParser;
		}

		[HttpGet]
		public virtual IQueryResults Get(ApiSearchRequest request)
		{
			if(request == null || string.IsNullOrEmpty(request.PageId)) return null;
			
			var query = new SearchQuery<T>(request, _searchPageParser);

			return _searchFactory.For<T>().GetItems(query);
		}
	}
}
