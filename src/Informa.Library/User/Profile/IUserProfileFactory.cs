namespace Informa.Library.User.Profile
{
	public interface IUserProfileFactory
	{
		IUserProfile Create(IUser user);
	}
}
