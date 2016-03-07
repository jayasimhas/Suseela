using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebRegisterUser : IWebRegisterUser
	{
		protected readonly IRegisterUser RegisterUser;

		public WebRegisterUser(
			IRegisterUser registerUser)
		{
			RegisterUser = registerUser;
		}

		public bool Register(INewUser newUser)
		{
			// TODO: Add actions for sending email etc.

			return RegisterUser.Register(newUser);
		}
	}
}
