﻿using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Autofac;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Pipelines.Processors;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using Informa.Library.Utilities.Extensions;

namespace Informa.Library.CustomSitecore.Pipelines.HttpRequest
{
    public class ArticleItemResolver : IProcessor<HttpRequestArgs>
    {
        protected IArticleSearch ArticleSearcher;
        protected ISitecoreContext SitecoreContext;

        public ArticleItemResolver(IArticleSearch searcher, ISitecoreContext context)
        {
            ArticleSearcher = searcher;
            SitecoreContext = context;
        }

        public void Process(HttpRequestArgs args)
        {
            Sitecore.Diagnostics.Log.Info("Started ArticleItemResolver", " ArticleItemResolver ");
            Assert.ArgumentNotNull((object)args, "args");
            if (Context.Item != null || Context.Database == null || args.Url.ItemPath.Length == 0)
                return;

            var match = GetArticleNumberFromRequestItemPath(args.Url.ItemPath);
            if (string.IsNullOrEmpty(match.ArticleNumber))
            {
                return;
            }

            //find the new article page
            IArticleSearchFilter filter = ArticleSearcher.CreateFilter();
            filter.PageSize = 1;
            filter.Page = 1;
            filter.ArticleNumbers = match.ArticleNumber.SingleToList();

            var results = ArticleSearcher.Search(filter);

            IArticle a = results.Articles.FirstOrDefault();
            if (a == null)
                return;

            string urlTitle = ArticleSearch.GetCleansedArticleTitle(a); // a._Name.ToLower().Replace(" ", "-");
            if (!urlTitle.Equals(match.ArticleTitle, System.StringComparison.InvariantCultureIgnoreCase))
                if (!HttpContext.Current.Response.IsRequestBeingRedirected)
                {
                    HttpContext.Current.Response.RedirectPermanent(ArticleSearch.GetArticleCustomPath(a), true);
                }

            Item i = SitecoreContext.GetItem<Item>(a._Id);
            if (i == null)
                return;

            Context.Item = i;
            args.Url.ItemPath = i.Paths.FullPath;
            Context.Request.ItemPath = i.Paths.FullPath;
            Sitecore.Diagnostics.Log.Info("Ended ArticleItemResolver", " ArticleItemResolver");
        }

        public struct ArticleMatch
        {
            public string ArticleNumber;
            public string ArticleTitle;
        }

        public ArticleMatch GetArticleNumberFromRequestItemPath(string input)
        {
            Regex r = new Regex(@"^(.*)(/home/)(\w{2}\d{6})(/?)(.*)");
            MatchCollection mc = r.Matches(input);
            if (mc.Count < 1 || mc[0].Groups.Count < 6)
                return new ArticleMatch();
            return new ArticleMatch { ArticleNumber = mc[0].Groups[3].Value, ArticleTitle = mc[0].Groups[5].Value };
        }
    }
}