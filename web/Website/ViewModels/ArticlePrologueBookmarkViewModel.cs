using Informa.Library.Presentation;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ArticlePrologueBookmarkViewModel : IArticlePrologueBookmarkViewModel
	{
		public IRenderingItemContext ArticleRenderingContext;

		public ArticlePrologueBookmarkViewModel(
			IRenderingItemContext articleRenderingContext)
		{
			ArticleRenderingContext = articleRenderingContext;
		}

		public bool Bookmarked => false;
	}
}