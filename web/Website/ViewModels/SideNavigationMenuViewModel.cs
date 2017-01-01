using System.Collections.Generic;
using Informa.Library.Globalization;
using Informa.Library.Navigation;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
	public class SideNavigationMenuViewModel : GlassViewModel<I___BasePage>
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

		public string MenuText => TextTranslator.Translate("MainNavigation.Menu");

		public string MenuButtonText => TextTranslator.Translate("MainNavigation.ToggleMenu");

        public string CurrentItemId => GlassModel?._Id.ToString();
	}
}