﻿using Glass.Mapper.Sc.Fields;
using Informa.Library.Authentication;
using Informa.Library.Globalization;
using Informa.Library.Navigation;
using Informa.Library.Site;
using Informa.Library.Subscription;
using Informa.Web.Models;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Web.ViewModels
{
	public class MainLayoutViewModel : GlassViewModel<IGlassBase>
	{
		protected readonly ISiteMainNavigationContext SiteMainNavigationContext;
		protected readonly IPageLinksFactory PageLinksFactory;
		protected readonly IUserAuthenticationContext UserAuthenticationContext;
		protected readonly IUserSubscriptionContext UserSubscriptionContext;
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly ISiteMaintenanceContext SiteMaintenanceContext;
		protected readonly ITextTranslator TextTranslator;

		public MainLayoutViewModel(
			ISiteMainNavigationContext siteMainNavigationContext,
			IPageLinksFactory pageLinksFactory,
			IUserAuthenticationContext userAuthenticationContext,
			IUserSubscriptionContext userSubscriptionContext,
			ISiteRootContext siteRootContext,
			ISiteMaintenanceContext siteMaintenanceContext,
			ITextTranslator textTranslator)
		{
			SiteMainNavigationContext = siteMainNavigationContext;
			PageLinksFactory = pageLinksFactory;
			UserAuthenticationContext = userAuthenticationContext;
			UserSubscriptionContext = userSubscriptionContext;
			SiteRootContext = siteRootContext;
			SiteMaintenanceContext = siteMaintenanceContext;
			TextTranslator = textTranslator;
		}

		public IMaintenanceViewModel MaintenanceMessage
		{
			get
			{
				var siteMaintenanceInfo = SiteMaintenanceContext.Info;

				return new MaintenanceViewModel
				{
					DismissText = TextTranslator.Translate("MaintenanceDismiss"),
					DisplayFrom = siteMaintenanceInfo.From,
					DisplayTo = siteMaintenanceInfo.To,
					Message = siteMaintenanceInfo.Message
				};
			}
		}

		public string FooterLogoUrl => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Footer_Logo.Src;

		public string CopyrightText => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Copyright_Text;

		public Link SubscribeLink
		{
			get
			{
				if (SiteRootContext.Item == null)
				{
					return null;
				}

				if (!UserSubscriptionContext.IsSubscribed)
				{
					return SiteRootContext.Item.Subscribe_Link;
				}

				if (UserAuthenticationContext.IsAuthenticated)
				{
					return SiteRootContext.Item.Purchase_Link;
				}
				else
				{
					return SiteRootContext.Item.Register_Link;
				}
			}
		}

		public IEnumerable<IPageLink> LocalLinks => SiteRootContext.Item == null ? Enumerable.Empty<IPageLink>() : PageLinksFactory.Create(SiteRootContext.Item.Local_Footer_Links);

		public string FollowText => TextTranslator.Translate("FooterFollow");

		public Link LinkedInLink => SiteRootContext.Item == null ? null : SiteRootContext.Item.LinkedIn_Link;

		public Link FacebookLink => SiteRootContext.Item == null ? null : SiteRootContext.Item.Facebook_Link;

		public Link TwitterLink => SiteRootContext.Item == null ? null : SiteRootContext.Item.Twitter_Link;

		public string MenuOneHeader => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Menu_One_Header;

		public IEnumerable<IPageLink> MenuOneLinks => SiteRootContext.Item == null ? Enumerable.Empty<IPageLink>() : PageLinksFactory.Create(SiteRootContext.Item.Menu_One_Links);

		public string MenuTwoHeader => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Menu_Two_Header;

		public IEnumerable<IPageLink> MenuTwoLinks => SiteRootContext.Item == null ? Enumerable.Empty<IPageLink>() : PageLinksFactory.Create(SiteRootContext.Item.Menu_Two_Links);

		public IEnumerable<INavigation> SideNavigationMenu => SiteMainNavigationContext.Navigation;

		public string SideNavigationMenuText => TextTranslator.Translate("Menu");

		public string SideNavigationMenuButtonText => TextTranslator.Translate("ToggleMenu");
	}
}