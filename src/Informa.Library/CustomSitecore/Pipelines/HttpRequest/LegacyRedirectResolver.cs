using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Library.Logging;
using Informa.Library.Services.Global;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Pipelines.Processors;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Pipelines.HttpRequest;

namespace Informa.Library.CustomSitecore.Pipelines.HttpRequest
{
	public class LegacyRedirectResolver : IProcessor<HttpRequestArgs>
	{
		protected readonly IArticleSearch ArticleSearcher;
		protected readonly ILogWrapper Logger;
		protected readonly ISitecoreContext SitecoreContext;
	    protected readonly IGlobalSitecoreService GlobalService;

		private readonly string[] excludePaths =
		{
			"/sitecore/",
			"/sitecore modules/",
			"/sitecore_files/",
			"/PublicPages/",
			"/assets/",
			"/upload/"
		};

		public LegacyRedirectResolver(
			IArticleSearch searcher,
			ISitecoreContext context,
			ILogWrapper logger,
            IGlobalSitecoreService globalService)
		{
			ArticleSearcher = searcher;
			SitecoreContext = context;
			Logger = logger;
		    GlobalService = globalService;

		}

		public void Process(HttpRequestArgs args)
		{
			Logger.SitecoreInfo($"LegacyRedirectResolver started");
			Assert.ArgumentNotNull(args, "args");

			if ((Context.Item != null)
			    || Context.Site.Name != "LegacyRedirect"
			    || excludePaths.Any(x => args.LocalPath.StartsWith(x)))
				return;

			try
			{
				// Does the url end in a 6 digit number?
				var r = new Regex(@"^.*-(\d{6})$");
				var match = r.Match(args.Url.ItemPath);

				var results = !match.Success || match.Groups.Count < 2
					? GetResultsByPath(args.Url.FilePath)
					: GetResultsByLegacyID(match.Groups[1].Value);

			    var article = results?.Articles?.FirstOrDefault();
			    if (article == null)
			    {
			        Logger.SitecoreInfo("LegacyRedirectResolver article not found");
			        return;
			    }
			    else
			    {
			        Logger.SitecoreInfo($"LegacyRedirectResolver article found: {article._Path}"); 
			    }

				var newPath = ArticleSearch.GetArticleCustomPath(article);
				Logger.SitecoreInfo($"LegacyRedirectResolver article path: {newPath}");

				var siteRoot = GlobalService.GetSiteRootAncestor(article._Id);
			    if (siteRoot == null)
			    {
			        Logger.SitecoreInfo($"LegacyRedirectResolver didn't find site root for: {article._Path}");
			        return;
			    }
			    else
			    {
                    Logger.SitecoreInfo($"LegacyRedirectResolver did find site root for: {article._Path}");
                }


                var siteInfo = Factory.GetSiteInfoList().FirstOrDefault(a => a.RootPath.Equals(siteRoot._Path));
			    if (siteInfo == null)
			    {
			        Logger.SitecoreInfo($"LegacyRedirectResolver couldn't find site info match for: {siteRoot._Path}");
			        return;
			    }
			    else
			    {
                    Logger.SitecoreInfo($"LegacyRedirectResolver found site info match: {siteInfo.Name}");
                }

			    var host = siteInfo.TargetHostName;
                var protocol = Settings.GetSetting("Site.Protocol", "https");

				Logger.SitecoreInfo($"LegacyRedirectResolver article url: {protocol}://{host}{newPath}");

				args.Context.Response.Status = "301 Moved Permanently";
				args.Context.Response.AddHeader("Location", $"{protocol}://{host}{newPath}");
				args.Context.Response.End();
			}
			catch (ThreadAbortException)
			{
				//This is a valid error resulting from an aborted redirect.
				//No need to log.
			}
			catch (Exception ex)
			{
				Logger.SitecoreError("Could not get site configuration to serve a 404 page for the request " + args.LocalPath, ex,
					this);
			}
		}

		public IArticleSearchResults GetResultsByPath(string filePath)
		{
			// Get legacy article path
			var basePath = "/sitecore/content/Home";
			var path = $"{basePath}{filePath}".Replace("-", " ");
			var results = ArticleSearcher.GetLegacyArticleUrl(path);

			Logger.SitecoreInfo($"LegacyRedirectResolver path request: {path}");

			return results;
		}

		public IArticleSearchResults GetResultsByLegacyID(string legacyID)
		{
			var filter = ArticleSearcher.CreateFilter();
			filter.PageSize = 1;
			filter.Page = 1;
			filter.EScenicID = legacyID;

			var results = ArticleSearcher.Search(filter);

			Logger.SitecoreInfo($"LegacyRedirectResolver pattern request: {legacyID}");

			return results;
		}
	}
}