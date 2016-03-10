using System.Net;

namespace Informa.Library.Net
{
	public interface IIpAddressRangeCheck
	{
		bool IsInRange(IPAddress ipAddress, IPAddress lowerAddress, IPAddress upperAddress);
	}
}
