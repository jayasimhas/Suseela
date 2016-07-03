using System;
using Informa.Library.Article.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Fields;
using Informa.Library.ContentCuration;
using Informa.Library.Globalization;
using Informa.Library.Search.Utilities;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Autofac.Mvc.Services;

namespace Informa.Web.ViewModels
{
	public class LatestNewsViewModel : GlassViewModel<IGlassBase>
	{
		protected readonly IArticleSearch ArticleSearch;
		protected readonly IItemManuallyCuratedContent ItemManuallyCuratedContent;
		protected readonly IArticleListItemModelFactory ArticleListableFactory;
		protected readonly ITextTranslator TextTranslator;

		public LatestNewsViewModel(IGlassBase datasource,
			IRenderingContextService renderingParametersService,
			IArticleSearch articleSearch,
			IItemManuallyCuratedContent itemManuallyCuratedContent,
			IArticleListItemModelFactory articleListableFactory,
			ISiteRootContext rootContext,
			ITextTranslator textTranslator)
		{
			ArticleSearch = articleSearch;
			ItemManuallyCuratedContent = itemManuallyCuratedContent;
			ArticleListableFactory = articleListableFactory;
			TextTranslator = textTranslator;


			var parameters = renderingParametersService.GetCurrentRenderingParameters<ILatest_News_Options>();
			var subjects = parameters?.Subjects ?? Enumerable.Empty<ITaxonomy_Item>();
			DisplayTitle = parameters?.Display_Title ?? false;
			if (DisplayTitle)
			{
				Topics = subjects.Select(s => s.Item_Name).ToArray();
				TitleTextPrefix = textTranslator.Translate("Article.LatestFrom");
				TitleText = GetTitleText();
			}
			int itemsToDisplay = parameters?.Number_To_Display?.Value ?? 6;

			var publicationNames = parameters != null && parameters.Publications.Any()
				? parameters.Publications.Select(p => p.Publication_Name)
				: new[] { rootContext.Item.Publication_Name };
			News = GetLatestNews(datasource._Id, subjects.Select(s => s._Id), publicationNames, itemsToDisplay);
			SeeAllLink = parameters != null && parameters.Show_See_All ? new Link
			{
				Text = textTranslator.Translate("Article.LatestFrom.SeeAllLink"),
				Url = SearchTaxonomyUtil.GetSearchUrl(subjects.ToArray())
			} : null;
		}

		public IList<string> Topics { get; set; }
		public IEnumerable<IListableViewModel> News { get; set; }
		public string TitleTextPrefix { get; set; }
		public string TitleText { get; set; }
		public bool DisplayTitle { get; set; }
		public Link SeeAllLink { get; set; }

		private string GetTitleText()
		{
			var take = Topics.Count - 1;
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Join(", ", Topics.Take(take > 0 ? take : 1)));
			if (take > 0)
				sb.AppendFormat(" &amp; {0}", Topics.Last());

			return sb.ToString();
		}
		private IEnumerable<IListableViewModel> GetLatestNews(Guid datasourceId, IEnumerable<Guid> subjectIds, IEnumerable<string> publicationNames,
			int itemsToDisplay)
		{
			var manuallyCuratedContent = ItemManuallyCuratedContent.Get(datasourceId);
			var filter = ArticleSearch.CreateFilter();

			filter.Page = 1;
			filter.PageSize = itemsToDisplay;
			filter.ExcludeManuallyCuratedItems.AddRange(manuallyCuratedContent);
			filter.TaxonomyIds.AddRange(subjectIds);
			filter.PublicationNames.AddRange(publicationNames);

			var results = ArticleSearch.Search(filter);
			var articles =
				results.Articles.Where(a => a != null)
					.Select(a => ArticleListableFactory.Create(a).Alter(l => l.DisplayImage = false));

			return articles;
		}
	}
}