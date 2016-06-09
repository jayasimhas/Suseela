using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Pipelines.Processors;
using Sitecore;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Pipelines.HttpRequest;
using Log = Sitecore.Diagnostics.Log;

namespace Informa.Library.CustomSitecore.Pipelines.HttpRequest
{
	public class LegacyRedirectResolver : IProcessor<HttpRequestArgs>
	{
		protected readonly IArticleSearch ArticleSearcher;
		protected readonly ISitecoreContext SitecoreContext;

		public LegacyRedirectResolver(IArticleSearch searcher, ISitecoreContext context)
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

					IArticleSearchResults results;
					// If it is a pmbi legacy article url
					if (!match.Success || match.Groups.Count < 2)
					{
						// Get legacy article path
						var basePath = "/sitecore/content/Home";
						var path = $"{basePath}{args.Url.FilePath}";
						results = ArticleSearcher.GetLegacyArticleUrl(path);
					}
					else
					{
						IArticleSearchFilter filter = ArticleSearcher.CreateFilter();
						filter.PageSize = 1;
						filter.Page = 1;
						filter.EScenicID = match.Groups[1].Value;

						results = ArticleSearcher.Search(filter);
					}

					//redirect 
					IArticle article = results?.Articles?.FirstOrDefault();

					if (article == null)
					{
						return;
					}

					string newPath = ArticleSearch.GetArticleCustomPath(article);

					var options = LinkManager.GetDefaultUrlOptions();
					options.SiteResolving = true;
					options.AlwaysIncludeServerUrl = true;

					var item = Context.Database.GetItem(new ID(article._Id));
					var domainUri = new Uri(LinkManager.GetItemUrl(item, options));
					var protocol = Sitecore.Configuration.Settings.GetSetting("Site.Protocol", "https");

					args.Context.Response.Status = "301 Moved Permanently";
					args.Context.Response.AddHeader("Location", $"{protocol}://{domainUri.Host}{newPath}");
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
