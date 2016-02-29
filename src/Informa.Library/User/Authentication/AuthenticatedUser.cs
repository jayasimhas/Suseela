namespace Informa.Library.User.Authentication
{
	public class AuthenticatedUser : IAuthenticatedUser
	{
		public string Username { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
	}
}
