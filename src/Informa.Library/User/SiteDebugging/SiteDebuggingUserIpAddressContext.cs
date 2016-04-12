using Informa.Library.Session;
using Informa.Library.SiteDebugging;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Net;

namespace Informa.Library.User.SiteDebugging
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SiteDebuggingUserIpAddressContext : ISiteDebuggingUserIpAddressContext
	{
		private const string DebuggerKey = "UserIpAddress";

		protected readonly ISpecificSessionStores SessionStores;
		protected readonly ISiteDebugger SiteDebugger;
		protected readonly ISetUserIpAddressContext UserIpAddressContext;

		public SiteDebuggingUserIpAddressContext(
			ISpecificSessionStores sessionStores,
			ISiteDebugger siteDebugger,
			ISetUserIpAddressContext userIpAddressContext)
		{
			SessionStores = sessionStores;
			SiteDebugger = siteDebugger;
			UserIpAddressContext = userIpAddressContext;
		}

		public bool IsDebugging => SiteDebugger.IsDebugging(DebuggerKey);

		public void StopDebugging()
		{
			SessionStores.Clear();
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
