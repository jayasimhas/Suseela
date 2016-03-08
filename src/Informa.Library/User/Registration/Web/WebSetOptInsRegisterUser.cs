using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebSetOptInsRegisterUser : IWebSetOptInsRegisterUser
	{
		protected readonly IWebRegisterUserSession RegisterUserSession;
		protected readonly ISetOptInsRegisterUser SetOptInsRegisterUser;

		public WebSetOptInsRegisterUser(
			IWebRegisterUserSession registerUserSession,
			ISetOptInsRegisterUser setOptInsRegisterUser)
		{
			RegisterUserSession = registerUserSession;
			SetOptInsRegisterUser = setOptInsRegisterUser;
		}

		public bool Set(bool offers, bool newsletters)
		{
			var newUser = RegisterUserSession.NewUser;

			if (newUser == null)
			{
				return false;
			}

			var success = SetOptInsRegisterUser.Set(newUser, offers, newsletters);

			return success;
		}
	}
}
