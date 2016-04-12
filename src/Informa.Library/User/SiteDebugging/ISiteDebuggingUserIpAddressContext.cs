using Informa.Library.SiteDebugging;
using System.Net;

namespace Informa.Library.User.SiteDebugging
{
	public interface ISiteDebuggingUserIpAddressContext : ISiteDebugging
	{
		void StartDebugging(IPAddress ipAddress);
		void StopDebugging();
	}
}
