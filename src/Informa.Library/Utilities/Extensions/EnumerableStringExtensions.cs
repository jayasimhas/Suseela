using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Utilities.Extensions
{
	public static class EnumerableStringExtensions
	{
		public static string JoinWithFinal(this IEnumerable<string> source, string separator, string finalSeparator)
		{
			if (!source.Any())
			{
				return string.Empty;
			}

			if (source.Count() == 1)
			{
				return source.First();
			}

			return $"{string.Join(separator, source.Take(source.Count() - 1))} {finalSeparator} {source.Last()}";
		}
	}
}
