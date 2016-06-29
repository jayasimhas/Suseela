namespace Informa.Library.User.Registration
{
	public interface IRegisterUser
	{
		IRegisterUserResult Register(INewUser newUser);
	}
}
