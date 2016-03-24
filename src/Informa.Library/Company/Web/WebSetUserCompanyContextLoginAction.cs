using Informa.Library.User.Authentication.Web;
using Jabberwocky.Glass.Autofac.Attributes;
using Informa.Library.User;

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

		public void Process(IUser user)
		{
			var company = FindCompany.Find(user);

			UserCompanyContext.Company = company;
		}
	}
}
