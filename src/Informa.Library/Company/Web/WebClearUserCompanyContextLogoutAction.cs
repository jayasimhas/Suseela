using Informa.Library.User.Authentication.Web;
using Jabberwocky.Glass.Autofac.Attributes;
using Informa.Library.User;

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

		public void Process(IUser user)
		{
			UserCompanyContext.Clear();
		}
	}
}
