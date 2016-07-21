using System.Runtime.CompilerServices;
using Informa.Library.Salesforce.SalesforceAPI;

namespace Informa.Library.Salesforce
{
	public class SalesforceSessionFactory : ISalesforceSessionFactory
	{
		protected readonly ISalesforceSessionFactoryConfiguration Configuration;
	    protected readonly ISalesforceDebugLogger DebugLogger;

		public SalesforceSessionFactory(
			ISalesforceSessionFactoryConfiguration configuration, ISalesforceDebugLogger debugLogger)
		{
            DebugLogger = debugLogger;
			Configuration = configuration;
		}

		public ISalesforceSession Create([CallerMemberName] string CallerMemberName = "", [CallerFilePath] string CallerFilePath = "", [CallerLineNumber] int CallerLineNumber = 0)
		{
            //TODO: Salesforce logging.
            DebugLogger.Log($"Salesforced called by: {CallerMemberName}, File: {CallerFilePath}, Line Number {CallerLineNumber}");
            var username = Configuration.Username;
			var password = string.Concat(Configuration.Password, Configuration.Token);
			var service = new SforceService
			{
				Url = Configuration.Url,
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
