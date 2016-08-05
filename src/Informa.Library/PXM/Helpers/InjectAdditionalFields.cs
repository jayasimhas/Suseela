using System.Linq;
using System.Text.RegularExpressions;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.PXM.Helpers
{
    public interface IInjectAdditionalFields
    {
        string InjectIntoHtml(string html, IArticle article);
    }

    [AutowireService]
    public class InjectAdditionalFields : IInjectAdditionalFields
    {
        private const string RootRegex = @"<\s*div .*?class\s*=\s*['""]root['""].*?>";

        public string InjectIntoHtml(string html, IArticle article)
        {
            //Order is important here.  
            //Both inject right after <div class="root">
            var result = InjectAuthors(html, article);
            result = InjectTitles(result, article);
            return result;
        }

        public string InjectAuthors(string html, IArticle article)
        {
            var authorsTag = $"<pre><h3 class='authors'>{GetAuthorsFormatted(article)}</h3></pre>";

            return InjectAfterRegex(RootRegex, html, authorsTag);
        }

        public string GetAuthorsFormatted(IArticle article)
        {
            if(article?.Authors == null) { return string.Empty; }

            var formattedStrings =
                article.Authors.Select(a => $"{a.First_Name} {a.Last_Name} {a.Email_Address}");

            return string.Join(" ", formattedStrings);
        }

        public string InjectTitles(string html, IArticle article)
        {
            var titles = GetTitlesHtml(article);
            return InjectAfterRegex(RootRegex, html, titles);
        }

        public string GetTitlesHtml(IArticle article)
        {
            var html = string.Empty;
            if (article.Title.HasContent())
            {
                html += $"<pre><h1 class=\"title\">{article.Title}</h1></pre>";
            }
            if (article.Sub_Title.HasContent())
            {
                html += $"<pre><h2 class=\"subtitle\">{article.Sub_Title}</h2></pre>";
            }

            return html;
        }

        public string InjectAfterRegex(string regex, string initialHtml, string htmlToInject)
        {
            regex = $"({regex})";
            htmlToInject = $"$1{htmlToInject}";
            var rx = new Regex(regex, RegexOptions.IgnoreCase);
            return rx.Replace(initialHtml, htmlToInject);
        }
    }
}