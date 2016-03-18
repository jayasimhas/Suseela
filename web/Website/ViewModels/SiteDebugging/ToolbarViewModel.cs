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
			IUserIpAddressViewModel userIpAddressViewModel)
		{
			SiteDebuggingAllowedContext = siteDebuggingAllowedContext;
			UserIpAddressViewModel = userIpAddressViewModel;
		}

		public IUserIpAddressViewModel UserIpAddressViewModel { get; set; }

		public bool Enabled => SiteDebuggingAllowedContext.IsAllowed;
		public string ToggleButtonText => "Debugging";
	}
}