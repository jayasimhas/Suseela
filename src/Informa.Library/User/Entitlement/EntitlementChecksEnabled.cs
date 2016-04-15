using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
	[AutowireService(LifetimeScope.Default)]
	public class EntitlementChecksEnabled : IEntitlementChecksEnabled
	{
		private const string ChecksEnabledSessionKey = "ChecksEnabled";

		protected readonly ISitecoreUserContext SitecoreUserContext;
		protected readonly IEntitlementSession Session;

		public EntitlementChecksEnabled(
			ISitecoreUserContext sitecoreUserContext,
			IEntitlementSession session)
		{
			SitecoreUserContext = sitecoreUserContext;
			Session = session;
		}

		public bool Enabled
		{
			get
			{
				var isEnabled = Session.Get<bool>(ChecksEnabledSessionKey);

				if (!isEnabled.HasValue)
				{
					return Enabled = !SitecoreUserContext.User.IsAdministrator;
				}

				return isEnabled.Value;
			}
			set
			{
				bool? isEnabled = value;

				Session.Set(ChecksEnabledSessionKey, isEnabled);
			}
		}
	}
}
