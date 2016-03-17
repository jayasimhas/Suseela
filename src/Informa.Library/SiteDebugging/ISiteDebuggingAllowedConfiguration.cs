using System.Collections.Generic;
using System.Net;

namespace Informa.Library.SiteDebugging
{
	public interface ISiteDebuggingAllowedConfiguration
	{
		IEnumerable<string> EmailAddresses { get; }
		IEnumerable<IPAddress> IpAddresses { get; }
	}
}