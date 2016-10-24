namespace Informa.Library.User
{
	public interface IUser
	{
		string Username { get; }
        string SalesForceSessionId { get; set; }
        string SalesForceURL { get; set; }
    }
}
