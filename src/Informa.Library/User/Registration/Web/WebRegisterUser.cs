using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebRegisterUser : IWebRegisterUser
	{
		protected readonly IRegisterUser RegisterUser;
		protected readonly IWebRegisterUserContext RegisterUserContext;
		protected readonly IWebRegisterUserActions RegisterUserActions;

		public WebRegisterUser(
			IRegisterUser registerUser,
			IWebRegisterUserContext registerUserContext,
			IWebRegisterUserActions registerUserActions)
		{
			RegisterUser = registerUser;
			RegisterUserContext = registerUserContext;
			RegisterUserActions = registerUserActions;
		}

		public bool Register(INewUser newUser)
		{
			var registered = RegisterUser.Register(newUser);

			if (registered)
			{
				RegisterUserContext.NewUser = newUser;
				RegisterUserActions.Process(newUser);
			}

			return registered;
		}
	}
}
