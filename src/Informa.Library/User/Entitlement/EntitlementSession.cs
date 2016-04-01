using Informa.Library.Session;

namespace Informa.Library.User.Entitlement
{
	public class EntitlementSession : SpecificSessionStore, IEntitlementSession
	{
		private const string sessionStoreId = "Entitlement";

		public EntitlementSession(
			ISessionStore sessionStore)
			: base(sessionStore)
		{

		}

		public override string SessionStoreId => sessionStoreId;
	}
}
