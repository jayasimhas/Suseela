using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Purchase
{
	[AutowireService]
	public class ArticlePurchaseItemsFactory : IArticlePurchaseItemsFactory
	{
		protected readonly ISitecoreContext SitecoreContext;
		protected readonly IArticleSearch ArticleSearch;

		public ArticlePurchaseItemsFactory(
			ISitecoreContext sitecoreContext,
			IArticleSearch articleSearch)
		{
			SitecoreContext = sitecoreContext;
			ArticleSearch = articleSearch;
		}

		public IEnumerable<IArticlePurchaseItem> Create(IEnumerable<IArticlePurchase> articlePurchases)
		{
			if (!articlePurchases.Any())
			{
				return Enumerable.Empty<IArticlePurchaseItem>();
			}

			var filter = ArticleSearch.CreateFilter();

			filter.ArticleNumbers = articlePurchases.Select(ap => ap.DocumentId).ToList();

			var results = ArticleSearch.Search(filter);
			var articlePurchaseItems = new List<IArticlePurchaseItem>();

			foreach (var article in results.Articles)
			{
				var articlePurchase = articlePurchases.FirstOrDefault(ap => string.Equals(ap.DocumentId, article.Article_Number));

				if (articlePurchase == null)
				{
					continue;
				}

				articlePurchaseItems.Add(new ArticlePurchaseItem
				{
					Expiration = articlePurchase.Expiration,
					Publication = articlePurchase.Publication,
					Title = article.Title,
					Url = article._Url
				});
			}

			return articlePurchaseItems;
		}
	}
}
