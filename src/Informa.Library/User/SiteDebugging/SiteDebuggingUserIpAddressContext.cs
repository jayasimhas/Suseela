using Informa.Library.SiteDebugging;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Net;

namespace Informa.Library.User.SiteDebugging
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SiteDebuggingUserIpAddressContext : ISiteDebuggingUserIpAddressContext
	{
		private const string DebuggerKey = "UserIpAddress";

		protected readonly ISiteDebugger SiteDebugger;
		protected readonly ISetUserIpAddressContext UserIpAddressContext;

		public SiteDebuggingUserIpAddressContext(
			ISiteDebugger siteDebugger,
			ISetUserIpAddressContext userIpAddressContext)
		{
			SiteDebugger = siteDebugger;
			UserIpAddressContext = userIpAddressContext;
		}

		public bool IsDebugging => SiteDebugger.IsDebugging(DebuggerKey);

		public void StopDebugging()
		{
			UserIpAddressContext.IpAddress = null;
			SiteDebugger.StopDebugging(DebuggerKey);
		}

		public void StartDebugging(IPAddress ipAddress)
		{
			UserIpAddressContext.IpAddress = ipAddress;
			SiteDebugger.StartDebugging(DebuggerKey);
		}
	}
}
