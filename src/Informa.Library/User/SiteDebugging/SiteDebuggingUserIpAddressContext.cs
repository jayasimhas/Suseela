using Informa.Library.SiteDebugging;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Net;

namespace Informa.Library.User.SiteDebugging
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SiteDebuggingUserIpAddressContext : ISiteDebuggingUserIpAddressContext
	{
		private const string IsDebuggingSessionKey = "UserIpAddress.IsDebugging";

		protected readonly ISiteDebuggingSession DebugSession;
		protected readonly ISetUserIpAddressContext UserIpAddressContext;

		public SiteDebuggingUserIpAddressContext(
			ISiteDebuggingSession debugSession,
			ISetUserIpAddressContext userIpAddressContext)
		{
			DebugSession = debugSession;
			UserIpAddressContext = userIpAddressContext;
		}

		public bool IsDebugging => DebugSession.Get<bool>(IsDebuggingSessionKey);

		public void StopDebugging()
		{
			UserIpAddressContext.IpAddress = null;
			DebugSession.Set(IsDebuggingSessionKey, false);
		}

		public void StartDebugging(IPAddress ipAddress)
		{
			UserIpAddressContext.IpAddress = ipAddress;
			DebugSession.Set(IsDebuggingSessionKey, true);
		}
	}
}
