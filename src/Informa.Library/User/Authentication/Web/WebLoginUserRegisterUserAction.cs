using Informa.Library.User.Registration;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Authentication.Web
{
	[AutowireService]
	public class WebLoginUserRegisterUserAction : IWebLoginUserRegisterUserAction
	{
		protected readonly IWebLoginUser LoginUser;

		public WebLoginUserRegisterUserAction(
			IWebLoginUser loginUser)
		{
			LoginUser = loginUser;
		}

		public void Process(INewUser value)
		{
			LoginUser.Login(value, false);
		}
	}
}
