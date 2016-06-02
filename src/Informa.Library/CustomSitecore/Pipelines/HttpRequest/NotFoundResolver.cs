using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Jabberwocky.Glass.Autofac.Pipelines.Processors;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using Log = Sitecore.Diagnostics.Log;

namespace Informa.Library.CustomSitecore.Pipelines.HttpRequest
{
	public class NotFoundResolver : IProcessor<HttpRequestArgs>
	{
		protected readonly IArticleSearch ArticleSearcher;
		protected readonly ISitecoreContext SitecoreContext;

		public NotFoundResolver(IArticleSearch searcher, ISitecoreContext context)
		{
			ArticleSearcher = searcher;
			SitecoreContext = context;
		}

		public void Process(HttpRequestArgs args)
		{
			Assert.ArgumentNotNull(args, "args");

			if (Context.Item == null)
			{
				var excludeSites = new[]
				{
					"shell",
					"login",
					"admin",
					"service",
					"modules_shell",
					"modules_website",
					"scheduler",
					"system",
					"publisher"
				};

				if (excludeSites.Any(x => x.Equals(Context.Site.Name, StringComparison.CurrentCultureIgnoreCase)))
				{
					return;
				}

				var excludePaths = new[]
				{
					"/sitecore/",
					"/sitecore modules/",
					"/sitecore_files/",
					"/PublicPages/",
					"/assets/",
					"/upload/"
				};

				if (excludePaths.Any(x => args.LocalPath.StartsWith(x)))
				{
					return;
				}

				try
				{
					// Does the url end in a 6 digit number?
					Regex r = new Regex(@"^.*-(\d{6})$");
					Match match = r.Match(args.Url.ItemPath);

					if (!match.Success || match.Groups.Count < 2) return;

					IArticleSearchFilter filter = ArticleSearcher.CreateFilter();
					filter.PageSize = 1;
					filter.Page = 1;
					filter.EScenicID = match.Groups[1].Value;

					var results = ArticleSearcher.Search(filter);
					var article = results?.Articles?.FirstOrDefault();
					if (article == null)
						return;

					//redirect 
					string newPath = ArticleSearch.GetArticleCustomPath(article);

					args.Context.Response.Status = "301 Moved Permanently";
					args.Context.Response.AddHeader("Location", newPath);
					args.Context.Response.End();
				}
				catch (ThreadAbortException)
				{
					//This is a valid error resulting from an aborted redirect.
					//No need to log.
				}
				catch (Exception ex)
				{
					Log.Error("Could not get site configuration to serve a 404 page for the request " + args.LocalPath, ex, this);
				}
			}
		}
	}
}