using Glass.Mapper.Sc;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.ResetPassword.Web
{
	[AutowireService(LifetimeScope.PerScope)]
	public class WebUserResetPasswordUrlFactory : IWebUserResetPasswordUrlFactory
	{
		protected readonly ISitecoreContext SitecoreContext;
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IWebUserResetPasswordUrlConfiguration Configuration;

		public WebUserResetPasswordUrlFactory(
			ISitecoreContext sitecoreContext,
			ISiteRootContext siteRootContext,
			IWebUserResetPasswordUrlConfiguration configuration)
		{
			SitecoreContext = sitecoreContext;
			SiteRootContext = siteRootContext;
			Configuration = configuration;
		}

		public string Create(IUserResetPassword userResetPassword)
		{
			var item = SitecoreContext.GetItem<I___BasePage>(SiteRootContext.Item.Reset_Password_Page);
			var resetPasswordUrl = item?._AbsoluteUrl ?? string.Empty;

			return string.Format("{0}?{1}={2}", resetPasswordUrl, Configuration.Parameter, userResetPassword.Token);
		}
	}
}
