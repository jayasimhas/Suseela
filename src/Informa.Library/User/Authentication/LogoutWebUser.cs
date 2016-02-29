using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Security.Authentication;

namespace Informa.Library.User.Authentication
{
	[AutowireService(LifetimeScope.Default)]
	public class LogoutWebUser : ILogoutWebUser
	{
		public void Logout()
		{
			AuthenticationManager.Logout();
		}
	}
}
