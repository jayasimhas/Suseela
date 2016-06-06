using System.Collections.Generic;
using Glass.Mapper.Sc;
using Informa.Library.Search.PredicateBuilders;
using Informa.Library.Search.Results;
using Informa.Library.User.Authentication;
using Informa.Library.User.Document;
using Informa.Library.Utilities.TokenMatcher;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Factory;
using Velir.Search.Core.Managers;
using Velir.Search.Core.Page;
using Velir.Search.Core.Queries;
using Velir.Search.Core.Results;
using Velir.Search.WebApi.Controllers;
using Velir.Search.WebApi.Models;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.Search.ComputedFields.SearchResults.Converter;

using System.Linq;

namespace Informa.Web.Controllers.Search
{
    public class InformaSearchController : VelirSearchController<InformaSearchResultItem>
    {
        private readonly ICacheProvider _cache;
        private readonly IGlassInterfaceFactory _interfaceFactory;
        private ISitecoreContext _context;
        private readonly ISearchPageParser _parser;
        private readonly ISearchManager<InformaSearchResultItem> _searchManager;
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IIsSavedDocumentContext IsSavedDocumentContext;

		public InformaSearchController(
			ISearchManager<InformaSearchResultItem> searchManager,
			ISearchPageParser parser,
            IGlassInterfaceFactory interfaceFactory,
			ISitecoreContext context,
			ICacheProvider cache,
			IAuthenticatedUserContext userContext,
			IIsSavedDocumentContext isSavedDocumentContext)
            : base(searchManager, parser)
        {
            _searchManager = searchManager;
            _parser = parser;
            _context = context;
            _cache = cache;
            _interfaceFactory = interfaceFactory;
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

            var predicateBuilder = new InformaPredicateBuilder<InformaSearchResultItem>(_parser, request);

            //predicateBuilder.ListableOnly = true;

            var q = new SearchQuery<InformaSearchResultItem>(request, _parser);
            q.PredicateBuilder = predicateBuilder;

            var results = _searchManager.GetItems(q);


            foreach (InformaSearchResultItem queryResult in results.Results)
            {
                var taxenomy = _context.GetItem<IArticle>(Sitecore.Data.ID.Parse(queryResult.ItemId).ToGuid());
                if (taxenomy != null)
                {
                    List<HtmlLink> lTaxenomy = _context.GetItem<IArticle>(Sitecore.Data.ID.Parse(queryResult.ItemId).ToGuid()).Taxonomies.Take(3).Select((tax) => { return new HtmlLink() { Url = "#?areas=" + tax._Name, Title = tax._Name }; }).ToList();
                    queryResult.SearchDisplayTaxonomy = new HtmlLinkList() { Links = lTaxenomy };
                }
                //var lTax = _context.GetItem<IArticle>(new Guid(queryResult.ItemId.ToString())).Taxonomies.Take(3).Select(tax => tax);
            }

            //Loop through the results and add the authenticaton and bookmarking property values to be used
            //on the front end.
            if (UserContext.IsAuthenticated)
			{
				List<InformaSearchResultItem> resultsWithBookmarks = new List<InformaSearchResultItem>();
				foreach (InformaSearchResultItem queryResult in results.Results)
				{
                    if (queryResult.Title != "")
                    {
                        queryResult.IsUserAuthenticated = UserContext.IsAuthenticated;
                        queryResult.IsArticleBookmarked = IsSavedDocumentContext.IsSaved(queryResult.ItemId.ToGuid());
                        resultsWithBookmarks.Add(queryResult);
                    }
				}

				results.Results = resultsWithBookmarks;
			}

            //Replace DCD tokens in the summary
            foreach (InformaSearchResultItem queryResult in results.Results)
            {
                queryResult.Summary = DCDTokenMatchers.ProcessDCDTokens(queryResult.Summary);
            }

            return results;
        }
    }
}