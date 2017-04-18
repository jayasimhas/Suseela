using ArticleItem = Elsevier.Library.CustomItems.Publication.General.ArticleItem;
using Glass.Mapper.Sc;
using Informa.Library.Rss;
using Informa.Library.Search.Formatting;
using Informa.Library.Search.PredicateBuilders;
using Informa.Library.Search.Results;
using Informa.Library.Search.SearchIndex;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Factory;
using Sitecore.Configuration;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Velir.Search.Core.Page;
using Velir.Search.Core.Queries;
using Velir.Search.WebApi.Models;
using Informa.Library.Services.Global;
using Velir.Search.Models;
using System.Web.Http.ModelBinding;
using Informa.Library.Search;

namespace Informa.Web.Controllers.Search
{
    public class InformaVWBSearchController : InformaBaseSearchController
    {
        private readonly ISearchPageParser _parser;
        private readonly IQueryFormatter _queryFormatter;
        private readonly IGlassInterfaceFactory _interfaceFactory;
        private readonly ICacheProvider _cacheProvider;
        protected readonly IGlobalSitecoreService GlobalService;

        public InformaVWBSearchController(
            ISearchPageParser parser,
            IQueryFormatter queryFormatter,
        IGlassInterfaceFactory interfaceFactory,
        ICacheProvider cacheProvider, ISitecoreContext sitecoreContext, ISiteRootContext siterootContext, ISearchIndexNameService indexNameService, IGlobalSitecoreService globalService)
                        : base(siterootContext, sitecoreContext, indexNameService)
        {
            _parser = parser;
            _queryFormatter = queryFormatter;
            _interfaceFactory = interfaceFactory;
            _cacheProvider = cacheProvider;
            GlobalService = globalService;
        }
        // GET: InformaVWBSearch       
       
        public VWBSearchQueryResults Get([ModelBinder(typeof(ApiSearchRequestModelBinder))]ApiSearchRequest request)
        {
            if (request == null)
            {
                return null;
            }
            var defaultCount = "1000";
            int PerPageCount = Convert.ToInt32(!string.IsNullOrEmpty(Settings.GetSetting("VwbDropdownCount")) ? Settings.GetSetting("VwbDropdownCount") : defaultCount);
            request.PerPage = PerPageCount;

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
                            if (tax._Path.Contains("Environment Globals"))
                                vwb.Taxonomies.Add(tax._Id.ToString(), tax._Path.Replace("/sitecore/content/Environment Globals/Taxonomy/", ""));
                            else if (tax._Path.Contains("Agri"))
                                vwb.Taxonomies.Add(tax._Id.ToString(), tax._Path.Replace("/sitecore/content/Agri/Vertical Globals/Taxonomy/", ""));
                            else if (tax._Path.Contains("Maritime"))
                                vwb.Taxonomies.Add(tax._Id.ToString(), tax._Path.Replace("/sitecore/content/Maritime/Vertical Globals/Taxonomy/", ""));
                            else if (tax._Path.Contains("Pharma"))
                                vwb.Taxonomies.Add(tax._Id.ToString(), tax._Path.Replace("/sitecore/content/Pharma/Vertical Globals/Taxonomy/", ""));
                            else
                                vwb.Taxonomies.Add(tax._Id.ToString(), tax._Path);
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