using System;

namespace Informa.Library.Salesforce
{
	public class SalesforceServiceExecutor<TService> : ISalesforceServiceExecutor<TService>
		where TService : ISalesforceServiceContext
	{
		public TService Service { get; set; }

		public SalesforceServiceExecutor(
			TService service)
		{
			Service = service;
		}

		public TResult Execute<TResult>(Func<TResult> function)
		{
			var result = function();

			return result;
		}
	}
}
