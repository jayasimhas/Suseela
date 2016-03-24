﻿using Informa.Library.SiteDebugging;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.SiteDebugging
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ToolbarViewModel : IToolbarViewModel
	{
		protected readonly ISiteDebuggingAllowedContext SiteDebuggingAllowedContext;

		public ToolbarViewModel(
			ISiteDebuggingAllowedContext siteDebuggingAllowedContext,
			IUserIpAddressViewModel userIpAddressViewModel,
			IEntitlementsCheckEnabledViewModel entitlementsCheckEnabledViewModel,
			IUserEntitlementsViewModel userEntitlementsViewModel,
			IUserSubscriptionsViewModel userSubscriptionsViewModel)
		{
			SiteDebuggingAllowedContext = siteDebuggingAllowedContext;
			UserIpAddressViewModel = userIpAddressViewModel;
			EntitlementsCheckEnabledViewModel = entitlementsCheckEnabledViewModel;
			UserEntitlementsViewModel = userEntitlementsViewModel;
			UserSubscriptionsViewModel = userSubscriptionsViewModel;
		}

		public IUserIpAddressViewModel UserIpAddressViewModel { get; set; }
		public IEntitlementsCheckEnabledViewModel EntitlementsCheckEnabledViewModel { get; set; }
		public IUserEntitlementsViewModel UserEntitlementsViewModel { get; set; }
		public IUserSubscriptionsViewModel UserSubscriptionsViewModel { get; set; }

		public bool Enabled => SiteDebuggingAllowedContext.IsAllowed;
		public string ToggleButtonText => "Debugging";
	}
}