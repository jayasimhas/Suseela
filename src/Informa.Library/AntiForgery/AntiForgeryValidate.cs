using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Informa.Library.AntiForgery
{
	using System.Web.Helpers;

	public class AntiForgeryValidate : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var cookieToken = string.Empty;
			var formToken = string.Empty;
			var tokenHeaders = Enumerable.Empty<string>();

			if (actionContext.Request.Headers.TryGetValues("RequestVerificationToken", out tokenHeaders))
			{
				var tokens = tokenHeaders.First().Split(':');

				if (tokens.Length == 2)
				{
					cookieToken = tokens[0].Trim();
					cookieToken = tokens[1].Trim();
				}
			}

			AntiForgery.Validate(cookieToken, formToken);

			base.OnActionExecuting(actionContext);
		}
	}
}
