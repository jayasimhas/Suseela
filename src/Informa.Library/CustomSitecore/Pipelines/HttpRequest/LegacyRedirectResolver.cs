using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Library.Logging;
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
	    protected readonly ILogWrapper Logger;

        public LegacyRedirectResolver(
            IArticleSearch searcher, 
            ISitecoreContext context,
            ILogWrapper logger)
		{
			ArticleSearcher = searcher;
			SitecoreContext = context;
            Logger = logger;
		}

        private string[] excludeSites = new[]
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

        private string[] excludePaths = new[]
		{
			"/sitecore/",
			"/sitecore modules/",
			"/sitecore_files/",
			"/PublicPages/",
			"/assets/",
			"/upload/"
		};

        public void Process(HttpRequestArgs args)
		{
            Logger.SitecoreInfo($"LegacyRedirectResolver started");
            Assert.ArgumentNotNull(args, "args");

		    if ((Context.Item != null)
                || (excludeSites.Any(x => x.Equals(Context.Site.Name, StringComparison.CurrentCultureIgnoreCase)))
                || (excludePaths.Any(x => args.LocalPath.StartsWith(x))))
		        return;
		
			try
			{
				// Does the url end in a 6 digit number?
				Regex r = new Regex(@"^.*-(\d{6})$");
				Match match = r.Match(args.Url.ItemPath);

			    IArticleSearchResults results = (!match.Success || match.Groups.Count < 2)
			        ? GetResultsByPath(args.Url.FilePath)
                    : GetResultsByLegacyID(match.Groups[1].Value);
                
				IArticle article = results?.Articles?.FirstOrDefault();
				if (article == null) {
                    Logger.SitecoreInfo("LegacyRedirectResolver article not found");
                    return;
				}

				string newPath = ArticleSearch.GetArticleCustomPath(article);
                Logger.SitecoreInfo($"LegacyRedirectResolver article path: {newPath}");
                
				var item = Context.Database.GetItem(new ID(article._Id));
				var domainUri = new Uri(LinkManager.GetItemUrl(item, GetUrlOptions()));
				var protocol = Sitecore.Configuration.Settings.GetSetting("Site.Protocol", "https");

                Logger.SitecoreInfo($"LegacyRedirectResolver article url: {protocol}://{domainUri.Host}{newPath}");

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
                Logger.SitecoreError("Could not get site configuration to serve a 404 page for the request " + args.LocalPath, ex, this);
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
            IArticleSearchFilter filter = ArticleSearcher.CreateFilter();
            filter.PageSize = 1;
            filter.Page = 1;
            filter.EScenicID = legacyID;

            var results = ArticleSearcher.Search(filter);

            Logger.SitecoreInfo($"LegacyRedirectResolver pattern request: {legacyID}");

	        return results;
	    }

	    private UrlOptions GetUrlOptions()
	    {
            var options = LinkManager.GetDefaultUrlOptions();
            options.SiteResolving = true;
            options.AlwaysIncludeServerUrl = true;

	        return options;
;	    }
	}
}
