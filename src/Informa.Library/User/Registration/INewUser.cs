namespace Informa.Library.User.Registration
{
	public interface INewUser : IUser
	{
		new string Username { get; set; }
		string FirstName { get; set; }
		string LastName { get; set; }
		string Password { get; set; }
		string MasterId { get; set; }
		string MasterPassword { get; set; }
	}
}
