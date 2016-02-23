using System;

namespace Informa.Library.Salesforce
{
	public static class ISalesforceServiceContextExtensions
	{
		public static TResult Execute<TSource, TResult>(this TSource source, Func<TResult> function)
			where TSource : ISalesforceServiceContext
		{
			return function();
		}
	}
}
