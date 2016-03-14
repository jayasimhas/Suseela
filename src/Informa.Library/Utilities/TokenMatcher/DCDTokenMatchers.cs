using Informa.Model.DCD;
using Informa.Models.DCD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Informa.Library.Article.Search;

namespace Informa.Library.Utilities.TokenMatcher
{
    public static class DCDTokenMatchers
    {
        public static string ProcessDCDTokens(string text)
        {
            string body = text;

            try
            {
                string tempText = processCompanyTokens(body);

                if (string.IsNullOrEmpty(tempText) == false)
                    body = tempText;

                tempText = processDealTokens(body);

                if (string.IsNullOrEmpty(tempText) == false)
                    body = tempText;

                tempText = processArticleTokens(body);

                if (string.IsNullOrEmpty(tempText) == false)
                    body = tempText;
                
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error ProcessingDCDTokens", ex);
            }
            return body;
        }

        private static string processCompanyTokens(string text)
        {
            //Find all matches with Company token
            Regex regex = new Regex(DCDConstants.CompanyTokenRegex);

            MatchEvaluator evaluator = new MatchEvaluator(companyMatchEval);
            return regex.Replace(text, evaluator);
        }

        private static string processDealTokens(string text)
        {
            //Find all matches with Deal token
            Regex regex = new Regex(DCDConstants.DealTokenRegex);

            MatchEvaluator evaluator = new MatchEvaluator(dealMatchEval);
            return regex.Replace(text, evaluator);
        }

        private static string processArticleTokens(string text)
        {
            //Find all matches with Company token
            Regex regex = new Regex(DCDConstants.ArticleTokenRegex);

            MatchEvaluator evaluator = new MatchEvaluator(articleMatchEval);
            return regex.Replace(text, evaluator);
        }

        private static string companyMatchEval(Match match)
        {
            try
            {
                //return a strong link of the company name (from the token itself) to replace the token
                return string.Format("<a href=\"{0}\">{1}</a>", string.Format(Sitecore.Configuration.Settings.GetSetting("DCD.OldCompaniesURL"), match.Groups[1].Value.Split(':')[0]), match.Groups[1].Value.Split(':')[1]);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error when evaluating company match token", ex, "LogFileAppender");
                return string.Empty;
            }
        }

        public static string dealMatchEval(Match match)
        {
            try
            {
                //return a see deal (deal reference) (from the token itself) to replace the token
                return string.Format("[<a href=\"{0}\">See Deal</a>]", string.Format(Sitecore.Configuration.Settings.GetSetting("DCD.OldDealsURL"), match.Groups[1].Value));
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error when evaluating deal match token", ex, "LogFileAppender");
                return string.Empty;
            }
        }

        private static string articleMatchEval(Match match)
        {
            try
            {
                string articleNumber = match.Groups[1].Value;
                
                IArticleSearch ArticleSearch = DependencyResolver.Current.GetService<IArticleSearch>();
                IArticleSearchFilter filter = ArticleSearch.CreateFilter();
                filter.ArticleNumber = articleNumber;
                var results = ArticleSearch.Search(filter);

                HtmlString replace = new HtmlString("");

                if (results.Articles.Any())
                {
                    var article = results.Articles.FirstOrDefault();
                    if (article != null)
                        return string.Format("<a href='{0}'>{1}</a>", article._Url, HttpUtility.HtmlDecode(article.Navigation_Title));
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error when evaluating company match token", ex, "LogFileAppender");
                return string.Empty;
            }
            return string.Empty;
        }
    }
}
