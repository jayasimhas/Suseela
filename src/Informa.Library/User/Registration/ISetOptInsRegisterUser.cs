namespace Informa.Library.User.Registration
{
	public interface ISetOptInsRegisterUser : ISetOptInsRegisterUserByUsername
    {
		bool Set(INewUser newUser, bool offers, bool newsletters);
    }

    public interface ISetOptInsRegisterUserByUsername
    {
        bool Set(string newUser, bool offers, bool newsletters);
    }
}
