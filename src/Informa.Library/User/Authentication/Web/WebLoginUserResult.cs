using System.Collections.Generic;
using Informa.Library.User.Entitlement;

namespace Informa.Library.User.Authentication.Web
{
	public class WebLoginUserResult : IWebLoginUserResult
	{
		public AuthenticateUserResultState State { get; set; }
		public bool Success { get; set; }
		public IAuthenticatedUser User { get; set; }
	}
}
