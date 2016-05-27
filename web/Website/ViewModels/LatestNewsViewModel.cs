using System;
using Informa.Library.Article.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc.Fields;
using Informa.Library.ContentCuration;
using Informa.Library.Globalization;
using Informa.Library.Search.Utilities;
using Jabberwocky.Glass.Autofac.Mvc.Services;

namespace Informa.Web.ViewModels
{
	public class LatestNewsViewModel : GlassViewModel<IGlassBase>
	{
		protected readonly IArticleSearch ArticleSearch;
		protected readonly IItemManuallyCuratedContent ItemManuallyCuratedContent;
		protected readonly IArticleListItemModelFactory ArticleListableFactory;
		
		public LatestNewsViewModel(IGlassBase datasource,
			IRenderingContextService renderingParametersService,
			IArticleSearch articleSearch,
			IItemManuallyCuratedContent itemManuallyCuratedContent,
			IArticleListItemModelFactory articleListableFactory,
			ITextTranslator textTranslator)
		{
			ArticleSearch = articleSearch;
			ItemManuallyCuratedContent = itemManuallyCuratedContent;
			ArticleListableFactory = articleListableFactory;
			
			var parameters = renderingParametersService.GetCurrentRenderingParameters<ILatest_News_Options>();
			DisplayTitle = parameters.Display_Title;
			if (DisplayTitle)
			{
				TitleText = textTranslator.Translate("Article.LatestFrom");
				Topics = parameters.Subjects.Select(s => s.Item_Name).ToArray();
			}
			int itemsToDisplay = parameters.Number_To_Display?.Value ?? 6;
			News = GetLatestNews(datasource._Id, parameters.Subjects.Select(s => s._Id), itemsToDisplay);
			SeeAllLink = parameters.Display_See_All_Link ? new Link
			{
				Text = textTranslator.Translate("Article.LatestFrom.SeeAllLink"),
				Url = SearchTaxonomyUtil.GetSearchUrl(parameters.Subjects.ToArray())
			} : null;
		}

		public IList<string> Topics { get; set; }
		public IEnumerable<IListableViewModel> News { get; set; }
		public string TitleText { get; set; }
		public bool DisplayTitle { get; set; }
		public Link SeeAllLink { get; set; }

		private IEnumerable<IListableViewModel> GetLatestNews(Guid datasourceId, IEnumerable<Guid> subjectIds,
			int itemsToDisplay)
		{
			var manuallyCuratedContent = ItemManuallyCuratedContent.Get(datasourceId);
			var filter = ArticleSearch.CreateFilter();

			filter.Page = 1;
			filter.PageSize = itemsToDisplay;
			filter.ExcludeManuallyCuratedItems.AddRange(manuallyCuratedContent);
			filter.TaxonomyIds.AddRange(subjectIds);

			var results = ArticleSearch.Search(filter);
			var articles =
				results.Articles.Where(a => a != null)
					.Select(a => ArticleListableFactory.Create(a).Alter(l => l.DisplayImage = false));

			return articles;
		}
	}
}