namespace Informa.Library.Salesforce
{
	public interface ISalesforceSessionContext
	{
		ISalesforceSession Session { get; }
		ISalesforceSession Refresh();
	}
}
