using Informa.Library.User.Registration;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.Company
{
	[AutowireService(LifetimeScope.Default)]
	public class CompanyRegisterUser : IRegisterUser
	{
		protected readonly IUserCompanyContext UserCompanyContext;
		protected readonly IRegisterCompanyUser RegisterCompanyUser;
		protected readonly IAllowedRegisterUserCompanyTypes AllowedCompanyTypes;

		public CompanyRegisterUser(
			IUserCompanyContext userCompanyContext,
			IRegisterCompanyUser registerCompanyUser,
			IAllowedRegisterUserCompanyTypes allowedCompanyTypes)
		{
			UserCompanyContext = userCompanyContext;
			RegisterCompanyUser = registerCompanyUser;
			AllowedCompanyTypes = allowedCompanyTypes;
		}

		public bool Register(INewUser newUser)
		{
			var company = UserCompanyContext.Company;

		    if (UserCompanyContext.Company == null)
		        return RegisterCompanyUser.Register(newUser, null);

			return RegisterCompanyUser.Register(newUser, AllowedCompanyTypes.Types.Contains(company.Type) ? company : null);
		}
	}
}
