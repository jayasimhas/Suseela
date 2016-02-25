using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Informa.Library.Utilities.WebApi.Filters
{
	using System.Web.Helpers;

	public class ValidateCsrfAttribute : FilterAttribute, IAuthorizationFilter
	{
		private const string HeaderTokenName = "RequestVerificationToken";

		public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken,
													Func<Task<HttpResponseMessage>> continuation)
		{
			string cookieToken;
			IEnumerable<string> tokenHeaders;
			if (actionContext.Request.Headers.TryGetValues(HeaderTokenName, out tokenHeaders))
			{
				var headers = actionContext.Request.Headers;
				var cookie = headers
					.GetCookies()
					.Select(c => c[AntiForgeryConfig.CookieName])
					.FirstOrDefault();

				cookieToken = cookie == null ? null : cookie.Value;
			}
			else
			{
				cookieToken = null;
			}

			try
			{
				AntiForgery.Validate(cookieToken, tokenHeaders.FirstOrDefault());
			}
			catch (Exception)
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
			}

			return continuation();
		}
	}
}
