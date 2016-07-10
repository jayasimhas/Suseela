using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Company
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserCompanyNameContext : IUserCompanyNameContext
	{
		protected readonly IUserCompanyContext UserCompanyContext;

		public UserCompanyNameContext(IUserCompanyContext userCompanyContext)
		{
			UserCompanyContext = userCompanyContext;
		}

		public string Name => UserCompanyContext.Company?.Name ?? string.Empty;

		public string CompanyId => UserCompanyContext.Company?.Id ?? string.Empty;
	}
}
