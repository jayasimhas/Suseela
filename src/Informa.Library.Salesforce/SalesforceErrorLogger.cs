using Informa.Library.Logging;
using System;

namespace Informa.Library.Salesforce
{
	public class SalesforceErrorLogger : ISalesforceErrorLogger
	{
		private const string LogMessagePrefix = "Salesforce Error: ";

		protected readonly IErrorLogger ErrorLogger;

		public SalesforceErrorLogger(
			IErrorLogger errorLogger)
		{
			ErrorLogger = errorLogger;
		}

		public void Log(string message, Exception ex)
		{
			ErrorLogger.Log(string.Concat(LogMessagePrefix, message), ex);
		}
	}

    public class SalesforceDebugLogger : ISalesforceDebugLogger
    {
        private const string LogMessagePrefix = "Salesforce Debug: ";

        protected readonly IDebugLogger DebugLogger;

        public SalesforceDebugLogger(
            IDebugLogger debugLogger)
        {
            DebugLogger = debugLogger;
        }

        public void Log(string message)
        {
            DebugLogger.Log(string.Concat(LogMessagePrefix, message));
        }
    }

    public class SalesforceInfoLogger : ISalesforceInfoLogger
    {
        private const string LogMessagePrefix = "Salesforce V2 Information Logging: ";

        protected readonly IInfoLogger InfoLogger;

        public SalesforceInfoLogger(
            IInfoLogger infoLogger)
        {
            InfoLogger = infoLogger;
        }

        public void Log(string message, string owner)
        {
            InfoLogger.Log(string.Concat(LogMessagePrefix, message), owner);
        }
    }
}
