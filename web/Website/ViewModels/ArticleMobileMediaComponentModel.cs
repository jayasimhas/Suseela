using System.Collections.Generic;
using System.Linq;
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

		public RelatedArticlesModel(IArticleListItemModelFactory articleListableFactory, IArticleSearch searcher)
		{
			ArticleListableFactory = articleListableFactory;
			Searcher = searcher;

			RelatedArticles = GetRelatedArticles();
		}

		private IEnumerable<IListable> GetRelatedArticles()
		{
			var relatedArticles = GlassModel.Related_Articles.Concat(GlassModel.Referenced_Articles).Take(10).ToList();

			if (relatedArticles.Count < 10)
			{
				var filter = Searcher.CreateFilter();
				filter.ReferencedArticle = GlassModel._Id;
				filter.PageSize = 10 - relatedArticles.Count;

				var results = Searcher.Search(filter);
				relatedArticles.AddRange(results.Articles);
			}
			return relatedArticles.Select(x => ArticleListableFactory.Create(x)).Cast<IListable>().OrderByDescending(x => x.ListableDate);
		} 

		public IEnumerable<IListable> RelatedArticles { get; set; }
	}
}