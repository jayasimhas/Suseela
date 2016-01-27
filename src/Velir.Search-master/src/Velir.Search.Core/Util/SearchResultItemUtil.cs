using System;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace Velir.Search.Core.Util
{
	/// <summary>
	/// Utility class for working with Search and SearchResult classes
	/// </summary>
	public class SearchResultItemUtil
	{
		/// <summary>
		/// Gets the name of the property, given the Sitecore field name.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns></returns>
		public static string GetPropertyName<T>(string fieldName) where T : SearchResultItem
		{
			var propertyInfo = typeof(T).GetProperties()
				.FirstOrDefault(
					p =>
						p.GetCustomAttributes(typeof(IndexFieldAttribute), false)
							.FirstOrDefault(a => ((IndexFieldAttribute)a).IndexFieldName.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase)) != null);
			if (propertyInfo != null)
			{
				return propertyInfo.Name;
			}

			return null;
		}
	}
}
