using Informa.Library.Salesforce.SalesforceAPI;

namespace Informa.Library.Salesforce
{
	public class SalesforceSessionFactory : ISalesforceSessionFactory
	{
		protected readonly ISalesforceSessionFactoryConfiguration Configuration;

		public SalesforceSessionFactory(
			ISalesforceSessionFactoryConfiguration configuration)
		{
			Configuration = configuration;
		}

		public ISalesforceSession Create()
		{
			var username = Configuration.Username;
			var password = string.Concat(Configuration.Password, Configuration.Token);
			var service = new SforceService
			{
				Timeout = Configuration.Timeout
			};

			var result = service.login(username, password);

			return new SalesforceSession
			{
				Id = result.sessionId
			};
		}
	}
}
