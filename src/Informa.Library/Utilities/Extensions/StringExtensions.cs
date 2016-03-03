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

		public static string ReplaceCaseInsensitive(this string source, string oldValue, string newValue)
		{
			if (source == null)
			{
				return null;
			}

			return Regex.Replace(source, oldValue, newValue, RegexOptions.IgnoreCase);
		}

		public static string ReplaceCaseInsensitive(this string source, Dictionary<string, string> replacements)
		{
			if (source == null)
			{
				return null;
			}

			replacements.ToList().ForEach(kvp => source = source.ReplaceCaseInsensitive(kvp.Key, kvp.Value));

			return source;
		}
	}
}
