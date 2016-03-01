namespace Informa.Library.User.Authentication
{
	public class LoginWebUserResult : ILoginWebUserResult
	{
		public bool Success { get; set; }
		public string Message { get; set; }
	}
}
