using Informa.Library.Presentation;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ArticleTagsViewModel : IArticleTagsViewModel
	{
		public IRenderingItemContext ArticleRenderingContext;

		public ArticleTagsViewModel(
			IRenderingItemContext articleRenderingContext)
		{
			ArticleRenderingContext = articleRenderingContext;
		}

		public IEnumerable<ILinkable> Tags
			=> ArticleRenderingContext.Get<IArticle>().Taxonomies.Take(3).Select(x => new LinkableModel { LinkableText = x.Item_Name, LinkableUrl = "/search?tag=" + x._Id });
	}
}