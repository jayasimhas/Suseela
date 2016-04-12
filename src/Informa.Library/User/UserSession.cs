using Informa.Library.Session;

namespace Informa.Library.User
{
	public class UserSession : SpecificSessionStore, IUserSession
	{
		private const string userSessionStoreId = nameof(UserSession);

		public UserSession(
			ISessionStore sessionStore)
			: base(sessionStore)
		{
			
		}

		public override string Id => userSessionStoreId;
	}
}
