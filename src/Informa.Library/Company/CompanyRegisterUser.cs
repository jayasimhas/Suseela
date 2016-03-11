using Informa.Library.User.Registration;

namespace Informa.Library.Company
{
	public class CompanyRegisterUser : IRegisterUser
	{
		protected readonly ICompanyContext CompanyContext;
		protected readonly IRegisterCompanyUser RegisterCompanyUser;

		public CompanyRegisterUser(
			ICompanyContext companyContext,
			IRegisterCompanyUser registerCompanyUser)
		{
			CompanyContext = companyContext;
			RegisterCompanyUser = registerCompanyUser;
		}

		public bool Register(INewUser newUser)
		{
			return RegisterCompanyUser.Register(newUser, CompanyContext.Company);
		}
	}
}
