using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Authentication.Web.SiteDebugging
{
	public class SiteDebuggingWebLoginUser
	{
		protected readonly IWebLoginUser LoginUser;
		protected readonly IWebLogoutUser LogoutUser;

		public SiteDebuggingWebLoginUser(
			IWebLoginUser loginUser,
			IWebLogoutUser logoutUser)
		{
			LoginUser = loginUser;
			LogoutUser = logoutUser;
		}

		public void StartDebugging(string username)
		{
			StopDebugging();
			
		}

		public void StopDebugging()
		{
			LogoutUser.Logout();
		}
	}
}
