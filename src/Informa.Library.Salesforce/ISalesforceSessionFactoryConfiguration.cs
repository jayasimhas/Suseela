namespace Informa.Library.Salesforce
{
	public interface ISalesforceSessionFactoryConfiguration
	{
		string Username { get; }
		string Password { get; }
		string Token { get; }
		int Timeout { get; }
	}
}
