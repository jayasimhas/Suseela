using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce
{
	public class SalesforceService : EBI_WebServicesService, ISalesforceService
	{
		public SalesforceService(
			ISalesforceServiceConfiguration configuration)
		{
			Url = configuration.Url;
		}
	}
}
