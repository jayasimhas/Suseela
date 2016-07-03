using Informa.Library.Salesforce.EBIWebServices;
using System;
using System.Runtime.CompilerServices;

namespace Informa.Library.Salesforce
{
	public interface ISalesforceServiceContext
	{
		TResult Execute<TResult>(Func<ISalesforceService, TResult> function, [CallerMemberName] string CallerMemberName = "", [CallerFilePath] string CallerFilePath = "", [CallerLineNumber] int CallerLineNumber = 0)
			where TResult : IEbiResponse;
	}
}
