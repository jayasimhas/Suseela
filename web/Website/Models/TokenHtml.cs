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

namespace Informa.Web.Models
{
	public class TokenHtml<TK>
	{
		protected GlassHtmlMvc<TK> GlassHtml { get; }
		protected HtmlHelper<TK> HtmlHelper { get; }
		protected TextWriter Output { get; private set; }
		protected TK Model { get; set; }

		public IArticleSearch ArticleSearch { get; }
	    private readonly ICacheProvider CacheProvider; 
        private readonly IArticleListItemModelFactory _articleListableFactory;
	    private readonly IGlobalSitecoreService GlobalService;

		public TokenHtml(HtmlHelper<TK> helper,
            IGlobalSitecoreService globalService)
		{
			HtmlHelper = helper;
			GlassHtml = helper.Glass();
			Model = HtmlHelper.ViewData.Model;

			ArticleSearch = DependencyResolver.Current.GetService<IArticleSearch>();
            CacheProvider = DependencyResolver.Current.GetService<ICacheProvider>();
            _articleListableFactory = DependencyResolver.Current.GetService<IArticleListItemModelFactory>();
            GlobalService = globalService;
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

		public string ReplaceRelatedArticles(string content)
		{
			var referenceArticleTokenRegex = new Regex(@"\(<a>\[A#(.*?)\]</a>\)");

			foreach (Match match in referenceArticleTokenRegex.Matches(content))
			{
				string articleNumber = match.Groups[1].Value;
                string cacheKey = $"TokenRepRelated-{articleNumber}";
                string replace = CacheProvider.GetFromCache(cacheKey, () => BuildReplaceRelatedArticles(articleNumber));

				content = content.Replace(match.Value, replace);
            }
			return content;
		}

	    public string BuildReplaceRelatedArticles(string articleNumber)
	    {
            HtmlString replace = new HtmlString("");

            IArticleSearchFilter filter = ArticleSearch.CreateFilter();
            filter.ArticleNumbers = articleNumber.SingleToList();
            var results = ArticleSearch.Search(filter);

            if (results.Articles.Any()) {
                var article = results.Articles.FirstOrDefault();
                if (article != null)
                {
                    var articleText = $" (Also see \"<a href='{article._Url}'>{WebUtility.HtmlDecode(article.Title)}</a>\" - {GlobalService.GetPublicationName(article._Id)}, {(article.Actual_Publish_Date > DateTime.MinValue ? article.Actual_Publish_Date.ToString("d MMM, yyyy") : "")}.)";
                    replace = new HtmlString(articleText);
                }
            }

	        return replace.ToHtmlString();
	    }

		public string ReplaceSidebarArticles(string content, string partialName)
		{
			var sidebarRegex = new Regex(@"\[Sidebar#(.*?)\]");

			foreach (Match match in sidebarRegex.Matches(content))
			{
				string articleNumber = match.Groups[1].Value;
                string cacheKey = $"TokenRepSidebar-{articleNumber}";
                string replace = CacheProvider.GetFromCache(cacheKey, () => BuildReplaceSidebarArticles(articleNumber, partialName));
                
				content = content.Replace(match.Value, replace);
			}
			return content;
		}

	    private string BuildReplaceSidebarArticles(string articleNumber, string partialName)
	    {
            HtmlString replace = new HtmlString("");

            IArticleSearchFilter filter = ArticleSearch.CreateFilter();
            filter.ArticleNumbers = articleNumber.SingleToList();
            var results = ArticleSearch.Search(filter);
            
            if (results.Articles.Any()) {
                var article = results.Articles.FirstOrDefault();
                if (article != null)
                    replace = HtmlHelper.Partial(partialName, _articleListableFactory.Create(article));
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

			ReplaceRelatedArticles(fieldValue);

			return HtmlHelper.Raw(fieldValue);
			//return new HtmlString(this.GlassHtml.RenderLink<T>(model, field, attributes, isEditable, contents));
		}
	}


	public static class TokenExtensions
	{
		public static TokenHtml<T> TokenTransform<T>(this HtmlHelper<T> htmlHelper)
		{
            return new TokenHtml<T>(htmlHelper, DependencyResolver.Current.GetService<IGlobalSitecoreService>());
		}
	}
}