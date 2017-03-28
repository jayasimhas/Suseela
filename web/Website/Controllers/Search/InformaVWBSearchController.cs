using ArticleItem = Elsevier.Library.CustomItems.Publication.General.ArticleItem;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Library.Rss;
using Informa.Library.Search;
using Informa.Library.Search.Formatting;
using Informa.Library.Search.PredicateBuilders;
using Informa.Library.Search.Results;
using Informa.Library.Search.SearchIndex;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Factory;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Velir.Search.Core.Models;
using Velir.Search.Core.Page;
using Velir.Search.Core.Queries;
using Velir.Search.WebApi.Models;
using httppost = System.Web.Http;
using Sitecore.Data.Items;
using Sitecore.Workflows;
namespace Informa.Web.Controllers.Search
{
    public class InformaVWBSearchController : InformaBaseSearchController
    {
        private readonly ISearchPageParser _parser;
        private readonly IQueryFormatter _queryFormatter;
        private readonly IGlassInterfaceFactory _interfaceFactory;
        private readonly ICacheProvider _cacheProvider;


        public InformaVWBSearchController(
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
        // GET: InformaVWBSearch       
        public VWBSearchQueryResults Get(string vertCode, string pubCode)
        {
            if (string.IsNullOrEmpty(pubCode))
            {
                return null;
            }
            ApiSearchRequest request = new ApiSearchRequest(1, 1000);
            request.Page = 1;
            request.PageId = "A0163A51-2FF8-4A9C-8FBA-6516546E5AE1";
            request.PerPage = 1000;
            request.QueryParameters.Add("PublicationCode", pubCode);

            var q = new SearchQuery<InformaSearchResultItem>(request, _parser);
            q.FilterPredicateBuilder = new InformaPredicateBuilder<InformaSearchResultItem>(_parser, request);
            q.QueryPredicateBuilder = new InformaQueryPredicateBuilder<InformaSearchResultItem>(_queryFormatter, request);

            var results = _searchManager.GetItems(q);

            var searchResults = results as SearchResults;
            Database masterDb = Factory.GetDatabase("master");
            VWBSearchQueryResults vwb = new VWBSearchQueryResults();

            // Initialize dictionaries
            vwb.Taxonomies = new Dictionary<string, string>();
            vwb.Authors = new Dictionary<string, string>();
            vwb.ContentTypes = new Dictionary<string, string>();
            vwb.MediaTypes = new Dictionary<string, string>();
            //vwb.WorkflowStates = new Dictionary<string, string>();
            foreach (Sitecore.ContentSearch.SearchTypes.SearchResultItem searchResult in results.Results)
            {
                var theItem = (ArticleItem)masterDb.GetItem(searchResult.ItemId);

                if (theItem == null)
                {
                    continue;
                }
                //Manually filtering for time

                IArticle article = theItem.InnerItem.GlassCast<IArticle>(inferType: true);
                if (article != null && article.Taxonomies != null && article.Taxonomies.Any())
                {
                    foreach (var tax in article.Taxonomies)
                    {
                        if (!vwb.Taxonomies.ContainsKey(tax._Id.ToString()))
                        {
                            vwb.Taxonomies.Add(tax._Id.ToString(), !string.IsNullOrEmpty(tax.Item_Name) ? tax.Item_Name : tax._Name);
                        }
                    }
                }
                if (article != null && article.Authors != null && article.Authors.Any())
                {
                    foreach (var author in article.Authors)
                    {
                        if (!vwb.Authors.ContainsKey(author._Id.ToString()))
                        {
                            vwb.Authors.Add(author._Id.ToString(), author.First_Name + author.Last_Name);
                        }
                    }
                }
                if (article != null && article.Content_Type != null)
                {
                    if (!vwb.ContentTypes.ContainsKey(article.Content_Type._Id.ToString()))
                    {
                        vwb.ContentTypes.Add(article.Content_Type._Id.ToString(), !string.IsNullOrEmpty(article.Content_Type.Item_Name) ? article.Content_Type.Item_Name : article.Content_Type._Name);
                    }
                }
                if (article != null && article.Media_Type != null)
                {
                    if (!vwb.MediaTypes.ContainsKey(article.Media_Type._Id.ToString()))
                    {
                        vwb.MediaTypes.Add(article.Media_Type._Id.ToString(), !string.IsNullOrEmpty(article.Media_Type.Item_Name) ? article.Media_Type.Item_Name : article.Media_Type._Name);
                    }
                }
                if (article != null && article.Media_Type != null)
                {
                    if (!vwb.MediaTypes.ContainsKey(article.Media_Type._Id.ToString()))
                    {
                        vwb.MediaTypes.Add(article.Media_Type._Id.ToString(), !string.IsNullOrEmpty(article.Media_Type.Item_Name) ? article.Media_Type.Item_Name : article.Media_Type._Name);
                    }
                }
            }
            vwb.Taxonomies = vwb.Taxonomies.OrderBy(k => k.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            vwb.Authors = vwb.Authors.OrderBy(k => k.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            vwb.ContentTypes = vwb.ContentTypes.OrderBy(k => k.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            vwb.MediaTypes = vwb.MediaTypes.OrderBy(k => k.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            return vwb;
        }
    }
}