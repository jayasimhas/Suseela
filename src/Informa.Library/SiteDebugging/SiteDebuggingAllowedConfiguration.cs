using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace Informa.Library.SiteDebugging
{
	public class SiteDebuggingAllowedConfiguration : ISiteDebuggingAllowedConfiguration
	{
		public IEnumerable<IPAddress> IpAddresses => Enumerable.Empty<IPAddress>(); // TODO: Hook up with Sitecore field
		public IEnumerable<string> EmailAddresses => Enumerable.Empty<string>(); // TODO: Hook up with Sitecore field
	}
}
