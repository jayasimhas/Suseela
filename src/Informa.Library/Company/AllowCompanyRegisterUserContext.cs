using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.Company
{
	[AutowireService(LifetimeScope.Default)]
	public class AllowCompanyRegisterUserContext : IAllowCompanyRegisterUserContext
	{
		protected readonly IUserCompanyContext UserCompanyContext;
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
		protected readonly IAllowedRegisterUserCompanyTypes AllowedCompanyTypes;

		public AllowCompanyRegisterUserContext(
			IUserCompanyContext userCompanyContext,
			IAuthenticatedUserContext authenticatedUserContext,
			IAllowedRegisterUserCompanyTypes allowedCompanyTypes)
		{
			UserCompanyContext = userCompanyContext;
			AuthenticatedUserContext = authenticatedUserContext;
			AllowedCompanyTypes = allowedCompanyTypes;
		}

		public bool IsAllowed => !AuthenticatedUserContext.IsAuthenticated &&
								 UserCompanyContext.Company != null &&
								 AllowedCompanyTypes.Types.Contains(UserCompanyContext.Company.Type);
	}
}
