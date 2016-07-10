using Informa.Library.User.Registration;

namespace Informa.Library.Company
{
	public interface IRegisterCompanyUser
	{
		IRegisterUserResult Register(INewUser newUser, ICompany company);
	}
}
