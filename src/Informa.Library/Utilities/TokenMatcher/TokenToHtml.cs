using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using HtmlAgilityPack;
using Informa.Library.Article.Search;
using Informa.Library.Services.Article;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.StringUtils;
using Informa.Models.DCD;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Core.Caching;

namespace Informa.Library.Utilities.TokenMatcher
{
	public interface ITokenToHtml
	{
		string ReplaceAllTokens(string content);
		string ReplaceSidebarArticles(string content);
		string ReplaceRelatedArticles(string content);
		string ReplaceDeals(string content);
		string ReplaceCompanies(string content);
	}

	[AutowireService]
	public class TokenToHtml : ITokenToHtml
	{
		private readonly IDependencies _dependencies;
		private readonly string OldCompaniesUrl = Sitecore.Configuration.Settings.GetSetting("DCD.OldCompaniesURL");

		[AutowireService(IsAggregateService = true)]
		public interface IDependencies
		{
			IArticleService ArticleService { get; }
            IArticleSearch ArticleSearch { get; }
			ICacheProvider CacheProvider { get; }
			IBylineMaker ByLineMaker { get; }
        }

		public TokenToHtml(IDependencies dependencies)
		{
			_dependencies = dependencies;
		}

		public string ReplaceAllTokens(string content)
		{
			var result = ReplaceSidebarArticles(content);
			result = ReplaceRelatedArticles(result);
			result = ReplaceCompanies(result);
			result = ReplaceDeals(result);
			return result;
		}

		public string ReplaceSidebarArticles(string content)
		{
			var sidebarRegex = new Regex(@"\[Sidebar#(.*?)\]");

			foreach (Match match in sidebarRegex.Matches(content))
			{
				string articleNumber = match.Groups[1].Value;
				string cacheKey = $"TokenRepSidebar-{articleNumber}";
				string replace = _dependencies.CacheProvider.GetFromCache(cacheKey, () => BuildReplaceSidebarArticles(articleNumber));

				content = content.Replace(match.Value, replace);
			}
			return content;
		}

		private string BuildReplaceSidebarArticles(string articleNumber)
		{
			IArticleSearchFilter filter = _dependencies.ArticleSearch.CreateFilter();
			filter.ArticleNumbers = articleNumber.SingleToList();
			var results = _dependencies.ArticleSearch.Search(filter);
			var replace = new HtmlString("");
			if (results.Articles.Any())
			{
				var article = results.Articles.FirstOrDefault();
				if (article != null)
				{
					replace = BuildSideArticleHtml(article);
				}
			}
			return replace.ToHtmlString();
		}

		private HtmlString BuildSideArticleHtml(IArticle article)
		{
			var readAllArticle = "Read the full article here";
			var document = new HtmlDocument();
			document.LoadHtml("<aside class=\"article-sidebar\"><h3></h3><span class=\"article-sidebar__byline\"></span><time class=\"article-sidebar__date\"></time><div></div><a class=\"article-sidebar__read-more click-utag\" href=\"\"></a></aside>");

			var root = document.DocumentNode.FirstChild;
			// Set title
			document.DocumentNode.SelectSingleNode("//h3").InnerHtml = HttpUtility.HtmlDecode(article.Title);
			// Set author
			var span = document.DocumentNode.SelectSingleNode("//span");
			var author = _dependencies.ByLineMaker.MakeByline(article.Authors);
            if (!string.IsNullOrEmpty(author))
			{
				span.InnerHtml = author;
			}
			else
			{
				span.Remove();
			}
			// Set time
			var time = document.DocumentNode.SelectSingleNode("//time");
			time.InnerHtml = article.Actual_Publish_Date.ToString("dd MMM yyyy");
			// Set linkable url
			var a = document.DocumentNode.SelectSingleNode("//a");
			a.Attributes["href"].Value = article._Url;
			a.InnerHtml = readAllArticle;
			// Add summary
			var div = document.DocumentNode.SelectSingleNode("//div");
			div.InnerHtml = WordUtil.TruncateArticle(_dependencies.ArticleService.GetArticleSummary(article), 60);

			return new HtmlString(document.DocumentNode.OuterHtml);
		}

		public string ReplaceRelatedArticles(string content)
		{
			var referenceArticleTokenRegex = new Regex(@"\(<a>\[A#(.*?)\]</a>\)");

			foreach (Match match in referenceArticleTokenRegex.Matches(content))
			{
				string articleNumber = match.Groups[1].Value;
				string cacheKey = $"TokenRepRelated-{articleNumber}";
				string replace = _dependencies.CacheProvider.GetFromCache(cacheKey, () => BuildReplaceRelatedArticles(articleNumber));

				content = content.Replace(match.Value, replace);
			}
			return content;
		}

		private string BuildReplaceRelatedArticles(string articleNumber)
		{
			HtmlString replace = new HtmlString("");

			IArticleSearchFilter filter = _dependencies.ArticleSearch.CreateFilter();
			filter.ArticleNumbers = articleNumber.SingleToList();
			var results = _dependencies.ArticleSearch.Search(filter);

			if (results.Articles.Any())
			{
				var article = results.Articles.FirstOrDefault();
				if (article != null)
				{
					var articleText = $" (Also see \"<a href='{article._Url}'>{WebUtility.HtmlDecode(article.Title)}</a>\" - {"Scrip"}, {(article.Actual_Publish_Date > DateTime.MinValue ? article.Actual_Publish_Date.ToString("d MMM, yyyy") : "")}.)";
					replace = new HtmlString(articleText);
				}
			}

			return replace.ToHtmlString();
		}

		public string ReplaceDeals(string content)
		{
			var dealRegex = new Regex(DCDConstants.DealTokenRegex);

			foreach (Match match in dealRegex.Matches(content))
			{
				var replace = DCDTokenMatchers.DealMatchEval(match);

				content = content.Replace(match.Value, replace);
			}

			return content;
		}

		public string ReplaceCompanies(string content)
		{
			//Find all matches with Company token
			Regex regex = new Regex(DCDConstants.CompanyTokenRegex);

			var matchSet = new HashSet<string>();
			var matches = regex.Matches(content);
			foreach (Match match in matches)
			{
				var replace = string.Empty;
				// Replace the first occurrence with hyperlink
				if (!matchSet.Contains(match.Groups[1].Value))
				{
					replace = $"<a href=\"{string.Format(OldCompaniesUrl, match.Groups[1].Value.Split(':')[0])}\">{match.Groups[1].Value.Split(':')[1]}</a>";
					content = regex.Replace(content, replace, 1);
					matchSet.Add(match.Groups[1].Value);
				}
				// Replace other occurrences with normal names
				else
				{
					replace = match.Groups[1].Value.Split(':')[1];
					content = content.Replace(match.Value, replace);
				}
			}
			return content;
		}
	}
}