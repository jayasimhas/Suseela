using Jabberwocky.Glass.Autofac.Attributes;
using System.Web;

namespace Informa.Library.User.ResetPassword.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebUserResetPasswordTokenContext : IWebUserResetPasswordTokenContext
	{
		protected readonly IWebUserResetPasswordUrlConfiguration Configuration;

		public WebUserResetPasswordTokenContext(
			IWebUserResetPasswordUrlConfiguration configuration)
		{
			Configuration = configuration;
		}

		public string Token => HttpContext.Current.Request[Configuration.Parameter];
	}
}
