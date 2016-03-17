using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using Informa.Library.Article.Search;
using Informa.Library.Utilities.TokenMatcher;
using Informa.Models.DCD;
using Informa.Web.ViewModels;

namespace Informa.Web.Models
{
	public class TokenHtml<TK>
	{
		protected GlassHtmlMvc<TK> GlassHtml { get; }
		protected HtmlHelper<TK> HtmlHelper { get; }
		protected TextWriter Output { get; private set; }
		protected TK Model { get; set; }

		public ISitecoreContext SitecoreContext => GlassHtml.SitecoreContext;
		public IArticleSearch ArticleSearch { get; }
		private TokenReplacer TokenReplacer { get; }

		private readonly IArticleListItemModelFactory _articleListableFactory;

		public TokenHtml(HtmlHelper<TK> helper)
		{
			HtmlHelper = helper;
			GlassHtml = helper.Glass();
			Model = HtmlHelper.ViewData.Model;

			ArticleSearch = DependencyResolver.Current.GetService<IArticleSearch>();
			_articleListableFactory = DependencyResolver.Current.GetService<IArticleListItemModelFactory>();

            TokenReplacer = new TokenReplacer(ArticleSearch);

        }

		
		public string ReplaceSidebarArticles(string content, string partialName)
		{
			var sidebarRegex = new Regex(@"\[Sidebar#(.*?)\]");

			foreach (Match match in sidebarRegex.Matches(content))
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
						replace = HtmlHelper.Partial(partialName, _articleListableFactory.Create(article));
				}

				content = content.Replace(match.Value, replace.ToString());
			}
			return content;
		}

		public virtual IHtmlString RenderCompanyLink(Expression<Func<TK, string>> expression)
		{
			var fieldValue = expression.Compile()(this.Model);
			return HtmlHelper.Raw(TokenReplacer.ReplaceCompany(fieldValue));
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
			fieldValue = TokenReplacer.ReplaceCompany(fieldValue);

			fieldValue = ReplaceSidebarArticles(fieldValue, partialName);

            TokenReplacer.ReplaceRelatedArticles(fieldValue);

			return HtmlHelper.Raw(fieldValue);
			//return new HtmlString(this.GlassHtml.RenderLink<T>(model, field, attributes, isEditable, contents));
		}
	}


	public static class TokenExtensions
	{
		public static TokenHtml<T> TokenTransform<T>(this HtmlHelper<T> htmlHelper)
		{
			return new TokenHtml<T>(htmlHelper);
		}
	}
}