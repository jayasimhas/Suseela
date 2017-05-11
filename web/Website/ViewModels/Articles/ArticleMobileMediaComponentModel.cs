using System.Collections.Generic;
using System.Linq;
using Informa.Library.Article.Search;
using Informa.Library.Services.Global;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Sitecore.Web;
using Informa.Library.Globalization;
using Jabberwocky.Core.Caching;

namespace Informa.Web.ViewModels.Articles
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
		protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ITextTranslator TextTranslator;
        protected readonly ICacheProvider CacheProvider;
        public RelatedArticlesModel(IArticle model,
              ICacheProvider cacheProvider,
                        IArticleListItemModelFactory articleListableFactory,
						IArticleSearch searcher,
						IGlobalSitecoreService globalService,
                        ITextTranslator textTranslator)
		{
            CacheProvider = cacheProvider;
            ArticleListableFactory = articleListableFactory;
			Searcher = searcher;
            GlobalService = globalService;
			RelatedArticles = GetRelatedArticles(model);
            TextTranslator = textTranslator;
		}
        private string CreateCacheKey(string suffix)
        {
            return $"{nameof(RelatedArticlesModel)}-{suffix}";
        }
        public string RelatedArticleComponentTitle => TextTranslator.Translate("Article.RelatedContentTitle");
        private IEnumerable<IListable> GetRelatedArticles(IArticle article)
		{
            string cacheKey = CreateCacheKey($"RelatedArticles-{article._Id}");
            return CacheProvider.GetFromCache(cacheKey, () => BuildRelatedArticle(article));
        }

        private IEnumerable<IListable>  BuildRelatedArticle(IArticle article)
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
            return relatedArticles.Where(r => r != null).Select(x => ArticleListableFactory.Create(GlobalService.GetItem<IArticle>(x._Id))).Cast<IListable>().OrderByDescending(x => x.ListableDate);
        }

		public IEnumerable<IListable> RelatedArticles { get; set; }
	}
}