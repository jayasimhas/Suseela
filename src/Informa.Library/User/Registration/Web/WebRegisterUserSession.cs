using Informa.Library.Session;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebRegisterUserSession : IWebRegisterUserSession, IWebSetRegisterUserSession
	{
		private const string NewUserSessionKey = "WebRegisterUserSession";

		protected readonly ISessionStore SessionStore;

		public WebRegisterUserSession(
			ISessionStore sessionStore)
		{
			SessionStore = sessionStore;
		}

		public INewUser NewUser
		{
			get
			{
				return SessionStore.Get<INewUser>(NewUserSessionKey).Value;
			}
			set
			{
				SessionStore.Set(NewUserSessionKey, value);
			}
		}
	}
}
