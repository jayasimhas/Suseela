using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Authentication
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class AuthenticatedUserContext : IAuthenticatedUserContext
	{
		protected readonly ISitecoreUserContext SitecoreUserContext;

		public AuthenticatedUserContext(
			ISitecoreUserContext sitecoreUserContext)
		{
			SitecoreUserContext = sitecoreUserContext;
		}

		public IAuthenticatedUser User
		{
			get
			{
				var sitecoreUser = SitecoreUserContext.User;

				return new AuthenticatedUser
				{
					Email = sitecoreUser.Profile.Email,
					Name = sitecoreUser.Profile.Name,
					Username = sitecoreUser.Profile.UserName
				};
			}
		}

		public bool IsAuthenticated => SitecoreUserContext.User.IsAuthenticated;
	}
}
