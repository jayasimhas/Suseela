using Informa.Library.User;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Company
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserCompanyContext : IUserCompanyContext
	{
		private const string CompanySessionKey = "Company";

		protected readonly IFindCompanyByUser FindCompany;
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IUserSession UserSession;

		public UserCompanyContext(
			IFindCompanyByUser findCompany,
			IAuthenticatedUserContext userContext,
			IUserSession userSession)
		{
			FindCompany = findCompany;
			UserContext = userContext;
			UserSession = userSession;
		}

		public ICompany Company
		{
			get
			{
				var companySession = UserSession.Get<ICompany>(CompanySessionKey);

				if (companySession.HasValue || UserContext.User == null)
				{
					return companySession.Value;
				}

				var company = FindCompany.Find(UserContext.User);

				Company = company;

				return company;
			}
			set
			{
				UserSession.Set(CompanySessionKey, value);
			}
		}
	}
}
