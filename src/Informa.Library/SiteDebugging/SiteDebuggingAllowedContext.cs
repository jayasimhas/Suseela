using Informa.Library.Net;
using System.Linq;

namespace Informa.Library.SiteDebugging
{
	public class SiteDebuggingAllowedContext : ISiteDebuggingAllowedContext
	{
		protected readonly IIpAddressContext IpAddressContext;
		protected readonly ISiteDebuggingAllowedConfiguration Configuration;

		public SiteDebuggingAllowedContext(
			IIpAddressContext ipAddressContext,
			ISiteDebuggingAllowedConfiguration configuration)
		{
			IpAddressContext = ipAddressContext;
			Configuration = configuration;
		}

		public bool IsAllowed => IsAllowedByIpAddress || IsAllowedByEmailAddress;

		public bool IsAllowedByIpAddress => 
			IpAddressContext.IpAddress != null &&
			Configuration.IpAddresses.Any(ia => ia.Equals(IpAddressContext.IpAddress));

		public bool IsAllowedByEmailAddress => false;
	}
}
