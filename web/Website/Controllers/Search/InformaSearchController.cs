using System.Collections.Generic;
using Glass.Mapper.Sc;
using Informa.Library.Search.PredicateBuilders;
using Informa.Library.Search.Results;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Library.Utilities.StringUtils;
using Informa.Library.Utilities.TokenMatcher;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Factory;
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
        private readonly ICacheProvider _cache;
        private readonly IGlassInterfaceFactory _interfaceFactory;
        private ISitecoreContext _context;
        private readonly ISearchPageParser _parser;
        private readonly ISearchManager<InformaSearchResultItem> _searchManager;
        private readonly IAuthenticatedUserContext _authenticatedUserContext;
        private readonly IManageSavedDocuments _manageSavedDocuments;

        public InformaSearchController(ISearchManager<InformaSearchResultItem> searchManager, ISearchPageParser parser,
            IGlassInterfaceFactory interfaceFactory, ISitecoreContext context, ICacheProvider cache, IAuthenticatedUserContext authenticatedUserContext, IManageSavedDocuments manageSavedDocuments)
            : base(searchManager, parser)
        {
            _searchManager = searchManager;
            _parser = parser;
            _context = context;
            _cache = cache;
            _interfaceFactory = interfaceFactory;
            _authenticatedUserContext = authenticatedUserContext;
            _manageSavedDocuments = manageSavedDocuments;
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

            //Loop through the results and add the authenticaton and bookmarking property values to be used
            //on the front end.
            if (_authenticatedUserContext.IsAuthenticated)
            {
                var currentUser = _authenticatedUserContext.User;

                List<InformaSearchResultItem> resultsWithBookmarks = new List<InformaSearchResultItem>();
                foreach (InformaSearchResultItem queryResult in results.Results)
                {
                    queryResult.IsUserAuthenticated = true;
                    queryResult.IsArticleBookmarked = _manageSavedDocuments.IsBookmarked(currentUser, queryResult.ItemId.ToGuid());
                    resultsWithBookmarks.Add(queryResult);
                }

              


                results.Results = resultsWithBookmarks;
            }

            foreach (InformaSearchResultItem queryResult in results.Results)
            {
                string processedText = XmlStringUtil.UnescapeXMLValue(queryResult.Summary);
                queryResult.Summary = ReplaceArticleTokens(processedText);
            }


            return results;
        }

        /// <summary>
        ///     Replace article and deal/company tokens in the summary
        /// </summary>
        /// <param name="bodyText"></param>
        /// <returns></returns>
        public string ReplaceArticleTokens(string bodyText)
        {
            TokenReplacer tokenReplacer = new TokenReplacer();

            //Companies
            string processedText = tokenReplacer.ReplaceCompany(bodyText);

            //Articles
            processedText = tokenReplacer.ReplaceRelatedArticles(processedText);

            return processedText;
        }




    }
}