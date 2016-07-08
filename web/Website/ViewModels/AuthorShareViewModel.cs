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
			IArticlePrologueEmailViewModel articlePrologueEmailViewModel,
			IArticlePrologueShareViewModel articlePrologueShareViewModel
			)
		{
			EmailViewModel = articlePrologueEmailViewModel;
			ShareViewModel = articlePrologueShareViewModel;
		}

		public IArticlePrologueEmailViewModel EmailViewModel;

		public IArticlePrologueShareViewModel ShareViewModel;
	}
}