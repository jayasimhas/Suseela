using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
	[AutowireService(LifetimeScope.Default)]
	public class EntitlementChecksEnabled : IEntitlementChecksEnabled
	{
		private const string ChecksEnabledSessionKey = "ChecksEnabled";

		protected readonly IEntitlementSession Session;

		public EntitlementChecksEnabled(
			IEntitlementSession session)
		{
			Session = session;
		}

		public bool IsEnabled
		{
			get
			{
				var isEnabled = Session.Get<bool?>(ChecksEnabledSessionKey);

				if (isEnabled == null || !isEnabled.HasValue)
				{
					return IsEnabled = true;
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
