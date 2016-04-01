using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Company
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class CompanyNameContext : ICompanyNameContext
	{
		protected readonly ICompanyContext CompanyContext;

		public CompanyNameContext(
			ICompanyContext companyContext)
		{
			CompanyContext = companyContext;
		}

		public string Name => CompanyContext.Company?.Name ?? string.Empty;
	}
}
