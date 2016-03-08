using System.Collections.Generic;

namespace Elsevier.Web.VWB.Report
{
	public interface IVwbArticleItemCollection
	{
		List<ArticleItemWrapper> GetCollectionOfArticles();
	}
}
