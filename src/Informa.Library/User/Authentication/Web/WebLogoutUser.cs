using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Security.Authentication;

namespace Informa.Library.User.Authentication.Web
{
	[AutowireService(LifetimeScope.Default)]
	public class WebLogoutUser : IWebLogoutUser
	{
		public void Logout()
		{
			AuthenticationManager.Logout();
		}
	}
}
