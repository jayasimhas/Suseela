using System.Net;

namespace Informa.Library.User
{
	public interface IUserIpAddressContext
	{
		IPAddress IpAddress { get; }
	}
}
