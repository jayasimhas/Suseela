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
		protected readonly ISalesforceErrorLogger ErrorLogger;
		protected readonly ISalesforceServiceContextEnabled ServiceContextEnabled;

		public SalesforceServiceContext(
			ISalesforceService service,
			ISalesforceSessionContext sessionContext,
			ISalesforceErrorLogger errorLogger,
			ISalesforceServiceContextEnabled serviceContextEnabled)
		{
			Service = service;
			SessionContext = sessionContext;
			ErrorLogger = errorLogger;
			ServiceContextEnabled = serviceContextEnabled;

			Service.SessionHeaderValue = new SessionHeader();
		}

		public void RefreshSession()
		{
            ErrorLogger.Log("Refresh Session", new Exception($"Header Value: {Service.SessionHeaderValue.sessionId}, ContextValue: {SessionContext.Session.Id}"));

            if (string.Equals(Service.SessionHeaderValue.sessionId, SessionContext.Session.Id))
			{
				SessionContext.Refresh();
			}

		    try
		    {
		        Service.SessionHeaderValue.sessionId = SessionContext.Session.Id;
		    }
		    catch (Exception e)
		    {
                ErrorLogger.Log("Is this the 'Cannot change header value' issue I've been seeing?", e);
            }
		}

		public TResult Execute<TResult>(Expression<Func<ISalesforceService, TResult>> functionExpression, string CallerMemberName = "", string CallerFilePath = "",
            int CallerLineNumber = 0)
			where TResult : IEbiResponse
		{
		    ErrorLogger.Log($"Salesforced called by: {CallerMemberName}, File: {CallerFilePath}, Line Number {CallerLineNumber}", null);

			if (!ServiceContextEnabled.Enabled)
			{
				return default(TResult);
			}

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
                    ErrorLogger.Log("Invalid Session, infinite loop?", ex);
                    invalidSession = true;
					SessionContext.Refresh();
				}

				ErrorLogger.Log("Execute", ex);
			}

			if (invalidSession)
			{
				RefreshSession();
                ErrorLogger.Log("Invalid Session", new Exception("Infinite loop?"));
                return Execute(functionExpression);
			}

            if(!result.IsSuccess())
                ErrorLogger.Log($"Request Failed: {result.errors[0]}", null);

			return result;
		}

		public bool HasSession => !string.IsNullOrWhiteSpace(Service.SessionHeaderValue.sessionId);
	}
}
