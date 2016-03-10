namespace Informa.Library.User.Registration.Web
{
	public interface IWebRegisterUserAction
	{
		void Process(INewUser newUser);
	}
}
