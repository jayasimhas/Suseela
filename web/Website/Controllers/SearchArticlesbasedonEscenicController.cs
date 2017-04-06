using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using Informa.Library.Services.Global;
using Sitecore.SharedSource.DataImporter.Providers;

namespace Informa.Web.Controllers
{
    public class SearchArticlesbasedonEscenicController : System.Web.Http.ApiController
    {
        private readonly ArticleUtil _articleUtil;
        protected readonly IGlobalSitecoreService GlobalService;
        private readonly ILog _logger;

        Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("master");


        public SearchArticlesbasedonEscenicController(ArticleUtil articleUtil, IGlobalSitecoreService globalService, ILog logger)
        {
            _articleUtil = articleUtil;
            GlobalService = globalService;
            _logger = logger;
        }
        // GET: SearchArticlesbasedonEscenic/Details/5
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: SearchArticlesbasedonEscenic/Create
        [HttpGet]
        public object Get(string articleNumber, string EscenicId)
        {
            try
            {
              
                var pubPrefixes = _articleUtil.GetPublicationsPrefixes();
             
                if (pubPrefixes.Any(n => articleNumber.StartsWith(n)))
                {
                    var publicationId = _articleUtil.GetVerticalRootByPubPrefix(new string(articleNumber.Take(2).ToArray()));
                  

                    Item article = _articleUtil.GetArticleItemByEscenic(EscenicId, publicationId.ToGuid());
                
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
                

                return null;
            }
            catch (Exception ex)
            {
                XMLDataLogger.WriteLog("ex :" + ex.Message, "ArticleCleanup");
                throw new Exception();
            }
        }
    }
}
