using System.Web.Mvc;

namespace Informa.Library.AntiForgery
{
	using System.Web.Helpers;

	public static class HtmlHelperExtensions
	{
		public static string RequestVerificationToken(this HtmlHelper source)
		{
			var cookieToken = string.Empty;
			var formToken = string.Empty;

			AntiForgery.GetTokens(null, out cookieToken, out formToken);

			return string.Format("{0}:{1}", cookieToken, formToken);
		}
	}
}
