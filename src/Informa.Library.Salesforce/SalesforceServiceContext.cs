using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce
{
	public class SalesforceServiceContext : SalesforceService, ISalesforceServiceContext
	{
		protected readonly ISalesforceSessionContext SessionContext;

		public SalesforceServiceContext(
			ISalesforceSessionContext sessionContext)
		{
			SessionContext = sessionContext;

			RefreshSession();
		}

		public void RefreshSession()
		{
			if (SessionHeaderValue == null)
			{
				SessionHeaderValue = new SessionHeader();
			}

			SessionHeaderValue.sessionId = SessionContext.Session.Id;
		}
	}
}
