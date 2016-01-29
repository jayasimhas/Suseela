using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Library.Presentation;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.ContentCuration;

namespace Informa.Web.ViewModels
{
	public class LatestNewsViewModel : GlassViewModel<IGlassBase>
	{
		protected readonly IRenderingParametersContext RenderingParametersContext;
		protected readonly ISitecoreContext SitecoreContext;
		protected readonly IArticleSearch ArticleSearch;
		protected readonly IItemManuallyCuratedContent ItemManuallyCuratedContent;
		protected readonly IArticleListItemModelFactory ArticleListableFactory;

		public LatestNewsViewModel(
			IRenderingParametersContext renderingParametersContext,
			ISitecoreContext sitecoreContext,
			IArticleSearch articleSearch,
			IItemManuallyCuratedContent itemManuallyCuratedContent,
			IArticleListItemModelFactory articleListableFactory)
		{
			RenderingParametersContext = renderingParametersContext;
			SitecoreContext = sitecoreContext;
			ArticleSearch = articleSearch;
			ItemManuallyCuratedContent = itemManuallyCuratedContent;
			ArticleListableFactory = articleListableFactory;
		}

		public IEnumerable<string> Topics => new List<string> { { TestData.TestData.GetRandomTopic().LinkableText }, { TestData.TestData.GetRandomTopic().LinkableText } };

		public IEnumerable<IListable> News
		{
			get
			{
				var manuallyCuratedContent = ItemManuallyCuratedContent.Get(GlassModel._Id);
				var filter = ArticleSearch.CreateFilter();

				filter.Page = 1;
				filter.PageSize = ArticlesToDisplay;
				filter.ExcludeManuallyCuratedItems.AddRange(manuallyCuratedContent);
				
				var results = ArticleSearch.Search(filter);

				return results.Articles.Select(a => ArticleListableFactory.Create(a));
			}
		}

		public int ArticlesToDisplay
		{
			get
			{
				var optionItem = SitecoreContext.GetItem<INumber_Option>(Parameters.Number_To_Display);

				return optionItem == null ? 6 : optionItem.Value;
			}
		}

		public bool DisplayTitle => Parameters.Display_Title;

		public ILatest_News_Options Parameters => RenderingParametersContext.GetParameters<ILatest_News_Options>();
	}
}