using System;
using System.Collections.Generic;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace Velir.Search.Core.Util
{
	/// <summary>
	/// Class of extension methods when working with SearchResultItems
	/// </summary>
	public static class SearchResultItemExtensions
	{
		/// <summary>
		/// Gets the numerical field value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns></returns>
		public static Decimal GetNumericalFieldValue<T>(this T item, string fieldName) where T : SearchResultItem
		{
			Decimal result;
			Decimal.TryParse(item[fieldName], out result);
			return result;
		}

		/// <summary>
		/// Gets the date field value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns></returns>
		public static DateTime GetDateFieldValue<T>(this T item, string fieldName) where T : SearchResultItem
		{
			DateTime result;
			DateTime.TryParse(item[fieldName], out result);
			return result;
		}

		/// <summary>
		/// Gets the multilist field value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns></returns>
		public static List<ID> GetMultilistFieldValue<T>(this T item, string fieldName) where T : SearchResultItem
		{
			List<ID> ids = new List<ID>();
			string fieldValue = item[fieldName];
			foreach (string id in fieldValue.Split('|'))
			{
				ids.Add(new ID(id));
			}
			return ids;
		}
	}
}