using System.Web.Mvc;
using Glass.Mapper.Sc.Web.Mvc;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Models;
using Velir.Search.Core.Page;
using Velir.Search.Core.Search.Factory;
using Velir.Search.Core.Search.Queries;

namespace Velir.Search.Mvc.Controllers
{
	public class SearchListingController : GlassController
	{
		private readonly ISearchPageParser _pageParser;
		private readonly ISearchManagerFactory _factory;

		public SearchListingController(ISearchPageParser pageParser, ISearchManagerFactory factory)
		{
			_pageParser = pageParser;
			_factory = factory;
		}

		public ActionResult SearchListing(SearchRequest request)
		{
			if (request == null || string.IsNullOrEmpty(request.PageId)) return null;

			var query = new SearchQuery<SearchResultItem>(request, _pageParser);

			var results = _factory.For<SearchResultItem>().GetItems(query);

			return View(results);
		}
	}
}
