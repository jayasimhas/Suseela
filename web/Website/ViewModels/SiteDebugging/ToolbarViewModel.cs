using Informa.Library.SiteDebugging;
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
			IEntitlementsCheckEnabledViewModel entitlementsCheckEnabledViewModel)
		{
			SiteDebuggingAllowedContext = siteDebuggingAllowedContext;
			UserIpAddressViewModel = userIpAddressViewModel;
			EntitlementsCheckEnabledViewModel = entitlementsCheckEnabledViewModel;
		}

		public IUserIpAddressViewModel UserIpAddressViewModel { get; set; }
		public IEntitlementsCheckEnabledViewModel EntitlementsCheckEnabledViewModel { get; set; }

		public bool Enabled => SiteDebuggingAllowedContext.IsAllowed;
		public string ToggleButtonText => "Debugging";
	}
}