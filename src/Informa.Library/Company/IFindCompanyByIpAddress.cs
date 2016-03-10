using System.Net;

namespace Informa.Library.Company
{
	public interface IFindCompanyByIpAddress
	{
		ICompany Find(IPAddress ipAddress);
	}
}
