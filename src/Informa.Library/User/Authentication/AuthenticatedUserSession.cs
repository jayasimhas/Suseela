using Informa.Library.Session;

namespace Informa.Library.User.Authentication
{
	public class AuthenticatedUserSession : SpecificSessionStore, IAuthenticatedUserSession
	{
		private const string sessionStoreId = "Authenticated";

		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;

		public AuthenticatedUserSession(
			IAuthenticatedUserContext authenticatedUserContext,
			IUserSession userSession)
			: base(userSession)
		{
			AuthenticatedUserContext = authenticatedUserContext;
		}

		public override string Id => string.Concat(sessionStoreId, ".", AuthenticatedUserContext.User?.Username ?? string.Empty);

		public override ISessionValue<T> Get<T>(string key)
		{
			if (!AuthenticatedUserContext.IsAuthenticated)
			{
				Clear(key);
			}

			return base.Get<T>(key);
		}

		public override void Set<T>(string key, T obj)
		{
			if (AuthenticatedUserContext.IsAuthenticated)
			{
				base.Set(key, obj);
			}
		}
	}
}
