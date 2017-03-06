﻿namespace Informa.Library.User.Authentication.Web
{
	public interface IWebAuthenticateUser
	{
		IWebAuthenticateUserResult Authenticate(string username, string password, bool persist);
        IWebAuthenticateUserResult Authenticate(string username, string password, bool persist, string verticalName);
        IWebAuthenticateUserResult Authenticate(string code, string redirectUrl,string verticalName);
        IWebAuthenticateUserResult Authenticate(IAuthenticatedUser user);
        IAuthenticatedUser AuthenticatedUser { get; set; }
	}
}