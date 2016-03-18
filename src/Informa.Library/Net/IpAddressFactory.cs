using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;
using System.Net;

namespace Informa.Library.Net
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class IpAddressFactory : IIpAddressFactory
	{
		public IPAddress Create(string rawIpAddress)
		{
			IPAddress ipAddress = null;

			rawIpAddress?.Split(',').Any(x => IPAddress.TryParse(x, out ipAddress));

			return ipAddress;
		}
	}
}
