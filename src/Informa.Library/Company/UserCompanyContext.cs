using Informa.Library.User;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Company
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserCompanyContext : IUserCompanyContext
	{
		private const string sessionKey = nameof(UserCompanyContext);

		protected readonly IFindCompanyByIpAddress FindCompanyByIpAddress;
		protected readonly IFindCompanyByUser FindCompany;
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IUserIpAddressContext UserIpAddressContext;
		protected readonly IUserSession UserSession;

		public UserCompanyContext(
			IFindCompanyByIpAddress findCompanyByIpAddress,
			IFindCompanyByUser findCompany,
			IAuthenticatedUserContext userContext,
			IUserIpAddressContext userIpAddressContext,
			IUserSession userSession)
		{
			FindCompanyByIpAddress = findCompanyByIpAddress;
			FindCompany = findCompany;
			UserContext = userContext;
			UserIpAddressContext = userIpAddressContext;
			UserSession = userSession;
		}

		public ICompany Company
		{
			get
			{
				var companySession = UserSession.Get<ICompany>(sessionKey);

				if (companySession.HasValue && companySession.Value != null)
				{
					return companySession.Value;
				}

				var company = FindCompany.Find(UserContext.User);

				if (company == null)
				{
					var ipAddress = UserIpAddressContext.IpAddress;

					if (ipAddress != null)
					{
						company = FindCompanyByIpAddress.Find(ipAddress);
					}
				}

				Company = company;

				return company;
			}
			set
			{
				UserSession.Set(sessionKey, value);
			}
		}

		public void Clear()
		{
			UserSession.Clear(sessionKey);
		}
	}
}
