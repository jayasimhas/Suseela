using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebRegisterUser : IWebRegisterUser
	{
		protected readonly IRegisterUser RegisterUser;
		protected readonly IWebSetRegisterUserSession RegisterUserSession;

		public WebRegisterUser(
			IRegisterUser registerUser,
			IWebSetRegisterUserSession registerUserSession)
		{
			RegisterUser = registerUser;
			RegisterUserSession = registerUserSession;
		}

		public bool Register(INewUser newUser)
		{
			var registered = RegisterUser.Register(newUser);

			if (registered)
			{
				RegisterUserSession.NewUser = newUser;

				// TODO: Add actions for sending email etc.
			}

			return registered;
		}
	}
}
