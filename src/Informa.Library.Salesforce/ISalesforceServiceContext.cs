using System;
using System.Linq.Expressions;

namespace Informa.Library.Salesforce
{
	public interface ISalesforceServiceContext
	{
		TResult Execute<TResult>(Expression<Func<ISalesforceService, TResult>> functionExpression);
	}
}
