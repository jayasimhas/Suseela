using Informa.Library.Article.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.ContentCuration;
using Informa.Library.Globalization;
using Jabberwocky.Glass.Autofac.Mvc.Services;

namespace Informa.Web.ViewModels
{
	public class LatestNewsViewModel : GlassViewModel<IGlassBase>
	{
		protected readonly IRenderingContextService RenderingParametersService;
		protected readonly IArticleSearch ArticleSearch;
		protected readonly IItemManuallyCuratedContent ItemManuallyCuratedContent;
		protected readonly IArticleListItemModelFactory ArticleListableFactory;
		protected readonly ITextTranslator TextTranslator;

		public LatestNewsViewModel(
			IRenderingContextService renderingParametersService,
			IArticleSearch articleSearch,
			IItemManuallyCuratedContent itemManuallyCuratedContent,
			IArticleListItemModelFactory articleListableFactory,
			ITextTranslator textTranslator)
		{
			RenderingParametersService = renderingParametersService;
			ArticleSearch = articleSearch;
			ItemManuallyCuratedContent = itemManuallyCuratedContent;
			ArticleListableFactory = articleListableFactory;
			TextTranslator = textTranslator;

			Parameters = RenderingParametersService.GetCurrentRenderingParameters<ILatest_News_Options>();
		}

		public IEnumerable<string> Topics => Parameters.Subjects.Select(s => s._Name);

		public IEnumerable<IListableViewModel> News
		{
			get
			{
				var manuallyCuratedContent = ItemManuallyCuratedContent.Get(GlassModel._Id);
				var filter = ArticleSearch.CreateFilter();

				filter.Page = 1;
				filter.PageSize = ArticlesToDisplay;
				filter.ExcludeManuallyCuratedItems.AddRange(manuallyCuratedContent);
				filter.TaxonomyIds.AddRange(Parameters.Subjects.Select(s => s._Id));
				
				var results = ArticleSearch.Search(filter);
				var articles = results.Articles.Select(a => ArticleListableFactory.Create(a)).Where(a => a != null).ToList();

				articles.ForEach(a => a.DisplayImage = false);

				return articles;
			}
		}

		public int ArticlesToDisplay => Parameters.Number_To_Display?.Value ?? 6;

		public string TitleText => TextTranslator.Translate("Article.LatestFrom");

		public bool DisplayTitle => Parameters.Display_Title;

		public ILatest_News_Options Parameters { get; set; }
	}
}