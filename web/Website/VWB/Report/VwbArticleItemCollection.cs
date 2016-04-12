using System.Collections.Generic;

namespace Elsevier.Web.VWB.Report
{
	public class VwbArticleItemCollection : IVwbArticleItemCollection
	{
		public List<ArticleItemWrapper> ArticleItemProxies;

		public VwbArticleItemCollection(List<ArticleItemWrapper> articleItemProxies)
		{
			ArticleItemProxies = articleItemProxies;
		}

		public List<ArticleItemWrapper> GetCollectionOfArticles()
		{
			return ArticleItemProxies;
		}
	}
}