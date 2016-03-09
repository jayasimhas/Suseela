using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebRegisterUser : IWebRegisterUser
	{
		protected readonly IRegisterUser RegisterUser;
		protected readonly IWebSetRegisterUserSession RegisterUserSession;
		protected readonly IWebRegisterUserActions RegisterUserActions;

		public WebRegisterUser(
			IRegisterUser registerUser,
			IWebSetRegisterUserSession registerUserSession,
			IWebRegisterUserActions registerUserActions)
		{
			RegisterUser = registerUser;
			RegisterUserSession = registerUserSession;
			RegisterUserActions = registerUserActions;
		}

		public bool Register(INewUser newUser)
		{
			var registered = RegisterUser.Register(newUser);

			if (registered)
			{
				RegisterUserSession.NewUser = newUser;
				RegisterUserActions.ToList().ForEach(a => a.Process(newUser));
			}

			return registered;
		}
	}
}
