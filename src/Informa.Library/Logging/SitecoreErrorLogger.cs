using System;

namespace Informa.Library.Logging
{
	public class SitecoreErrorLogger : IErrorLogger
	{
		public void Log(string message, Exception ex)
		{
			Sitecore.Diagnostics.Log.Error(message, ex, string.Empty);
		}
	}
}
