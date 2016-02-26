namespace Informa.Library.User.Authentication
{
	public interface ISitecoreVirtualUsernameFactory
	{
		string Create(IAuthenticatedUser user);
	}
}
