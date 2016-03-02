using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.ResetPassword.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebUserResetPasswordUrlConfiguration : IWebUserResetPasswordUrlConfiguration
	{
		public string Parameter => "rptoken";
	}
}
