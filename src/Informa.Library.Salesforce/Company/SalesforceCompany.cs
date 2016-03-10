using Informa.Library.Company;
using System.Net;

namespace Informa.Library.Salesforce.Company
{
	public class SalesforceCompany : ISalesforceCompany, ICompany
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public IPAddress LowerIpAddress { get; set; }
		public IPAddress UpperIpAddress { get; set; }
	}
}
