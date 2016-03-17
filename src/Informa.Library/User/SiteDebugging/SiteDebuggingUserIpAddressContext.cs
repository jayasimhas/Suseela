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

		public bool IsDebugging
		{
			get
			{
				var isDebugging = DebugSession.Get<bool?>(IsDebuggingSessionKey);

				if (!isDebugging.HasValue)
				{
					return false;
				}

				return isDebugging.Value;
			}
			set
			{
				bool? isDebugging = value;

				DebugSession.Set(IsDebuggingSessionKey, isDebugging);
			}
		}

		public void StopDebugging()
		{
			UserIpAddressContext.IpAddress = null;
			IsDebugging = false;
		}

		public void StartDebugging(IPAddress ipAddress)
		{
			UserIpAddressContext.IpAddress = ipAddress;
			IsDebugging = true;
		}
	}
}
