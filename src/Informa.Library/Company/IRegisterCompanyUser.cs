using Informa.Library.User.Registration;

namespace Informa.Library.Company
{
	public interface IRegisterCompanyUser
	{
		bool Register(INewUser newUser, ICompany company);
	}
}
