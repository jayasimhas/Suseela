using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.Company
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class AllowedRegisterUserCompanyTypes : IAllowedRegisterUserCompanyTypes
	{
		public IEnumerable<CompanyType> Types => new List<CompanyType> { CompanyType.SiteLicenseIP };
	}
}
