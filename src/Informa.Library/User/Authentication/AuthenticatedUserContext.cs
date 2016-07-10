using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Authentication
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class AuthenticatedUserContext : IAuthenticatedUserContext
	{
		protected readonly ISitecoreUserContext SitecoreUserContext;

		public AuthenticatedUserContext(ISitecoreUserContext sitecoreUserContext)
		{
			SitecoreUserContext = sitecoreUserContext;
		}

		public IAuthenticatedUser User
		{
			get
			{
                if(!IsAuthenticated) { return null; }

				var sitecoreUser = SitecoreUserContext.User;

                //Although sitecoreUser.Profile.Email gets filled and saved when creating the user, after the browser gets closed and reopened the Email becomes NULL.
                //So had to find another way to retrieve it when extranet
                string email = sitecoreUser.Profile.Email;
                if (string.IsNullOrEmpty(email) && sitecoreUser.IsAuthenticated && sitecoreUser.Domain.Name == "extranet")
                {
                    sitecoreUser.RuntimeSettings.IsVirtual = true;
                    sitecoreUser.Profile.Email = sitecoreUser.LocalName;
                }

                return new AuthenticatedUser
				{
					Email = sitecoreUser.LocalName,
					Name = sitecoreUser.Profile.Name,
					Username = sitecoreUser.LocalName
				};
			}
		}

		public bool IsAuthenticated => SitecoreUserContext.User.IsAuthenticated;             
	}                                      
}
