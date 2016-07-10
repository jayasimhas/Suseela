using Informa.Library.SiteDebugging;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.SiteDebugging
{
	[AutowireService(LifetimeScope.Default)]
	public class ToolbarViewModel : IToolbarViewModel
	{
		public ToolbarViewModel(
			IUserIpAddressViewModel userIpAddressViewModel,
			IEntitlementsCheckEnabledViewModel entitlementsCheckEnabledViewModel,
			IUserEntitlementsViewModel userEntitlementsViewModel,
			IUserSubscriptionsViewModel userSubscriptionsViewModel,
			IUsernameViewModel usernameViewModel,
			IUserPageEntitlementViewModel userPageEntitlementViewModel)
		{
			UserIpAddressViewModel = userIpAddressViewModel;
			EntitlementsCheckEnabledViewModel = entitlementsCheckEnabledViewModel;
			UserEntitlementsViewModel = userEntitlementsViewModel;
			UserSubscriptionsViewModel = userSubscriptionsViewModel;
			UsernameViewModel = usernameViewModel;
			UserPageEntitlementViewModel = userPageEntitlementViewModel;
		}

		public IUserIpAddressViewModel UserIpAddressViewModel { get; set; }
		public IEntitlementsCheckEnabledViewModel EntitlementsCheckEnabledViewModel { get; set; }
		public IUserEntitlementsViewModel UserEntitlementsViewModel { get; set; }
		public IUserSubscriptionsViewModel UserSubscriptionsViewModel { get; set; }
		public IUsernameViewModel UsernameViewModel { get; set; }
		public IUserPageEntitlementViewModel UserPageEntitlementViewModel { get; set; }
        public string ToggleButtonText => "Debugging";
	}
}