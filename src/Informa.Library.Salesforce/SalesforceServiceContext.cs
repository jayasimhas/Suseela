using Informa.Library.Salesforce.EBIWebServices;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Informa.Library.Salesforce
{
	public class SalesforceServiceContext : ISalesforceServiceContext
	{
		private const string InvalidSessionIdErrorKey = "INVALID_SESSION_ID";

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

			var result = default(TResult);
			var invalidSession = false;

			try
			{
				var function = functionExpression.Compile();

				result = function(Service);

				invalidSession = result.errors != null && result.errors.Any(e => e != null && string.Equals(e.statusCode, InvalidSessionIdErrorKey, StringComparison.InvariantCultureIgnoreCase));
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains(InvalidSessionIdErrorKey))
				{
					invalidSession = true;
					SessionContext.Refresh();
				}
			}

			if (invalidSession)
			{
				RefreshSession();

				return Execute(functionExpression);
			}

			return result;
		}

		public bool HasSession => !string.IsNullOrWhiteSpace(Service.SessionHeaderValue.sessionId);
	}
}
