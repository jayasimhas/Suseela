namespace Informa.Library.User.Profile
{
    public interface IUserProfileFactoryV2
    {
        IUserProfile Create(IUser user);
    }
}
