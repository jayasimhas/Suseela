using Jabberwocky.Glass.Autofac.Attributes;
using System.Net;
using Informa.Library.Net;

namespace Informa.Library.User
{
	[AutowireService(LifetimeScope.PerScope)]
	public class UserIpAddressContext : IUserIpAddressContext, ISetUserIpAddressContext
	{
		protected readonly IIpAddressContext IpAddressContext;
		protected readonly IUserIpAddressSession Session;

		public UserIpAddressContext(
			IIpAddressContext ipAddressContext,
			IUserIpAddressSession session)
		{
			IpAddressContext = ipAddressContext;
			Session = session;
		}

		public IPAddress IpAddress
		{
		    get
		    {
		        var ipAddress = Session.IpAddress;

				if (ipAddress != null)
				{
					return ipAddress;
				}

                return Session.IpAddress = IpAddressContext.IpAddress;                                     
		    }
			set
			{
				Session.IpAddress = value;
			}
		}
	}
}
