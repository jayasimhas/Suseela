using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Utilities.Extensions
{
	public static class EnumerableStringExtensions
	{
		public static string JoinWithFinal(this IEnumerable<string> source, string separator, string finalSeparator)
		{
			IList<string> indexedSource = source as IList<string> ?? source.ToArray();
			if (!indexedSource.Any())
			{
				return string.Empty;
			}

			if (indexedSource.Count == 1)
			{
				return indexedSource.First();
			}

			return $"{string.Join(separator, indexedSource.Take(indexedSource.Count - 1))} {finalSeparator} {indexedSource.Last()}";
		}
	}
}
