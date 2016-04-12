using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService(LifetimeScope.Default)]
	public class WebIsActiveRegistration : IWebIsActiveRegistration
	{
		protected readonly IWebRegisterUserContext RegisterUserContext;

		public WebIsActiveRegistration(
			IWebRegisterUserContext registerUserContext)
		{
			RegisterUserContext = registerUserContext;
		}

		public bool IsActive => RegisterUserContext.NewUser != null;
	}
}
