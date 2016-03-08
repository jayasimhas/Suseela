namespace Informa.Library.User.Registration
{
	public interface ISetOptInsRegisterUser
	{
		bool Set(INewUser newUser, bool offers, bool newsletters);
	}
}
