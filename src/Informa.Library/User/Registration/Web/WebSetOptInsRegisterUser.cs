using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebSetOptInsRegisterUser : IWebSetOptInsRegisterUser
	{
		protected readonly IWebRegisterUserContext RegisterUserContext;
		protected readonly ISetOptInsRegisterUser SetOptInsRegisterUser;
	    protected readonly IAuthenticatedUserContext AuthenticatedUserContext;

		public WebSetOptInsRegisterUser(
			IWebRegisterUserContext registerUserContext,
			ISetOptInsRegisterUser setOptInsRegisterUser,
            IAuthenticatedUserContext authenticatedUserContext)
		{
			RegisterUserContext = registerUserContext;
			SetOptInsRegisterUser = setOptInsRegisterUser;
		    AuthenticatedUserContext = authenticatedUserContext;
		}

		public bool Set(bool offers, bool newsletters)
		{
			var newUser = RegisterUserContext.NewUser;

			if (newUser == null)
			{
                var username = AuthenticatedUserContext?.User?.Username;
                if (!string.IsNullOrEmpty(username))
                    return SetOptInsRegisterUser.Set(username, offers, newsletters);
            }

			var success = SetOptInsRegisterUser.Set(newUser, offers, newsletters);

			return success;
		}
	}
}
