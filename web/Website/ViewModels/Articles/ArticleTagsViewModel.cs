using System.Collections.Generic;
using System.Linq;
using Informa.Library.Presentation;
using Informa.Library.Search.Utilities;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.Articles
{
	[AutowireService(LifetimeScope.PerScope)]
	public class ArticleTagsViewModel : IArticleTagsViewModel
	{
		public IRenderingItemContext ArticleRenderingContext;

		public ArticleTagsViewModel(
			IRenderingItemContext articleRenderingContext)
		{
			ArticleRenderingContext = articleRenderingContext;

			Tags = ArticleRenderingContext.Get<I___BaseTaxonomy>().Taxonomies.Take(3).Select(x => new LinkableModel
			{
				LinkableText = x.Item_Name,
				LinkableUrl = SearchTaxonomyUtil.GetSearchUrl(x)
			});
		}

		public IEnumerable<ILinkable> Tags { get; set; }
	}
}