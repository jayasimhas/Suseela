using Informa.Library.Session;

namespace Informa.Library.User
{
	public class UserSession : SpecificSessionStore, IUserSession
	{
		private const string userSessionStoreId = "User";

		public UserSession(
			ISessionStore sessionStore)
			: base(sessionStore)
		{
			
		}

		public override string SessionStoreId => userSessionStoreId;
	}
}
