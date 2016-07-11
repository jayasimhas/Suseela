using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
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