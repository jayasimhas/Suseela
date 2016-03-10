using System.Net;

namespace Informa.Library.Salesforce.Company
{
	public interface ISalesforceCompany
	{
		IPAddress LowerIpAddress { get; }
		IPAddress UpperIpAddress { get; }
	}
}
