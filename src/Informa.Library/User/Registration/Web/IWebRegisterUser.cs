namespace Informa.Library.User.Registration.Web
{
	public interface IWebRegisterUser
	{
		IRegisterUserResult Register(INewUser newUser);
	}
}