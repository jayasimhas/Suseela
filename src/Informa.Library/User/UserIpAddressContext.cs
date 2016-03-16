using System.Linq;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Net;
using System.Web;

namespace Informa.Library.User
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserIpAddressContext : IUserIpAddressContext
	{
		protected readonly IUserIpAddressSession Session;

		public UserIpAddressContext(
			IUserIpAddressSession session)
		{
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

		        string ip =
		            HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

		        if (string.IsNullOrEmpty(ip))
		            ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

		        if (string.IsNullOrEmpty(ip))
		            ip = HttpContext.Current.Request.UserHostAddress;

		        ip?.Split(',').Any(x => IPAddress.TryParse(x, out ipAddress));

                return Session.IpAddress = ipAddress;                                     
		    }                                                         
		}                   
	}
}
