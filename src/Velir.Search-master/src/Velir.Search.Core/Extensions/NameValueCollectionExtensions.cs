using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Velir.Search.Core.Reference;

namespace Velir.Search.Core.Extensions
{
	/// <summary>
	/// Extensions for NameValueCollections
	/// </summary>
	public static class NameValueCollectionExtensions
	{
		/// <summary>
		/// To the query parameter dictionary.
		/// </summary>
		/// <param name="queryParams">The query parameters.</param>
		/// <param name="ignoredParams">The ignored parameters.</param>
		/// <returns></returns>
		public static IDictionary<string, string> ToQueryParamDictionary(this NameValueCollection queryParams, params string[] ignoredParams)
		{
			if (queryParams == null)
			{
				return null;
			}

			var paramsDictionary = new Dictionary<string, string>();

			foreach (string key in queryParams)
			{
				if (ignoredParams.Any(p => p == key)) continue;

				if (!paramsDictionary.ContainsKey(key))
				{
					paramsDictionary[key] = HttpUtility.HtmlEncode(queryParams[key]);
				}
				else
				{
					paramsDictionary[key] = string.Format("{0}{1}{2}", paramsDictionary[key], SiteSettings.ValueSeparator, HttpUtility.HtmlEncode(queryParams[key]));
				}

			}

			return paramsDictionary;
		}
	}
}
