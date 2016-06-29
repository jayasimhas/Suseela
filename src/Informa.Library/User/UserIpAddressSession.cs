using Jabberwocky.Glass.Autofac.Attributes;
using System.Net;

namespace Informa.Library.User
{
	[AutowireService(LifetimeScope.PerScope)]
	public class UserIpAddressSession : IUserIpAddressSession
	{
		private const string UserIpAddressSessionKey = "UserIpAddress";

		protected readonly IUserSession UserSession;

		public UserIpAddressSession(
			IUserSession userSession)
		{
			UserSession = userSession;
		}

		public IPAddress IpAddress
		{
			get
			{
				return UserSession.Get<IPAddress>(UserIpAddressSessionKey).Value;
			}
			set
			{
				UserSession.Set(UserIpAddressSessionKey, value);
			}
		}
	}
}
