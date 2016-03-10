using Jabberwocky.Glass.Autofac.Attributes;
using System.Net;

namespace Informa.Library.Net
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class IpAddressRangeCheck : IIpAddressRangeCheck
	{
		public bool IsInRange(IPAddress ipAddress, IPAddress lowerAddress, IPAddress upperAddress)
		{
			var addressFamily = lowerAddress.AddressFamily;
			var lowerAddressBytes = lowerAddress.GetAddressBytes();
			var upperAddressBytes = upperAddress.GetAddressBytes();

			if (ipAddress.AddressFamily != addressFamily)
			{
				return false;
			}

			byte[] addressBytes = ipAddress.GetAddressBytes();

			bool lowerBoundary = true, upperBoundary = true;

			for (int i = 0; i < lowerAddressBytes.Length && (lowerBoundary || upperBoundary); i++)
			{
				if ((lowerBoundary && addressBytes[i] < lowerAddressBytes[i]) ||
					(upperBoundary && addressBytes[i] > upperAddressBytes[i]))
				{
					return false;
				}

				lowerBoundary &= (addressBytes[i] == lowerAddressBytes[i]);
				upperBoundary &= (addressBytes[i] == upperAddressBytes[i]);
			}

			return true;

		}
	}
}
