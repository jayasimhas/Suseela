using System;
using System.Collections;
using System.Collections.Generic;

namespace Informa.Library.Utilities.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();

			foreach (TSource element in source)
			{
				if (seenKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}

	    public static bool IsNullOrEmpty(IEnumerable collection)
	    {
            if(collection == null) { return true; }

	        var e = collection.GetEnumerator();
            var any = !e.MoveNext();
            e.Reset();
	        return !any;
	    }
	}
}
