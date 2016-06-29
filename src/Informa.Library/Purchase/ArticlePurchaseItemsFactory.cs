using Informa.Library.Article.Search;
using Informa.Library.Site;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Purchase
{
	[AutowireService]
	public class ArticlePurchaseItemsFactory : IArticlePurchaseItemsFactory
	{
		protected readonly IArticleSearch ArticleSearch;
		protected readonly IIsUrlCurrentSite IsUrlCurrentSite;

		public ArticlePurchaseItemsFactory(
			IArticleSearch articleSearch,
			IIsUrlCurrentSite isUrlCurrentSite)
		{
			ArticleSearch = articleSearch;
			IsUrlCurrentSite = isUrlCurrentSite;
		}

		public IEnumerable<IArticlePurchaseItem> Create(IEnumerable<IArticlePurchase> articlePurchases)
		{
			if (articlePurchases == null || !articlePurchases.Any())
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

				var url = article._Url ?? string.Empty;
				var isExternalUrl = !IsUrlCurrentSite.Check(url);

				articlePurchaseItems.Add(new ArticlePurchaseItem
				{
					Expiration = articlePurchase.Expiration,
					Publication = articlePurchase.Publication,
					Title = article.Title,
					Url = url,
					IsExternalUrl = isExternalUrl
				});
			}

			return articlePurchaseItems;
		}
	}
}
