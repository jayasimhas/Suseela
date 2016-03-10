using Informa.Library.User;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Company
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserCompanyContext : IUserCompanyContext
	{
		private const string CompanySessionKey = "Company";

		protected readonly IUserSession UserSession;

		public UserCompanyContext(
			IUserSession userSession)
		{
			UserSession = userSession;
		}

		public ICompany Company
		{
			get
			{
				return UserSession.Get<ICompany>(CompanySessionKey);
			}
			set
			{
				UserSession.Set(CompanySessionKey, value);
			}
		}
	}
}
