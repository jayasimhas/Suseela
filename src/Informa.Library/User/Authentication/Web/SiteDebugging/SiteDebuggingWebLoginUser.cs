using Informa.Library.SiteDebugging;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Authentication.Web.SiteDebugging
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SiteDebuggingWebLoginUser : ISiteDebuggingWebLoginUser
	{
		private const string DebugWebLoginUserKey = "WebLoginUser";

		protected readonly IFindUserByEmail FindUser;
		protected readonly IWebLoginUser LoginUser;
		protected readonly IWebLogoutUser LogoutUser;
		protected readonly ISiteDebugger SiteDebugger;

		public SiteDebuggingWebLoginUser(
			IFindUserByEmail findUser,
			IWebLoginUser loginUser,
			IWebLogoutUser logoutUser,
			ISiteDebugger siteDebugger)
		{
			FindUser = findUser;
			LoginUser = loginUser;
			LogoutUser = logoutUser;
			SiteDebugger = siteDebugger;
		}

		public void StartDebugging(string username)
		{
			StopDebugging();

			var user = FindUser.Find(username);

			if (user == null)
			{
				return;
			}

			var result = LoginUser.Login(user, false);

			if (!result.Success)
			{
				return;
			}

			SiteDebugger.StartDebugging(DebugWebLoginUserKey);
		}

		public void StopDebugging()
		{
			LogoutUser.Logout();
			SiteDebugger.StopDebugging(DebugWebLoginUserKey);
		}

		public bool IsDebugging => SiteDebugger.IsDebugging(DebugWebLoginUserKey);
	}
}
