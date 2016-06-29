using Informa.Library.SiteDebugging;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement.SiteDebugging
{
	[AutowireService]
	public class SiteDebuggingEntitlementChecksEnabled : ISiteDebuggingEntitlementChecksEnabled
	{
		private const string DebuggerKey = "EntitlementChecksEnabled.IsDebugging";

		protected readonly ISiteDebugger SiteDebugger;
		protected readonly IEntitlementChecksEnabled EntitlementChecksEnabled;

		public SiteDebuggingEntitlementChecksEnabled(
			ISiteDebugger siteDebugger,
			IEntitlementChecksEnabled entitlementChecksEnabled)
		{
			SiteDebugger = siteDebugger;
			EntitlementChecksEnabled = entitlementChecksEnabled;
		}

		public bool IsDebugging => SiteDebugger.IsDebugging(DebuggerKey);

		public void StopDebugging()
		{
			EntitlementChecksEnabled.Enabled = true;
			SiteDebugger.StopDebugging(DebuggerKey);
		}

		public void StartDebugging()
		{
			EntitlementChecksEnabled.Enabled = false;
			SiteDebugger.StartDebugging(DebuggerKey);
		}
	}
}
