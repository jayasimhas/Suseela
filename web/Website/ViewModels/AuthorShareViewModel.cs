using Informa.Web.ViewModels.Articles;
using Informa.Web.ViewModels.Authors;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
	public class AuthorShareViewModel : GlassViewModel<IGlassBase>
	{
		public AuthorShareViewModel(
			IAuthorPrologueEmailViewModel authorPrologueEmailViewModel,
            IAuthorShareViewModel authorShareViewModel
            )
		{
			EmailViewModel = authorPrologueEmailViewModel;
			ShareViewModel = authorShareViewModel;
		}

		public IAuthorPrologueEmailViewModel EmailViewModel;

		public IAuthorShareViewModel ShareViewModel;
	}
}