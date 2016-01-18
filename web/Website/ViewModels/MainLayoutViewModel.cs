﻿using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
	public class MainLayoutViewModel : GlassViewModel<IGlassBase>
	{
		protected readonly ISiteRootContext SiteRootContext;

		public MainLayoutViewModel(
			ISiteRootContext siteRootContext,
			IMaintenanceViewModel maintenanceViewModel,
			ISideNavigationMenuViewModel sideNavigationMenuViewModel,
			IFooterViewModel footerViewModel)
		{
			SiteRootContext = siteRootContext;
			MaintenanceMessage = maintenanceViewModel;
			SideNavigationMenu = sideNavigationMenuViewModel;
			Footer = footerViewModel;
		}

		public IMaintenanceViewModel MaintenanceMessage;

		public ISideNavigationMenuViewModel SideNavigationMenu;

		public IFooterViewModel Footer;

		public string Title
		{
			get
			{
				var pageTitle = string.Empty;

				if (GlassModel is I___BasePage)
				{
					var page = (I___BasePage)GlassModel;

					if (!string.IsNullOrEmpty(page.Meta_Title_Override))
					{
						return page.Meta_Title_Override.StripHtml();
					}

					pageTitle = page.Title.StripHtml();
				}
				
				if (string.IsNullOrWhiteSpace(pageTitle))
				{
					pageTitle = GlassModel._Name;
				}

				var publicationName = SiteRootContext.Item == null ? string.Empty : string.Format(" :: {0}", SiteRootContext.Item.Publication_Name.StripHtml());

				return string.Concat(pageTitle, publicationName);
			}
		}
	}
}