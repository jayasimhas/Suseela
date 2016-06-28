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

				var notFoundItem = Context.Database.GetItem($"{Context.Site.StartPath}/404");

				if (notFoundItem != null)
				{
					Context.Item = notFoundItem;
				}
			}
		}
	}
}