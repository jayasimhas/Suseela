using Informa.Library.User.Authentication.Web;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Company.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebSetUserCompanyContextLoginAction : IWebLoginUserAction
	{
		protected readonly IFindCompanyByUser FindCompany;
		protected readonly IUserCompanyContext UserCompanyContext;

		public WebSetUserCompanyContextLoginAction(
			IFindCompanyByUser findCompany,
			IUserCompanyContext userCompanyContext)
		{
			FindCompany = findCompany;
			UserCompanyContext = userCompanyContext;
		}

		public void Process(IAuthenticatedUser authenticatedUser)
		{
			var company = FindCompany.Find(authenticatedUser);

			UserCompanyContext.Company = company;
		}
	}
}
