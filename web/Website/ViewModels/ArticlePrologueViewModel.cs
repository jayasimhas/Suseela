using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
	public class ArticlePrologueViewModel : GlassViewModel<IGlassBase>
	{
		public ArticlePrologueViewModel(
			IArticlePrologueBookmarkViewModel articlePrologueBookmarkViewModel,
			IArticleTagsViewModel articleTagsViewModel)
		{
			ArticlePrologueBookmark = articlePrologueBookmarkViewModel;
			ArticleTags = articleTagsViewModel;
		}

		public IArticlePrologueBookmarkViewModel ArticlePrologueBookmark;

		public IArticleTagsViewModel ArticleTags;
	}
}