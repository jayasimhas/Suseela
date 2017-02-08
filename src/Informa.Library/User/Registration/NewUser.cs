namespace Informa.Library.User.Registration
{
	public class NewUser : INewUser
	{
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Password { get; set; }
		public string MasterId { get; set; }
		public string MasterPassword { get; set; }

        public string AccessToken { get; }
    }
}
