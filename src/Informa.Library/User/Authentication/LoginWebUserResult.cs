namespace Informa.Library.User.Authentication
{
	public class LoginWebUserResult : ILoginWebUserResult
	{
		public AuthenticateUserResultState State { get; set; }
		public bool Success { get; set; }
		public IAuthenticatedUser User { get; set; }
	}
}
