using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Informa.Library.Utilities.WebUtils;
using Sitecore.Web;

namespace Informa.Library.Utilities.Extensions
{
	public static class StringExtensions
	{
		public static string StripHtml(this string source)
		{
			return Regex.Replace(source, "<[^>]*>", "", RegexOptions.Compiled).Replace("  ", " ");
		}

		public static string ReplacePatternCaseInsensitive(this string source, string oldValue, string newValue)
		{
			if (source == null)
			{
				return null;
			}

			return Regex.Replace(source, oldValue, newValue, RegexOptions.IgnoreCase);
		}

		public static string ReplacePatternCaseInsensitive(this string source, Dictionary<string, string> replacements)
		{
			if (source == null)
			{
				return null;
			}

			replacements.ToList().ForEach(kvp => source = source.ReplacePatternCaseInsensitive(kvp.Key, kvp.Value));

			return source;
		}

		/// <summary>
		/// Replaces a given substring with a replacement substring, and returns the entire resulting string using case-insensitive matching
		/// </summary>
		/// <param name="source">The original string to perform replacement on</param>
		/// <param name="oldValue">The substring to replace</param>
		/// <param name="newValue">The substring to use as a replacement for oldValue</param>
		/// <returns>The original string, with all oldValue substring instances replaced by newValue</returns>
		public static string ReplaceCaseInsensitive(string source, string oldValue, string newValue)
		{
			if (source == null)
			{
				return null;
			}

			return ReplacePatternCaseInsensitive(source, Regex.Escape(oldValue), newValue);
		}

		public static string ExtractParamValue(this string url, string key)
		{
			return WebUtil.ExtractUrlParm(key, url);
		}
	}
}
