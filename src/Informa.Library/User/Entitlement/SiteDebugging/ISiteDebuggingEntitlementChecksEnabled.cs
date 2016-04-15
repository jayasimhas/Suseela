using Informa.Library.SiteDebugging;

namespace Informa.Library.User.Entitlement.SiteDebugging
{
	public interface ISiteDebuggingEntitlementChecksEnabled : ISiteDebugging
	{
		void StartDebugging();
		void StopDebugging();
	}
}