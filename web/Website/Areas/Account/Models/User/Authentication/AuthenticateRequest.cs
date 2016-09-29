namespace Informa.Web.Areas.Account.Models.User.Authentication
{
	public class AuthenticateRequest
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public bool Persist { get; set; }
        public bool IsSignInFromMyView { get; set; }
	}
}