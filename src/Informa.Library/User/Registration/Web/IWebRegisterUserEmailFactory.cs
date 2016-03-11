using Informa.Library.Mail;

namespace Informa.Library.User.Registration.Web
{
	public interface IWebRegisterUserEmailFactory
	{
		IEmail Create(INewUser newUser);
	}
}