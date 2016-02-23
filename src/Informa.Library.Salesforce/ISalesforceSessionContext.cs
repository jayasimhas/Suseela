namespace Informa.Library.Salesforce
{
	public interface ISalesforceSessionContext
	{
		ISalesforceSession Session { get; }
		void Refresh();
	}
}
