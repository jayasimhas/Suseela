namespace Informa.Library.Salesforce.User
{
	public interface ISalesforceSetUserTemporaryPassword
	{
		bool Set(string username, string temporaryPassword);
	}
}
