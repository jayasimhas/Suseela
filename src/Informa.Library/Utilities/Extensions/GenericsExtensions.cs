using System.Collections.Generic;

namespace Informa.Library.Utilities.Extensions
{
	public static class GenericsExtensions
	{
		public static List<T> SingleToList<T>(this T source)
		{
			return new List<T> { source };
		}
	}
}
