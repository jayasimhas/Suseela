using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Informa.Library.Article.Search;
using Informa.Models.DCD;

namespace Informa.Library.Utilities.TokenMatcher
{
    public class TokenReplacer
    {
        public IArticleSearch ArticleSearch { get; }

        public TokenReplacer(IArticleSearch articleSearch)
        {
            ArticleSearch = articleSearch;
        }
        public TokenReplacer() : this(DependencyResolver.Current.GetService<IArticleSearch>())
        {

        }

        public string ReplaceCompany(string content)
        {
            var dealRegex = new Regex(DCDConstants.DealTokenRegex);

            foreach (Match match in dealRegex.Matches(content))
            {
                var replace = DCDTokenMatchers.dealMatchEval(match);

                content = content.Replace(match.Value, replace.ToString());
            }

            return content;
        }

        public string ReplaceRelatedArticles(string content)
        {
            var referenceArticleTokenRegex = new Regex(@"\(<a>\[A#(.*?)\]</a>\)");

            foreach (Match match in referenceArticleTokenRegex.Matches(content))
            {
                string articleNumber = match.Groups[1].Value;


                IArticleSearchFilter filter = ArticleSearch.CreateFilter();
                filter.ArticleNumber = articleNumber;
                var results = ArticleSearch.Search(filter);

                HtmlString replace = new HtmlString("");

                if (results.Articles.Any())
                {
                    var article = results.Articles.FirstOrDefault();
                    if (article != null)
                        replace = new HtmlString($"<a href='{article._Url}'>{article.Navigation_Title}</a>");
                }

                content = content.Replace(match.Value, replace.ToHtmlString());
            }
            return content;
        }
    }
}
