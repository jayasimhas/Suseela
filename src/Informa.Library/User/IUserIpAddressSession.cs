using System.Net;

namespace Informa.Library.User
{
	public interface IUserIpAddressSession
	{
		IPAddress IpAddress { get; set; }
	}
}