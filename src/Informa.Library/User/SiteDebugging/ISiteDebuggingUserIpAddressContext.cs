using System.Net;

namespace Informa.Library.User.SiteDebugging
{
	public interface ISiteDebuggingUserIpAddressContext
	{
		void StartDebugging(IPAddress ipAddress);
		void StopDebugging();
		bool IsDebugging { get; }
	}
}
