using Informa.Library.Session;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.SiteDebugging
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SiteDebugger : ISiteDebugger
	{
		private const string IsDebuggingSessionKey = "IsDebugging.";

		protected readonly ISpecificSessionStores SessionStores;
		protected readonly ISiteDebuggingSession DebugSession;

		public SiteDebugger(
			ISpecificSessionStores sessionStores,
			ISiteDebuggingSession debugSession)
		{
			SessionStores = sessionStores;
			DebugSession = debugSession;
		}

		public void StartDebugging(string key)
		{
			SetDebugging(key, true);
		}

		public void StopDebugging(string key)
		{
			SessionStores.Where(ss => ss.Id != DebugSession.Id).ToList().ForEach(ss => ss.Clear());
			SetDebugging(key, false);
		}

		public void SetDebugging(string key, bool isDebugging)
		{
			DebugSession.Set(CreateSessionKey(key), isDebugging);
		}

		public bool IsDebugging(string key)
		{
			var isDebugging = DebugSession.Get<bool?>(CreateSessionKey(key));

			if (isDebugging == null || !isDebugging.HasValue)
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
