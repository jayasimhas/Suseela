using System.Linq;
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
		        IPAddress ipAddress = null;

		        string ip =
		            HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

		        if (string.IsNullOrEmpty(ip))
		            ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

		        if (string.IsNullOrEmpty(ip))
		            ip = HttpContext.Current.Request.UserHostAddress;

		        ip?.Split(',').Any(x => IPAddress.TryParse(x, out ipAddress));

                return ipAddress;                                     
		    }                                                         
		}                   
	}
}
