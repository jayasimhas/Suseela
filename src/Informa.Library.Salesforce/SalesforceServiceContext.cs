using Informa.Library.Salesforce.EBIWebServices;
using System;
using System.Linq;
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
		}

		public void RefreshSession()
		{
			if (string.Equals(Service.SessionHeaderValue.sessionId, SessionContext.Session.Id))
			{
				SessionContext.Refresh();
			}

			Service.SessionHeaderValue.sessionId = SessionContext.Session.Id;
		}

		public TResult Execute<TResult>(Expression<Func<ISalesforceService, TResult>> functionExpression)
			where TResult : IEbiResponse
		{
			if (!HasSession)
			{
				RefreshSession();
			}

			var function = functionExpression.Compile();
			var result = function(Service);

			// TODO-Sforce: Re-factor to abstract away
			if (result.errors != null && result.errors.Any(e => e != null && string.Equals(e.statusCode, "INVALID_SESSION_ID", StringComparison.InvariantCultureIgnoreCase)))
			{
				RefreshSession();

				return Execute(functionExpression);
			}

			// TODO-Sforce: Check for critical errors and log

			return result;
		}

		public bool HasSession => !string.IsNullOrWhiteSpace(Service.SessionHeaderValue.sessionId);
	}
}
