using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore;

namespace Informa.Library.User.Authentication
{
	[AutowireService(LifetimeScope.Default)]
	public class SitecoreVirtualUsernameFactory : ISitecoreVirtualUsernameFactory
	{
		public string Create(IAuthenticatedUser user)
		{
			return string.Format("{0}\\{1}", Context.Domain.Name, user.Username);
		}
	}
}
