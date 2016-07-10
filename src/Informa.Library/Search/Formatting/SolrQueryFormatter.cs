using System.Linq;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Search.Formatting
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SolrQueryFormatter : IQueryFormatter
	{
		private static readonly string[] _keywords = {" and ", " or ", " not "};
		private static readonly char[] _groupingCharacters = {'"', '(', ')'};

		public bool NeedsFormatting(string rawQuery)
		{
			string lowercaseQuery = rawQuery.ToLowerInvariant();
			return rawQuery.IndexOfAny(_groupingCharacters) >= 0 || _keywords.Any(k => lowercaseQuery.Contains(k));
		}

		public string FormatQuery(string rawQuery)
		{
			if (NeedsFormatting(rawQuery))
			{
				string lowercaseQuery = rawQuery.ToLowerInvariant();
				string[] queryParts = rawQuery.Split('"');

				for (int i = 0; i < queryParts.Length; i++)
				{
					if (i%2 == 1) continue;

					if( _keywords.Any(k => lowercaseQuery.Contains(k)))
					{
						queryParts[i] = queryParts[i].Replace(" and ", " AND ").Replace(" or ", " OR ").Replace(" not ", " NOT ");
					}
				}

				return string.Join("\"", queryParts);
			}

			return rawQuery;
		}
	}
}