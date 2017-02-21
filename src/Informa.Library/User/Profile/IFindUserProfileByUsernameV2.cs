namespace Informa.Library.User.Profile
{
    public interface IFindUserProfileByUsernameV2
    {
        IUserProfile Find(string accessToken);
    }
}
