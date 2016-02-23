using System;

namespace Informa.Library.Salesforce
{
	public interface ISalesforceServiceExecutor<TService>
		where TService : ISalesforceServiceContext
	{
		TService Service { get; }
		TResult Execute<TResult>(Func<TResult> function);
	}
}
