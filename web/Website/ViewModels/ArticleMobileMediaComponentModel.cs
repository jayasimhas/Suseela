using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Sitecore.Web;

namespace Informa.Web.ViewModels
{
	public class ArticleMobileMediaComponentModel : GlassViewModel<IArticle>
	{
		protected readonly IArticleComponentFactory ArticleComponentFactory;

		public ArticleMobileMediaComponentModel(IArticleComponentFactory articleComponentFactory)
		{
			ArticleComponentFactory = articleComponentFactory;
		}

		public string SelectedMobileMedia => Sitecore.Context.Device.QueryString == "mobilemedia=true" ? ArticleComponentFactory.Component(WebUtil.GetQueryString("selectedid"), GlassModel) : null;
	}

	public class RelatedArticlesModel : GlassViewModel<IArticle>
	{
		protected readonly IArticleSearch Searcher;
		protected readonly IArticleListItemModelFactory ArticleListableFactory;
		protected readonly ISitecoreService SitecoreService;

		public RelatedArticlesModel(IArticle model,
						IArticleListItemModelFactory articleListableFactory,
						IArticleSearch searcher,
						ISitecoreService sitecoreService)
		{
			ArticleListableFactory = articleListableFactory;
			Searcher = searcher;
			SitecoreService = sitecoreService;
			RelatedArticles = GetRelatedArticles(model);
		}

		private IEnumerable<IListable> GetRelatedArticles(IArticle article)
		{
			var relatedArticles = article.Related_Articles.Concat(article.Referenced_Articles).Take(10).ToList();

			if (relatedArticles.Count < 10)
			{
				var filter = Searcher.CreateFilter();
				filter.ReferencedArticle = article._Id;
				filter.PageSize = 10 - relatedArticles.Count;

				var results = Searcher.Search(filter);
				relatedArticles.AddRange(results.Articles);
			}
			return relatedArticles.Where(r => r != null).Select(x => ArticleListableFactory.Create(SitecoreService.GetItem<IArticle>(x._Id))).Cast<IListable>().OrderByDescending(x => x.ListableDate);
		}

		public IEnumerable<IListable> RelatedArticles { get; set; }
	}
}