using System.Linq;
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
            Assert.ArgumentNotNull((object) args, "args");
	        if (Context.Item != null || Context.Database == null || args.Url.ItemPath.Length == 0)
	            return;

            Regex r = new Regex(@"^(.*)(/home/)(\w{2}\d{6})(/)(.*)");
            MatchCollection mc = r.Matches(args.Url.ItemPath);
            if (mc.Count < 1 || mc[0].Groups.Count < 6)
			{
	            return;
			}
            string numFormat = mc[0].Groups[3].Value;

            //find the new article page
            IArticleSearchFilter filter = ArticleSearcher.CreateFilter();
            filter.PageSize = 1;
            filter.Page = 1;
            filter.ArticleNumbers = numFormat.SingleToList();

            var results = ArticleSearcher.Search(filter);

	        IArticle a = results.Articles.FirstOrDefault();
	        if (a == null)
	            return;

            string matchTitle = mc[0].Groups[5].Value;
            string urlTitle = ArticleSearch.GetCleansedArticleTitle(a);
            if (!urlTitle.Equals(matchTitle, System.StringComparison.InvariantCultureIgnoreCase))
                HttpContext.Current.Response.RedirectPermanent(urlTitle);

            Item i = SitecoreContext.GetItem<Item>(a._Id);
	        if (i == null)
	            return;

	        Context.Item = i;
	        args.Url.ItemPath = i.Paths.FullPath;
	        Context.Request.ItemPath = i.Paths.FullPath;
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
			return new ArticleMatch {ArticleNumber = mc[0].Groups[3].Value, ArticleTitle = mc[0].Groups[5].Value};
		}
    }
}