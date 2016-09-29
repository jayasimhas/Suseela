using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels.Articles
{
	public class ArticlePrologueViewModel : GlassViewModel<IGlassBase>
	{
		public ArticlePrologueViewModel(
			IArticleProloguePrintViewModel articleProloguePrintViewModel,
			IArticlePrologueEmailViewModel articlePrologueEmailViewModel,
			IArticlePrologueBookmarkViewModel articlePrologueBookmarkViewModel,
			IArticlePrologueShareViewModel articlePrologueShareViewModel,
			IArticleTagsViewModel articleTagsViewModel)
		{
			PrintViewModel = articleProloguePrintViewModel;
			EmailViewModel = articlePrologueEmailViewModel;
			BookmarkViewModel = articlePrologueBookmarkViewModel;
			ShareViewModel = articlePrologueShareViewModel;
			ArticleTagsViewModel = articleTagsViewModel;
		}

		public IArticleProloguePrintViewModel PrintViewModel;

		public IArticlePrologueEmailViewModel EmailViewModel;

		public IArticlePrologueBookmarkViewModel BookmarkViewModel;

		public IArticlePrologueShareViewModel ShareViewModel;

		public IArticleTagsViewModel ArticleTagsViewModel;
	}
}