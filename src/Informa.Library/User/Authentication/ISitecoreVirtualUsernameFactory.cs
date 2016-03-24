namespace Informa.Library.User.Authentication
{
	public interface ISitecoreVirtualUsernameFactory
	{
		string Create(IUser user);
	}
}
