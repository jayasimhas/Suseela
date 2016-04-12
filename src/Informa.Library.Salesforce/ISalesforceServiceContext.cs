using Informa.Library.Salesforce.EBIWebServices;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Informa.Library.Salesforce
{
	public interface ISalesforceServiceContext
	{
		TResult Execute<TResult>(Expression<Func<ISalesforceService, TResult>> functionExpression, [CallerMemberName] string CallerMemberName = "", [CallerFilePath] string CallerFilePath = "", [CallerLineNumber] int CallerLineNumber = 0)
			where TResult : IEbiResponse;
	}
}
