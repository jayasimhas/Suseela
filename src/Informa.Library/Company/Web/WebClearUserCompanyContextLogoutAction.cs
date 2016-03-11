using Informa.Library.User.Authentication.Web;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Company.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebClearUserCompanyContextLogoutAction : IWebLogoutUserAction
	{
		protected readonly IUserCompanyContext UserCompanyContext;

		public WebClearUserCompanyContextLogoutAction(
			IUserCompanyContext userCompanyContext)
		{
			UserCompanyContext = userCompanyContext;
		}

		public void Process(IAuthenticatedUser user)
		{
			UserCompanyContext.Company = null;
		}
	}
}
