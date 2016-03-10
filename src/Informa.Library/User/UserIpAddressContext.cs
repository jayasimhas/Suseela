using Jabberwocky.Glass.Autofac.Attributes;
using System.Net;
using System.Web;

namespace Informa.Library.User
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserIpAddressContext : IUserIpAddressContext
	{
		public IPAddress IpAddress
		{
			get
			{
				IPAddress ipAddress;

				if (IPAddress.TryParse(HttpContext.Current.Request.UserHostAddress, out ipAddress))
				{
					return ipAddress;
				}

				return null;
			}
		}
	}
}
