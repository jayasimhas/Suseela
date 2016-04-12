namespace Informa.Library.Salesforce
{
	public interface ISalesforceSessionFactoryConfiguration
	{
		string Url { get; }
		string Username { get; }
		string Password { get; }
		string Token { get; }
		int Timeout { get; }
	}
}
