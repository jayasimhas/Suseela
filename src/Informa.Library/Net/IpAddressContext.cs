using Jabberwocky.Glass.Autofac.Attributes;
using System.Net;
using System.Web;

namespace Informa.Library.Net
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class IpAddressContext : IIpAddressContext
	{
		protected readonly IIpAddressFactory IpAddressFactory;

		public IpAddressContext(
			IIpAddressFactory ipAddressFactory)
		{
			IpAddressFactory = ipAddressFactory;
		}

		public IPAddress IpAddress
		{
			get
			{
				string rawIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

				if (string.IsNullOrEmpty(rawIpAddress))
				{
					rawIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
				}

				if (string.IsNullOrEmpty(rawIpAddress))
				{
					rawIpAddress = HttpContext.Current.Request.UserHostAddress;
				}

				return IpAddressFactory.Create(rawIpAddress);
			}
		}
	}
}
