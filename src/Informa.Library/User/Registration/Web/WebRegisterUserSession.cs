using Informa.Library.Session;

namespace Informa.Library.User.Registration.Web
{
	public class WebRegisterUserSession : SpecificSessionStore, IWebRegisterUserSession
	{
		private const string sessionKey = nameof(WebRegisterUserSession);

		public WebRegisterUserSession(
			IUserSession sessionStore)
			: base(sessionStore)
		{

		}

		public override string Id => sessionKey;
	}
}
