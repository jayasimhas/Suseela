using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Glass.Mapper.Sc.Web.Mvc;
using Informa.Library.Article.Search;
using Informa.Library.Services.Global;
using Informa.Library.Utilities.TokenMatcher;
using Informa.Models.DCD;
using Informa.Web.ViewModels;
using Informa.Library.Utilities.Extensions;
using Informa.Web.ViewModels.Articles;
using Jabberwocky.Core.Caching;
using Jabberwocky.Autofac.Attributes;
using Informa.Library.DataTools;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Library.Globalization;
namespace Informa.Web.Models
{
    [AutowireService(true)]
    public interface IDependencies
    {
        IArticleSearch ArticleSearch { get; }
        ICacheProvider CacheProvider { get; }
        IArticleListItemModelFactory ArticleListableFactory { get; }
        IGlobalSitecoreService GlobalService { get; }
        IDCDTokenMatchers DCDTokenMatchers { get; }
        ITableauUtil TableauUtil { get; }
        ITextTranslator TextTranslator { get; }
    }

    public class TokenHtml<TK>
    {
        protected GlassHtmlMvc<TK> GlassHtml { get; }
        protected HtmlHelper<TK> HtmlHelper { get; }
        protected TextWriter Output { get; private set; }
        protected TK Model { get; set; }
        private IDependencies _;
     
        public TokenHtml(HtmlHelper<TK> helper, IDependencies dependencies)
        {
            _ = dependencies;            
            HtmlHelper = helper;
            GlassHtml = helper.Glass();
            Model = HtmlHelper.ViewData.Model;

        }

        public string ReplaceDeals(string content)
        {
            var dealRegex = new Regex(DCDConstants.DealTokenRegex);

            foreach (Match match in dealRegex.Matches(content))
            {
                var replace = _.DCDTokenMatchers.DealMatchEval(match);

                content = content.Replace(match.Value, replace);
            }

            return content;
        }

        public string ReplaceRelatedArticles(string content)
        {
            var referenceArticleTokenRegex = new Regex(@"\(<a>\[A#(.*?)\]</a>\)");

            foreach (Match match in referenceArticleTokenRegex.Matches(content))
            {
                string articleNumber = match.Groups[1].Value;
                string cacheKey = $"TokenRepRelated-{articleNumber}";
                string replace = _.CacheProvider.GetFromCache(cacheKey, () => BuildReplaceRelatedArticles(articleNumber));
                content = content.Replace(match.Value, replace);
            }
            return content;
        }

        public string BuildReplaceRelatedArticles(string articleNumber)
        {
            HtmlString replace = new HtmlString("");

            IArticleSearchFilter filter = _.ArticleSearch.CreateFilter();
            filter.ArticleNumbers = articleNumber.SingleToList();
            var results = _.ArticleSearch.Search(filter);

            if (results.Articles.Any())
            {
                var article = results.Articles.FirstOrDefault();
                if (article != null)
                {
                    var articleText = $" (Also see \"<a href='{article._Url}'>{WebUtility.HtmlDecode(article.Title)}</a>\" - {_.GlobalService.GetPublicationName(article._Id)}, {(article.Actual_Publish_Date > DateTime.MinValue ? article.Actual_Publish_Date.ToString("d MMM, yyyy") : "")}.)";
                    replace = new HtmlString(articleText);
                }
            }

            return replace.ToHtmlString();
        }

        public string ReplaceTableauForArticles(string content, string partialName)
        {
            var sidebarRegex = new Regex(@"\[T#(.*?)\]");

            foreach (Match match in sidebarRegex.Matches(content))
            {             
                string replace = BuildReplaceTableauArticles(Regex.Replace(Regex.Replace(match.Value, "[[T#:]", ""), "[]]", ""), partialName);
                content = content.Replace(match.Value, replace);
            }

            return content;
        }

        private string BuildReplaceTableauArticles(string TableauId, string partialName)
        {
            string TableauTicket = _.TableauUtil.GenerateSecureTicket(_.GlobalService.GetTableauItemByPath(ITableau_ConfigurationConstants.TemplateId.ToString())[ITableau_ConfigurationConstants.Server_NameFieldId], _.GlobalService.GetTableauItemByPath(ITableau_ConfigurationConstants.TemplateId.ToString())[ITableau_ConfigurationConstants.User_NameFieldId]);
            string HostUrl = _.GlobalService.GetTableauItemByPath(ITableau_ConfigurationConstants.TemplateId.ToString())[ITableau_ConfigurationConstants.Server_NameFieldId];
            Sitecore.Data.Fields.LinkField JSAPILinkField = _.GlobalService.GetTableauItemByPath(ITableau_ConfigurationConstants.TemplateId.ToString()).Fields[ITableau_ConfigurationConstants.JS_API_UrlFieldId];
            string JSAPIUrl = JSAPILinkField.Url;
            string LandingPageLinkLable = _.TextTranslator.Translate("DataTools.LandingPageLink");
            HtmlString replace = new HtmlString("");
            Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components.ITableau_Dashboard tableauItem = _.GlobalService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components.ITableau_Dashboard>(TableauId);
            //Informa.Web.ViewModels.DataTools.TableauDashboardViewModel tableauDashboardItem = (Informa.Web.ViewModels.DataTools.TableauDashboardViewModel)tableauItem;

            tableauItem.ArticleTableauTicket = TableauTicket;
            tableauItem.ArticleTableauJSAPIUrl = JSAPIUrl;
            tableauItem.ArticleTableauHostUrl = HostUrl;
            tableauItem.ArticleTableuLandingPageLinkLable = LandingPageLinkLable;
            replace = HtmlHelper.Partial(partialName, tableauItem);
            return replace.ToHtmlString();
        }


        public string ReplaceSidebarArticles(string content, string partialName)
        {
            var sidebarRegex = new Regex(@"\[Sidebar#(.*?)\]");

            foreach (Match match in sidebarRegex.Matches(content))
            {
                string articleNumber = match.Groups[1].Value;
                string cacheKey = $"TokenRepSidebar-{articleNumber}";
                string replace = _.CacheProvider.GetFromCache(cacheKey, () => BuildReplaceSidebarArticles(articleNumber, partialName));

                content = content.Replace(match.Value, replace);
            }
            return content;
        }

        private string BuildReplaceSidebarArticles(string articleNumber, string partialName)
        {
            HtmlString replace = new HtmlString("");

            IArticleSearchFilter filter = _.ArticleSearch.CreateFilter();
            filter.ArticleNumbers = articleNumber.SingleToList();
            var results = _.ArticleSearch.Search(filter);

            if (results.Articles.Any())
            {
                var article = results.Articles.FirstOrDefault();
                if (article != null)
                    replace = HtmlHelper.Partial(partialName, _.ArticleListableFactory.Create(article));
            }

            return replace.ToHtmlString();
        }

        public virtual IHtmlString RenderCompanyLink(Expression<Func<TK, string>> expression)
        {
            var fieldValue = expression.Compile()(this.Model);
            return HtmlHelper.Raw(ReplaceDeals(fieldValue));
        }

        /// <summary>
        /// Render HTML for a link
        /// 
        /// </summary>
        /// <typeparam name="T">The model type</typeparam><param name="model">The model</param><param name="field">The link field to user</param><param name="attributes">Any additional link attributes</param><param name="isEditable">Make the link editable</param><param name="contents">Content to override the default decription or item name</param>
        /// <returns/>
        public virtual IHtmlString RenderTokenBody(Expression<Func<TK, string>> expression, string partialName)
        {
            var fieldValue = expression.Compile()(this.Model);
            fieldValue = ReplaceDeals(fieldValue);
            fieldValue = ReplaceSidebarArticles(fieldValue, partialName);
            fieldValue = ReplaceTableauForArticles(fieldValue, "../Shared/Components/DataTools/TableauInArticle");
            return HtmlHelper.Raw(fieldValue);
            //return new HtmlString(this.GlassHtml.RenderLink<T>(model, field, attributes, isEditable, contents));
        }
    }


    public static class TokenExtensions
    {
        public static TokenHtml<T> TokenTransform<T>(this HtmlHelper<T> htmlHelper)
        {
            return new TokenHtml<T>(htmlHelper, DependencyResolver.Current.GetService<IDependencies>());
        }
    }
}