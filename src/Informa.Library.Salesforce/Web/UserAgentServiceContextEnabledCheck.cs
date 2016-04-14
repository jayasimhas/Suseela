using Informa.Library.Session;
using System.Linq;

namespace Informa.Library.Salesforce.Web
{
	public class UserAgentServiceContextEnabledCheck : ISalesforceServiceContextEnabledCheck
	{
		private const string sessionKey = nameof(UserAgentServiceContextEnabledCheck);

		protected readonly ISessionStore SessionStore;
		protected readonly IUserAgentServiceContextEnabledCheckConfiguration Configuration;

		public UserAgentServiceContextEnabledCheck(
			ISessionStore sessionStore,
			IUserAgentServiceContextEnabledCheckConfiguration configuration)
		{
			SessionStore = sessionStore;
			Configuration = configuration;
		}

		public bool Enabled
		{
			get
			{
				var enabledSession = SessionStore.Get<bool>(sessionKey);

				if (enabledSession.HasValue)
				{
					return enabledSession.Value;
				}

				var enabled = Enabled = Configuration.UserAgents.Any(ua => ua == "Check User Agent Logic"); // TODO-Ladan: Replace with business logic for checking against user agents

				enabled = Enabled = true; // TODO-Ladan: Remove once implementation is done.

				return enabled;
			}
			set
			{
				SessionStore.Set(sessionKey, value);
			}
		}
	}
}
