﻿namespace Informa.Library.User.Authentication.Web
{
	public class WebAuthenticateUserResult : IWebAuthenticateUserResult
	{
		public AuthenticateUserResultState State { get; set; }
		public bool Success { get; set; }
		public IUser User { get; set; }
	}
}
