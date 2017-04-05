using Informa.Library.User.Authentication.Web;
using Jabberwocky.Glass.Autofac.Attributes;
using Informa.Library.User;
using Informa.Library.SalesforceConfiguration;

namespace Informa.Library.Company.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebSetUserCompanyContextLoginAction : IWebLoginUserAction
	{
		protected readonly IFindCompanyByUser FindCompany;
		protected readonly IUserCompanyContext UserCompanyContext;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;

        public WebSetUserCompanyContextLoginAction(
			IFindCompanyByUser findCompany,
			IUserCompanyContext userCompanyContext,
             ISalesforceConfigurationContext salesforceConfigurationContext)
		{
			FindCompany = findCompany;
			UserCompanyContext = userCompanyContext;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

		public void Process(IUser user)
		{
            if (!SalesforceConfigurationContext.IsNewSalesforceEnabled)
            {
                var company = FindCompany.Find(user);

                UserCompanyContext.Company = company;
            }
		}
	}
}
