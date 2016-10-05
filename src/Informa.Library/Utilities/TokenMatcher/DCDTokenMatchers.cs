using Informa.Models.DCD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Informa.Library.Article.Search;
using Informa.Library.Services.Global;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Core.Caching;

namespace Informa.Library.Utilities.TokenMatcher
{
	public static class DCDTokenMatchers
	{
		private static readonly Lazy<IArticleSearch> _lazySearch = new Lazy<IArticleSearch>(() => DependencyResolver.Current.GetService<IArticleSearch>());
		private static IArticleSearch ArticleSearch => _lazySearch.Value;
		private static readonly Lazy<ICacheProvider> _lazyCache = new Lazy<ICacheProvider>(() => DependencyResolver.Current.GetService<ICacheProvider>());
		private static ICacheProvider CacheProvider => _lazyCache.Value;
		private static readonly string OldCompaniesUrl = Sitecore.Configuration.Settings.GetSetting("DCD.OldCompaniesURL");
		private static readonly string OldDealsUrl = Sitecore.Configuration.Settings.GetSetting("DCD.OldDealsURL");
		private static IGlobalSitecoreService GlobalService = null;

		static DCDTokenMatchers()
		{
			try {
				GlobalService = DependencyResolver.Current.GetService<IGlobalSitecoreService>(); 
			} catch { }
		}

		public static string ProcessDCDTokens(string text)
		{
			string body = text;

		    try
		    {
		        string tempText = ProcessCompanyTokens(body);

		        if (string.IsNullOrEmpty(tempText) == false)
		            body = tempText;
		    }
		    catch (Exception ex1)
		    {
                Sitecore.Diagnostics.Log.Error("Error Processing DCD Company Tokens", ex1, typeof(DCDTokenMatchers));
            }

		    try
		    {
		        string tempText = ProcessDealTokens(body);

		        if (string.IsNullOrEmpty(tempText) == false)
		            body = tempText;
		    }
		    catch (Exception ex2)
		    {
                Sitecore.Diagnostics.Log.Error("Error Processing DCD Deal Tokens", ex2, typeof(DCDTokenMatchers));
            }

            try { 
                string tempText = ProcessArticleTokens(body);

				if (string.IsNullOrEmpty(tempText) == false)
					body = tempText;

			}
			catch (Exception ex3)
			{
				Sitecore.Diagnostics.Log.Error("Error Processing DCD Article Tokens", ex3, typeof(DCDTokenMatchers));
			}

			return body;
		}

		private static string ProcessCompanyTokens(string text)
		{
			//Find all matches with Company token
			Regex regex = new Regex(DCDConstants.CompanyTokenRegex);

			var matchSet = new HashSet<string>();
			var matches = regex.Matches(text);
            foreach (Match match in matches)
            {
	            var replace = string.Empty;
                if (match.Groups.Count < 2)
                    continue;

                string matchValue = match.Groups[1].Value;
                string[] splitArr = matchValue.Split(':');
                string cDigit = (splitArr.Length > 0) ? splitArr[0] : string.Empty;
                string cName = (splitArr.Length > 1) ? splitArr[1] : string.Empty;

                // Replace the first occurrence with hyperlink
                if (!matchSet.Contains(matchValue))
                {
                    replace = $"<a href=\"{string.Format(OldCompaniesUrl, cDigit)}\">{cName}</a>";
					text = regex.Replace(text, replace, 1);
					matchSet.Add(matchValue);
				}
				// Replace other occurrences with normal names
				else
				{
					text = text.Replace(match.Value, cName);
				}			
			}
			return text;
		}

		private static string ProcessDealTokens(string text)
		{
			//Find all matches with Deal token
			Regex regex = new Regex(DCDConstants.DealTokenRegex);

			MatchEvaluator evaluator = new MatchEvaluator(DealMatchEval);
			return regex.Replace(text, evaluator);
		}

		private static string ProcessArticleTokens(string text)
		{
			//Find all matches with Article token
			Regex regex = new Regex(DCDConstants.ArticleTokenRegex);

			MatchEvaluator evaluator = new MatchEvaluator(ArticleMatchEval);
			var replacedText = regex.Replace(text, evaluator);

			Regex legacyRegex = new Regex(DCDConstants.LegacyArticleTokenRegex);
			return legacyRegex.Replace(replacedText, evaluator);
		}

		private static string CompanyMatchEval(Match match)
		{
			try
			{
				//return a strong link of the company name (from the token itself) to replace the token
				return string.Format("<a href=\"{0}\">{1}</a>", string.Format(OldCompaniesUrl, match.Groups[1].Value.Split(':')[0]), match.Groups[1].Value.Split(':')[1]);
			}
			catch (Exception ex)
			{
				Sitecore.Diagnostics.Log.Error("Error when evaluating company match token", ex, "LogFileAppender");
				return string.Empty;
			}
		}

		public static string DealMatchEval(Match match)
		{
			try
			{
				if (match == null || match.Groups.Count < 2)
				{
					return string.Empty;
				}
				//return a see deal (deal reference) (from the token itself) to replace the token
				return string.Format("<a href=\"{0}\">[See Deal]</a>", string.Format(OldDealsUrl, match.Groups[1].Value));
			}
			catch (Exception ex)
			{
				Sitecore.Diagnostics.Log.Error($"Error when evaluating deal match token. Match: {match.Value}", ex, "LogFileAppender");
				return string.Empty;
			}
		}

		private static string ArticleMatchEval(Match match)
		{
			string articleNumber = match.Groups[1].Value;
			string cacheKey = $"DCDArticleText-{articleNumber}";
            return CacheProvider.GetFromCache(cacheKey, () => BuildArticleMatch(articleNumber));
		}

	    private static string BuildArticleMatch(string articleNumber)
	    {
            try {

                IArticleSearchFilter filter = ArticleSearch.CreateFilter();
                filter.ArticleNumbers = articleNumber.SingleToList();
                filter.PageSize = 1;
                var results = ArticleSearch.Search(filter);

                if (results.Articles.Any()) {
                    var article = results.Articles.FirstOrDefault();

                    if (article != null) {
                        return
                            $" (Also see \"<a href='{article._Url}'>{WebUtility.HtmlDecode(article.Title)}</a>\" - {GlobalService?.GetPublicationName(article._Id)}, " +
                            $"{(article.Actual_Publish_Date > DateTime.MinValue ? article.Actual_Publish_Date.ToString("d MMM, yyyy") : "")}.)";
                    }
                }
            } catch (Exception ex) {
                Sitecore.Diagnostics.Log.Error("Error when evaluating company match token", ex, "LogFileAppender");
            }

	        return string.Empty;
	    }

        /// <summary>
        /// This will replace company and product tokens like this: [Company Name] or {Product Name} with <b>Company Name</b> or <i>Product Name</i>
        /// </summary>
        /// <returns></returns>
        public static string ReplaceDealNameTokens(string text) {

            Regex companyRegex = new Regex(DCDConstants.DealCompanyNameRegex);
            foreach(Match cm in companyRegex.Matches(text)) {
                text = text.Replace(cm.Value, $"<strong>{cm.Groups[1].Value}</strong>");
            }

            Regex productRegex = new Regex(DCDConstants.DealProductNameRegext);
            foreach (Match pm in productRegex.Matches(text)) {
                text = text.Replace(pm.Value, $"<em>{pm.Groups[1].Value}</em>");
            }

            return text;
        }
	}
}
