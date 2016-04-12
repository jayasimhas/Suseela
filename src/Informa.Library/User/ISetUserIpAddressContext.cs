using System.Net;

namespace Informa.Library.User
{
	public interface ISetUserIpAddressContext
	{
		IPAddress IpAddress { get; set; }
	}
}
