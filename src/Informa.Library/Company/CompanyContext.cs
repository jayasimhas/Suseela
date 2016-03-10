using Informa.Library.User;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Company
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class CompanyContext : ICompanyContext
	{
		protected readonly IFindCompanyByIpAddress FindCompanyByIpAddress;
		protected readonly IUserCompanyContext UserCompanyContext;
		protected readonly IUserIpAddressContext UserIpAddressContext;

		public CompanyContext(
			IFindCompanyByIpAddress findCompanyByIpAddress,
			IUserCompanyContext userCompanyContext,
			IUserIpAddressContext userIpAddressContext)
		{
			FindCompanyByIpAddress = findCompanyByIpAddress;
			UserCompanyContext = userCompanyContext;
			UserIpAddressContext = userIpAddressContext;
		}

		public ICompany Company
		{
			get
			{
				var company = UserCompanyContext.Company;

				if (company != null)
				{
					return company;
				}

				var ipAddress = UserIpAddressContext.IpAddress;

				if (ipAddress == null)
				{
					return null;
				}

				company = FindCompanyByIpAddress.Find(ipAddress);

				if (company == null)
				{
					return null;
				}

				return UserCompanyContext.Company = company;
			}
		}
	}
}
