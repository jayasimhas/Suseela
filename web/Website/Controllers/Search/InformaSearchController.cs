using Informa.Library.Search.PredicateBuilders;
using Informa.Library.Search.Results;
using Informa.Library.User.Authentication;
using Informa.Library.User.Document;
using Informa.Library.Utilities.TokenMatcher;
using Velir.Search.Core.Managers;
using Velir.Search.Core.Page;
using Velir.Search.Core.Queries;
using Velir.Search.Core.Results;
using Velir.Search.WebApi.Controllers;
using Velir.Search.WebApi.Models;

namespace Informa.Web.Controllers.Search
{
	public class InformaSearchController : VelirSearchController<InformaSearchResultItem>
	{
		private readonly ISearchPageParser _parser;
		private readonly ISearchManager<InformaSearchResultItem> _searchManager;
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IIsSavedDocumentContext IsSavedDocumentContext;

		public InformaSearchController(
			ISearchManager<InformaSearchResultItem> searchManager,
			ISearchPageParser parser,
			IAuthenticatedUserContext userContext,
			IIsSavedDocumentContext isSavedDocumentContext)
						: base(searchManager, parser)
		{
			_searchManager = searchManager;
			_parser = parser;
			UserContext = userContext;
			IsSavedDocumentContext = isSavedDocumentContext;
		}

		public override IQueryResults Get(ApiSearchRequest request)
		{
			//If an improper request is passed in return nothing
			if (string.IsNullOrEmpty(request?.PageId))
			{
				return null;
			}
			
			var q = new SearchQuery<InformaSearchResultItem>(request, _parser);
			q.PredicateBuilder = new InformaPredicateBuilder<InformaSearchResultItem>(_parser, request); 

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

			return results;
		}
	}
}