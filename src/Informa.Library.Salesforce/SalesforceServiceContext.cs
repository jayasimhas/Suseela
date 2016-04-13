using Informa.Library.Salesforce.EBIWebServices;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Informa.Library.Salesforce
{
	public class SalesforceServiceContext : ISalesforceServiceContext
	{
		private const string InvalidSessionIdErrorKey = "INVALID_SESSION_ID";

		protected readonly ISalesforceService Service;
		protected readonly ISalesforceSessionContext SessionContext;
		protected readonly ISalesforceErrorLogger ErrorLogger;

		public SalesforceServiceContext(
			ISalesforceService service,
			ISalesforceSessionContext sessionContext,
			ISalesforceErrorLogger errorLogger)
		{
			Service = service;
			SessionContext = sessionContext;
			ErrorLogger = errorLogger;

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
            //TODO: Salesforce logging.
		    ErrorLogger.Log($"Salesforced called by: {CallerMemberName}, File: {CallerFilePath}, Line Number {CallerLineNumber}", null);

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

		    if (!result.IsSuccess())
		    {
		        foreach (var error in result.errors)
		        {
		            ErrorLogger.Log($"Request Failed: {error?.message}", null);
		        }

		    }

		    return result;
		}

		public bool HasSession => !string.IsNullOrWhiteSpace(Service.SessionHeaderValue.sessionId);
	}
}
