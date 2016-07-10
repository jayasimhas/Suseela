using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Authentication.Web
{
	[AutowireService(LifetimeScope.PerScope)]
	public class WebClearAuthenticatedUserSessionAction : IWebLoginUserAction, IWebLogoutUserAction
	{
		protected readonly IAuthenticatedUserSession UserSession;

		public WebClearAuthenticatedUserSessionAction(
			IAuthenticatedUserSession userSession)
		{
			UserSession = userSession;
		}

		public void Process(IUser value)
		{
			UserSession.Clear();
		}
	}
}
