using Informa.Library.Session;

namespace Informa.Library.SiteDebugging
{
	public class SiteDebuggingSession : SpecificSessionStore, ISiteDebuggingSession
	{
		private const string DebugSessionKeyPrefix = "Debug";

		public SiteDebuggingSession(
			ISessionStore sessionStore)
			: base(sessionStore)
		{
			
		}

		public override string Prefix => DebugSessionKeyPrefix;
	}
}
