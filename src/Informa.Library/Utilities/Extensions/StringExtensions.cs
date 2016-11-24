using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

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

			if (oldValue == null || newValue == null)
			{
				Sitecore.Diagnostics.Log.Error($"OldValue or NewValue is NULL. Source: '{source}', OldValue: '{oldValue ?? string.Empty}', NewValue: '{newValue ?? string.Empty}'", typeof(StringExtensions));
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
			var querystring = url.Split('&');
			var param = querystring.FirstOrDefault(p => p.StartsWith($"{key}="));
			return param?.Split('=')[1] ?? string.Empty;
		}
		
		public static string GetByLine(this IEnumerable<string> names)
		{
			if (names == null) return string.Empty;

			var count = names.Count();

			if (count == 0) return string.Empty;

			if (count == 1) return names.FirstOrDefault();

			if (count == 2) return $"{names.First()} and {names.Last()}";

			return $"{string.Join(", ", names.Take(count - 1))} and {names.LastOrDefault()}";
		}

	    public static bool HasContent(this string source)
	    {
	        return !string.IsNullOrEmpty(source);
	    }

		public static string ExtractGuidString(this string source)
		{
			var startIndex = source.IndexOf("{", StringComparison.Ordinal);
			var endIndex = source.IndexOf('}');
			return source.Substring(startIndex, endIndex - startIndex + 1);
		}

        public static string NullIfNoContent(this string source)
        {
            return string.IsNullOrEmpty(source) ? null : source;
        }
        
        /// <summary>
        /// Uses DateTime.TryParse to parse the string as a DateTime.  If the TryParse fails, returns DateTime.MinValue.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
	    public static DateTime ToDate(this string source)
	    {
	        DateTime parsed;
	        if (!DateTime.TryParse(source, out parsed))
	        {
	            parsed = DateTime.MinValue;
	        }
            return parsed;
	    }
	}
}
