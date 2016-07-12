using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Informa.Library.Utilities.WebUtils
{
	public static class UrlUtils
	{
		public static string AddParameterToUrl(HttpRequest request, string parameterName, string parameterValue)
		{
			return AddParameterToUrl(request.Url.Scheme + "://" + request.Url.Host + request.RawUrl, parameterName, parameterValue);
		}

		public static string AddParameterToUrl(string fullUrl, string parameterName, string parameterValue)
		{
			try
			{
				Uri uri = new Uri(fullUrl);
				NameValueCollection parameters = HttpUtility.ParseQueryString(uri.Query);
				if (!string.IsNullOrEmpty(parameters[parameterName]))
					parameters[parameterName] = parameterValue;
				else
					parameters.Add(parameterName, parameterValue);
				return new UriBuilder(uri.Scheme, uri.Host)
				{
					Path = uri.AbsolutePath,
					Query = ConstructQueryString(parameters, false)
				}.ToString();
			}
			catch
			{
				return string.Empty;
			}
		}

		public static string ConstructQueryString(NameValueCollection parameters, bool splitOnCommas)
		{
			var query = HttpUtility.ParseQueryString(string.Empty);
			foreach (var item in parameters.AllKeys.SelectMany(parameters.GetValues, (k, v) => new { key = k, value = v }))
			{
				query[item.key] = item.value;
			}

			return query.ToString();

		}

		public static string ClearUrlParameters(HttpRequest request)
		{
			return ClearUrlParameters(request.Url.Scheme + "://" + request.Url.Host + request.RawUrl);
		}

		public static string ClearUrlParameters(string fullUrl)
		{
			try
			{
				Uri uri = new Uri(fullUrl);
				return new UriBuilder(uri.Scheme, uri.Host)
				{
					Path = uri.AbsolutePath,
					Query = string.Empty
				}.ToString();
			}
			catch
			{
				return string.Empty;
			}
		}

	    public static string GetLastUrlSement(HttpContextBase context)
	    {
	        return context?.Request?.Url?.Segments.Last();
	    }
	}
}
