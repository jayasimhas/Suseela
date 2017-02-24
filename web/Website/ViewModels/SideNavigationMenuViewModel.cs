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
        protected readonly ISiteRootContext SiteRootContext;

        public SideNavigationMenuViewModel(
            ISiteRootContext siteRootContext,
            ISiteMainNavigationContext siteMainNavigationContext,
			ITextTranslator textTranslator)
		{
            SiteRootContext = siteRootContext;
            SiteMainNavigationContext = siteMainNavigationContext;
			TextTranslator = textTranslator;
		}

		public IEnumerable<INavigation> Navigation => SiteMainNavigationContext.Navigation;

		public string MenuText => TextTranslator.Translate("MainNavigation.Menu");

		public string MenuButtonText => TextTranslator.Translate("MainNavigation.ToggleMenu");

        public bool MenuOpenFirstTime => SiteRootContext.Item.Is_Open_First_Time;

    }
}