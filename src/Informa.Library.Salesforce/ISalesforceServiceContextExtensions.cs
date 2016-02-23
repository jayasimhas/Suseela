using System;
using System.Linq.Expressions;

namespace Informa.Library.Salesforce
{
	public static class ISalesforceServiceContextExtensions
	{
		public static TResult Execute<TSource, TResult>(this TSource source, Expression<Func<TSource, TResult>> functionExpression)
			where TSource : ISalesforceServiceContext
		{
			var function = functionExpression.Compile();

			return function(source);
		}
	}
}
