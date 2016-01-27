using System.Web;
using System.Collections.Generic;

namespace Velir.Search.Core.Extensions
{
	/// <summary>
	/// Extensions for List of Key/Value pairs
	/// </summary>
	public static class KeyValuePairExtensions
	{
		/// <summary>
		/// To the query parameter dictionary.
		/// </summary>
		/// <param name="queryParams">The query parameters.</param>
		/// <returns></returns>
		public static IDictionary<string, string> ToQueryParamDictionary(this IEnumerable<KeyValuePair<string, string>> queryParams)
		{
			if (queryParams == null)
			{
				return null;
			}

			var paramsDictionary = new Dictionary<string, string>();

			foreach (var pair in queryParams)
			{
				if (!paramsDictionary.ContainsKey(pair.Key))
				{
					paramsDictionary[pair.Key] = pair.Value;
				}
				else
				{
					paramsDictionary[pair.Key] = string.Format("{0};{1}", paramsDictionary[pair.Key], HttpUtility.HtmlEncode(pair.Value));
				}

			}

			return paramsDictionary;
		}
	}
}
