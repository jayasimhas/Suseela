using System.Net;

namespace Informa.Library.Net
{
	public interface IIpAddressFactory
	{
		IPAddress Create(string rawIpAddress);
	}
}