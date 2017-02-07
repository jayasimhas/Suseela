namespace Informa.Library.User.Profile
{
    public interface IUserInfo
    {
        string UserName { get; set; }
        string Name { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string NickName { get; set; }
    }
}