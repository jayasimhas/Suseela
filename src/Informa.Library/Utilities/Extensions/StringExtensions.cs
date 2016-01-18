using System.Text.RegularExpressions;

namespace Informa.Library.Utilities.Extensions
{
	public static class StringExtensions
	{
		public static string StripHtml(this string source)
		{
			return Regex.Replace(source, "<[^>]*>", "", RegexOptions.Compiled).Replace("  ", " ");
		}
	}
}
