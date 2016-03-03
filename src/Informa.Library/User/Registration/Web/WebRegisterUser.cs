namespace Informa.Library.User.Registration.Web
{
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
