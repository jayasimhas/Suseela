using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Pipelines.Processors;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;

namespace Informa.Library.CustomSitecore.Pipelines.HttpRequest
{
	public class ArticleItemResolver : IProcessor<HttpRequestArgs>
	{

		protected readonly IArticleSearch ArticleSearcher;
		protected readonly ISitecoreContext SitecoreContext;

		public ArticleItemResolver(IArticleSearch searcher, ISitecoreContext context)
		{
			ArticleSearcher = searcher;
			SitecoreContext = context;
		}

		public void Process(HttpRequestArgs args)
		{
			Assert.ArgumentNotNull((object)args, "args");
			if (Context.Item != null || Context.Database == null || args.Url.ItemPath.Length == 0)
				return;

			Regex r = new Regex(@"^(.*)(/home/)(sc\d{6})(/)(.*)");
			MatchCollection mc = r.Matches(args.Url.ItemPath);
			if (mc.Count < 1 || mc[0].Groups.Count < 6)
				return;

			string numFormat = mc[0].Groups[3].Value;

			//find the new article page
			IArticleSearchFilter filter = ArticleSearcher.CreateFilter();
			filter.PageSize = 1;
			filter.Page = 1;
			filter.ArticleNumber = numFormat;

			var results = ArticleSearcher.Search(filter);
			if (!results.Articles.Any())
				return;

			IArticle a = results.Articles.First();
			if (a == null)
				return;

			string matchTitle = mc[0].Groups[5].Value;
			string urlTitle = a._Name.ToLower().Replace(" ", "-");
			if (!urlTitle.Equals(matchTitle))
				HttpContext.Current.Response.RedirectPermanent(ArticleSearch.GetArticleCustomPath(a));

			Item i = SitecoreContext.GetItem<Item>(a._Id);
			if (i == null)
				return;

			Context.Item = i;
			args.Url.ItemPath = i.Paths.FullPath;
			Context.Request.ItemPath = i.Paths.FullPath;
		}
	}
}
