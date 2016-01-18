using System.Collections.Generic;
using Informa.Library.Globalization;
using Informa.Library.Navigation;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SideNavigationMenuViewModel : ISideNavigationMenuViewModel
	{
		protected readonly ISiteMainNavigationContext SiteMainNavigationContext;
		protected readonly ITextTranslator TextTranslator;

		public SideNavigationMenuViewModel(
			ISiteMainNavigationContext siteMainNavigationContext,
			ITextTranslator textTranslator)
		{
			SiteMainNavigationContext = siteMainNavigationContext;
			TextTranslator = textTranslator;
		}

		public IEnumerable<INavigation> Navigation => SiteMainNavigationContext.Navigation;

		public string MenuText => TextTranslator.Translate("Menu");

		public string MenuButtonText => TextTranslator.Translate("ToggleMenu");
	}
}