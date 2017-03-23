using Informa.Library.Services.Global;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Newtonsoft.Json;
using Sitecore.Buckets.Search;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using System.Net.Http.Formatting;
using Sitecore.Web;
using Sitecore.Caching;
using Sitecore.Configuration;
using System.Web;

namespace Informa.Web.Controllers
{
    public class SearchArticlesInRTEController : ApiController
    {
        private readonly ArticleUtil _articleUtil;
        protected readonly IGlobalSitecoreService GlobalService;
        private readonly ILog _logger;

        Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("master");

        public SearchArticlesInRTEController(ArticleUtil articleUtil, IGlobalSitecoreService globalService, ILog logger)
        {
            _articleUtil = articleUtil;
            GlobalService = globalService;
            _logger = logger;
        }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet]
        public object Get(string articleNumber)
        {
            var pubPrefixes = _articleUtil.GetPublicationsPrefixes();
            if (pubPrefixes.Any(n => articleNumber.StartsWith(n)))
            {
                var publicationId = _articleUtil.GetVerticalRootByPubPrefix(new string(articleNumber.Take(2).ToArray()));
                Item article = _articleUtil.GetArticleItemByNumber(articleNumber, publicationId.ToGuid());
                if (article != null)
                {
                    var result = new ArticleItem
                    {
                        _Id = article.ID.ToGuid(),
                        _Name = article.Name,
                        _Path = article.Paths.Path
                    };
                    return result;
                }
            }
            //ArticleItem article = _articleUtil.GetArticlesByNumberWithinRTE(articleNumber, "master");

            return null;
        }
    }
}