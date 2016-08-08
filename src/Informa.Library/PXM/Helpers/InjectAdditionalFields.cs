using System.Linq;
using System.Text.RegularExpressions;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.StringUtils;
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
        private readonly IDependencies _dependencies;

        [AutowireService(true)]
        public interface IDependencies
        {
            IBylineMaker BylineMaker { get; }
        }
        public InjectAdditionalFields(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }


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
            var authors = article?.Authors?.ToArray();
            if (authors == null || authors.Length == 0) { return html; }

            var authorsTag = $"<pre><h3 class='authors'>{_dependencies.BylineMaker.MakePrintByLine(authors)}</h3></pre>";

            return InjectAfterRegex(RootRegex, html, authorsTag);
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