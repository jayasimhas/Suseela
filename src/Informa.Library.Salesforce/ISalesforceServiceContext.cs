namespace Informa.Library.Salesforce
{
	public interface ISalesforceServiceContext : ISalesforceService
	{
		void RefreshSession();
	}
}
