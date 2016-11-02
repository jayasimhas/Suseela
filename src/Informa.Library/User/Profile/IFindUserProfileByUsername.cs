namespace Informa.Library.User.Profile
{
    public interface IFindUserProfileByUsername
    {
        IUserProfile Find(string username);
        IUserProfile Find(string userId, string url, string sessionId);
    }
}
