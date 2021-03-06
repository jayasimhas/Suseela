﻿using Glass.Mapper.Sc;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
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
			IWebUserResetPasswordUrlConfiguration configuration)
		{
			SitecoreContext = sitecoreContext;
			Configuration = configuration;
		}

		public string Create(IUserResetPassword userResetPassword)
		{
			var siteRootContext = SitecoreContext?.GetRootItem<ISite_Root>();

			var item = SitecoreContext.GetItem<I___BasePage>(siteRootContext.Reset_Password_Page);
			var resetPasswordUrl = item?._AbsoluteUrl ?? string.Empty;

			return string.Format("{0}?{1}={2}", resetPasswordUrl, Configuration.Parameter, userResetPassword.Token);
		}
	}
}
