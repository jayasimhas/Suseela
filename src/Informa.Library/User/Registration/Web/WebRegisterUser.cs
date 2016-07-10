using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService]
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

		public IRegisterUserResult Register(INewUser newUser)
		{
			var registerResult = RegisterUser.Register(newUser);

			if (registerResult.Success)
			{
				RegisterUserContext.NewUser = newUser;
				RegisterUserActions.Process(newUser);
			}

			return registerResult;
		}
	}
}
