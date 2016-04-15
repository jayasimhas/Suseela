using Informa.Library.User.Entitlement.SiteDebugging;
using Informa.Library.Utilities.WebUtils;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Web;

namespace Informa.Web.ViewModels.SiteDebugging
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class EntitlementsCheckEnabledViewModel : IEntitlementsCheckEnabledViewModel
	{
		protected readonly IRefreshPage RefreshPage;
		protected readonly ISiteDebuggingEntitlementChecksEnabled DebugEntitlementChecksEnabled;

		public EntitlementsCheckEnabledViewModel(
			IRefreshPage refreshPage,
			ISiteDebuggingEntitlementChecksEnabled debugEntitlementChecksEnabled)
		{
			RefreshPage = refreshPage;
			DebugEntitlementChecksEnabled = debugEntitlementChecksEnabled;
		}

		public string LabelText => "Entitlement Checks";
		public string InputName => "debugEntitlementChecksEnabled";
		public string DisabledInputValue => "disabled";
		public string EnabledInputValue => "enabled";
		public string SubmitText => IsDebugging ? "Enable" : "Disable";
		public bool IsDebugging
		{
			get
			{
				var inputValue = HttpContext.Current.Request[InputName];

				if (string.IsNullOrWhiteSpace(inputValue))
				{
					return DebugEntitlementChecksEnabled.IsDebugging;
				}

				if (string.Equals(inputValue, EnabledInputValue))
				{
					DebugEntitlementChecksEnabled.StopDebugging();
					RefreshPage.Refresh();
				}
				else if (string.Equals(inputValue, DisabledInputValue))
				{
					DebugEntitlementChecksEnabled.StartDebugging();
					RefreshPage.Refresh();
				}

				return DebugEntitlementChecksEnabled.IsDebugging;
			}
		}
	}
}