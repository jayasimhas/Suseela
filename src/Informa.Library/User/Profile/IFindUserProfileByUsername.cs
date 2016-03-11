namespace Informa.Library.User.Profile
{
	public interface IFindUserProfileByUsername
	{
		IUserProfile Find(string username);
	}
}
