using Informa.Library.Salesforce.EBIWebServices;
using Newtonsoft.Json;
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
        protected readonly ISalesforceDebugLogger DebugLogger;
        protected readonly ISalesforceServiceContextEnabled ServiceContextEnabled;

        public SalesforceServiceContext(
            ISalesforceService service,
            ISalesforceSessionContext sessionContext,
            ISalesforceErrorLogger errorLogger,
                        ISalesforceDebugLogger debugLogger,
            ISalesforceServiceContextEnabled serviceContextEnabled)
        {
            Service = service;
            SessionContext = sessionContext;
            ErrorLogger = errorLogger;
            DebugLogger = debugLogger;
            ServiceContextEnabled = serviceContextEnabled;

            Service.SessionHeaderValue = new SessionHeader();
        }

        public void RefreshSession()
        {
            DebugLogger.Log($"Refresh Session: Header Value: {Service.SessionHeaderValue.sessionId}, ContextValue: {SessionContext.Session.Id}");

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

        public TResult Execute<TResult>(Func<ISalesforceService, TResult> function, string CallerMemberName = "", string CallerFilePath = "",
                        int CallerLineNumber = 0)
            where TResult : IEbiResponse
        {
            DebugLogger.Log($"Salesforced called by: {CallerMemberName}, File: {CallerFilePath}, Line Number {CallerLineNumber}");
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
                DebugLogger.Log($"Invalid Session from caller: {function.Method.Name}");
                return Execute(function);
            }

            if (!result.IsSuccess() && result.errors != null)
            {
                foreach (var error in result.errors)
                {
                    DebugLogger.Log($"Request Failed: {error?.message}");
                }

            }

            return result;
        }

        public bool HasSession => !string.IsNullOrWhiteSpace(Service.SessionHeaderValue.sessionId);
    }
}
