using Informa.Library.User.Registration;

namespace Informa.Library.Company
{
	public class CompanyRegisterUser : IRegisterUser
	{
		protected readonly IUserCompanyContext UserCompanyContext;
		protected readonly IRegisterCompanyUser RegisterCompanyUser;

		public CompanyRegisterUser(
			IUserCompanyContext userCompanyContext,
			IRegisterCompanyUser registerCompanyUser)
		{
			UserCompanyContext = userCompanyContext;
			RegisterCompanyUser = registerCompanyUser;
		}

		public bool Register(INewUser newUser)
		{
			return RegisterCompanyUser.Register(newUser, UserCompanyContext.Company);
		}
	}
}
