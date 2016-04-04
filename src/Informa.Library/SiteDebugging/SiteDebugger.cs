using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.SiteDebugging
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SiteDebugger : ISiteDebugger
	{
		private const string IsDebuggingSessionKey = "IsDebugging.";

		protected readonly ISiteDebuggingSession DebugSession;

		public SiteDebugger(
			ISiteDebuggingSession debugSession)
		{
			DebugSession = debugSession;
		}

		public void StartDebugging(string key)
		{
			SetDebugging(key, true);
		}

		public void StopDebugging(string key)
		{
			SetDebugging(key, false);
		}

		public void SetDebugging(string key, bool isDebugging)
		{
			DebugSession.Set(CreateSessionKey(key), isDebugging);
		}

		public bool IsDebugging(string key)
		{
			var isDebugging = DebugSession.Get<bool>(CreateSessionKey(key));

			if (!isDebugging.HasValue)
			{
				return false;
			}

			return isDebugging.Value;
		}

		public string CreateSessionKey(string key)
		{
			return string.Concat(IsDebuggingSessionKey, key);
		}
	}
}
