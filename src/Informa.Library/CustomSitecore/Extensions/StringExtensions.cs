namespace Informa.Library.CustomSitecore.Extensions
{
	public static class StringExtensions
	{
		public static bool IsStandardValues(this string source)
		{
			return string.Equals(source, "__Standard Values");
		}
	}
}
