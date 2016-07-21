using Informa.Web.ViewModels.Articles;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
	public class AuthorShareViewModel : GlassViewModel<IGlassBase>
	{
		public AuthorShareViewModel(
			IAuthorPrologueEmailViewModel authorPrologueEmailViewModel,
			IArticlePrologueShareViewModel articlePrologueShareViewModel
			)
		{
			EmailViewModel = authorPrologueEmailViewModel;
			ShareViewModel = articlePrologueShareViewModel;
		}

		public IAuthorPrologueEmailViewModel EmailViewModel;

		public IArticlePrologueShareViewModel ShareViewModel;
	}
}