using System.Web.Mvc;

namespace Informa.Library.AntiForgery
{
    using Autofac;
    using Jabberwocky.Glass.Autofac.Util;
    using System.Web.Helpers;
    using User.Authentication;
    public static class HtmlHelperExtensions
	{
		public static string RequestVerificationToken(this HtmlHelper source)
		{
			var cookieToken = string.Empty;
			var formToken = string.Empty;

			AntiForgery.GetTokens(null, out cookieToken, out formToken);

			return string.Format("{0}:{1}", cookieToken, formToken);
		}

        public static MvcHtmlString AuthToken(this HtmlHelper source)
        {
            IAuthenticatedFormToken authToken = null;
            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                authToken = scope.Resolve<IAuthenticatedFormToken>();
            }


            return new MvcHtmlString(string.Format("<input type='hidden' name='AuthToken' value='{0}'/>", authToken.GenerateMD5Token()));
        }
    }
}
