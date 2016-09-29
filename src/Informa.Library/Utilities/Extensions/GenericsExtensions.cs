using System;
using System.Collections.Generic;

namespace Informa.Library.Utilities.Extensions
{
	public static class GenericsExtensions
	{
		public static List<T> SingleToList<T>(this T source)
		{
			return new List<T> { source };
		}

	    public static T Alter<T>(this T source, Action<T> action)
	    {
	        action(source);
	        return source;
	    }
        public static T Alter<T>(this T source, Action action)
        {
            action();
            return source;
        }
	}
}
