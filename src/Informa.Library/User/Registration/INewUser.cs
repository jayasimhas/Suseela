namespace Informa.Library.User.Registration
{
	public interface INewUser
	{
		string Username { get; set; }
		string FirstName { get; set; }
		string LastName { get; set; }
		string Password { get; set; }
	}
}
