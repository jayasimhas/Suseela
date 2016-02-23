using Informa.Library.Salesforce.EBIWebServices;
using System;
using System.Linq.Expressions;

namespace Informa.Library.Salesforce
{
	public class SalesforceServiceContext : ISalesforceServiceContext
	{
		protected readonly ISalesforceService Service;
		protected readonly ISalesforceSessionContext SessionContext;

		public SalesforceServiceContext(
			ISalesforceService service,
			ISalesforceSessionContext sessionContext)
		{
			Service = service;
			SessionContext = sessionContext;

			Service.SessionHeaderValue = new SessionHeader();

			RefreshSession();
		}

		public void RefreshSession()
		{
			Service.SessionHeaderValue.sessionId = SessionContext.Refresh().Id;
		}

		public TResult Execute<TResult>(Expression<Func<ISalesforceService, TResult>> functionExpression)
			where TResult : IEbiResponse
		{
			var function = functionExpression.Compile();

			return function(Service);
		}
	}
}
