using System.Collections.Generic;

namespace Informa.Library.Utilities.Extensions
{
	public static class ListExtensions
	{
		public static void AddRange<T>(this IList<T> source, IEnumerable<T> collection)
		{
			foreach (var obj in collection)
			{
				source.Add(obj);
			}
		}
	}
}
