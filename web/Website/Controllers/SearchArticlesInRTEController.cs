﻿using Informa.Library.Services.Global;
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
            Item article = _articleUtil.GetArticleItemByNumber(articleNumber);
            if (article != null)
            {
                var result = new ArticleItem
                {
                    _Id = article.ID.ToGuid(),
                    _Name = article.DisplayName,
                    _Path = article.Paths.Path
                };
                return result;
            }
            return null;
        }


        /// <summary>
        /// Gets the article number using the item id.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns></returns>
        protected string GetArticleNumber(string id)
        {
            var article = GlobalService.GetItem<IArticle>(id);
            if (!string.IsNullOrEmpty(article.Article_Number))
                return article.Article_Number;
            else
                return string.Empty;
        }


        //POST api/<controller>
        public void Post([FromBody]ArticleRequestInRTE data)
        {
            if (data != null)
            {
                //var url = System.Web.HttpContext.Current.Request.Url;
                //var siteContext = Sitecore.Sites.SiteContextFactory.GetSiteContext(url.Host, url.PathAndQuery);
                var selectedID = "{" + data.ItemId.ToUpper() + "}";
                var article = master.GetItem(new ID(data.CurrentItemId));
                if (article != null)
                {
                    var referencedValues = article.Fields["Referenced articles"];
                    using (new SecurityDisabler())
                    {
                        //CacheManager.Enabled = false;
                        article.Editing.BeginEdit();
                        if (referencedValues != null)                        
                            //referencedValues.Value = "{5A3D67DF-3917-4AC2-BDAF-69CDA1204C2A}";
                            referencedValues.Value += "|" + selectedID.ToUpper();
                        else
                        referencedValues.Value = selectedID.ToUpper();
                        
                        article.Editing.EndEdit();
                        //CacheManager.Enabled = true;
                    }
                    //Sitecore.Data.Fields.MultilistField multiselectField = article.Fields[IArticleConstants.Referenced_ArticlesFieldName];

                    //if (multiselectField.Contains(selectedID))
                    //{
                    //    CacheManager.Enabled = false;
                    //    article.Editing.BeginEdit();
                    //    if (!multiselectField.Contains(selectedID))
                    //    {
                    //        multiselectField.Add(selectedID);
                    //    }
                    //    article.Editing.EndEdit();
                    //    CacheManager.Enabled = true;
                    //}
                }
            }
        }
    }

    public class ArticleRequestInRTE
    {
        public string CurrentItemId { get; set; }
        public string ArticleType { get; set; }
        public string ItemId { get; set; }
    }
}