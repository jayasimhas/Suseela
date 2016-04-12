using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebSetOptInsRegisterUser : IWebSetOptInsRegisterUser
	{
		protected readonly IWebRegisterUserContext RegisterUserContext;
		protected readonly ISetOptInsRegisterUser SetOptInsRegisterUser;

		public WebSetOptInsRegisterUser(
			IWebRegisterUserContext registerUserContext,
			ISetOptInsRegisterUser setOptInsRegisterUser)
		{
			RegisterUserContext = registerUserContext;
			SetOptInsRegisterUser = setOptInsRegisterUser;
		}

		public bool Set(bool offers, bool newsletters)
		{
			var newUser = RegisterUserContext.NewUser;

			if (newUser == null)
			{
				return false;
			}

			var success = SetOptInsRegisterUser.Set(newUser, offers, newsletters);

			return success;
		}
	}
}
