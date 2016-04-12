using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService(LifetimeScope.Default)]
	public class WebRegisterUserContext : IWebRegisterUserContext
	{
		private const string sessionKey = nameof(WebRegisterUserContext);

		protected readonly IWebRegisterUserSession UserSession;

		public WebRegisterUserContext(
			IWebRegisterUserSession userSession)
		{
			UserSession = userSession;
		}

		public INewUser NewUser
		{
			get
			{
				return UserSession.Get<INewUser>(sessionKey).Value;
			}
			set
			{
				UserSession.Set(sessionKey, value);
			}
		}
	}
}
