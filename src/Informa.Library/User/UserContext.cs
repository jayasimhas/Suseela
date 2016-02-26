using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserContext : IUserContext
	{
		protected readonly ISitecoreUserContext SitecoreUserContext;

		public UserContext(
			ISitecoreUserContext sitecoreUserContext)
		{
			SitecoreUserContext = sitecoreUserContext;
		}

		public IUser User
		{
			get
			{
				var sitecoreUser = SitecoreUserContext.User;

				return new User
				{
					Email = sitecoreUser.Profile.Email,
					Name = sitecoreUser.Profile.Name,
					Username = sitecoreUser.Profile.UserName
				};
			}
		}
	}
}
