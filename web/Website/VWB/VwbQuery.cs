using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Elsevier.Web.VWB.Report.Columns;

namespace Elsevier.Web.VWB
{
	public class VwbQuery
	{
		public string PublicationCodes;
		public bool ShouldRun;
		/// <summary>
		/// Guid representing "Next Issue" option
		/// </summary>
		public const string NextIssueValue = "0882b72b-2a05-4bc7-a187-cca414b65520";
		public const string RunParam = "run";
		public string PublicationIdValue;
		public const string PublicationIdParam = "pid";
		public string IssueIdValue;
		public const string IssueIdParam = "iid";
		/// <summary>
		/// Delimited list of columns in order
		/// </summary>
		public string ColumnOrderValue;
		public const string ColumnOrderParam = "cord";
		public const char ColumnOrderDelimiter = ',';
		/// <summary>
		/// List of column keys in order
		/// </summary>
		public List<string> ColumnKeysInOrder;
		/// <summary>
		/// If not descending, it is ascending on the column designated
		/// in SortColumnKey
		/// </summary>
		public bool Descending;
		public const string DescendingParam = "desc";
		/// <summary>
		/// Key of the column to sort by
		/// </summary>
		public string SortColumnKey;
		public const string SortColumnParam = "csort";
		public DateTime? StartDate = null;
		public const string StartDateParam = "sd";
		public DateTime? EndDate = null;
		public const string EndDateParam = "ed";

		public const string NumResultsParam = "max";
		public int? NumResultsValue;

		public const string InProgressParam = "inprogress";
		public bool InProgressValue;


		public VwbQuery()
		{
			ColumnKeysInOrder = new List<string>();
		}

		public VwbQuery(HttpRequest request)
		{
			PublicationIdValue = request[PublicationIdParam];
			IssueIdValue = request[IssueIdParam];
			ColumnOrderValue = request[ColumnOrderParam];
			SortColumnKey = request[SortColumnParam];
			if (ColumnOrderValue != null)
			{
				ColumnKeysInOrder = ColumnOrderValue.Split(ColumnOrderDelimiter).ToList();
			}
			else
			{
				ColumnKeysInOrder = new List<string>();
			}
			Descending = request[DescendingParam] == "1";
			//ShouldRun = request[RunParam] == "1";
			ShouldRun = true;
			if (request[EndDateParam] != null)
			{
				EndDate = GetDateTime(request[EndDateParam]);
			}
			if (request[StartDateParam] != null)
			{
				StartDate = GetDateTime(request[StartDateParam]);
			}
			try
			{
				NumResultsValue = Int32.Parse(request[NumResultsParam]);
			}
			catch (Exception)
			{
				NumResultsValue = null;
			}
			if (request[InProgressParam] != null)
			{
				InProgressValue = true;
			}
			else
			{
				InProgressValue = false;
			}
			if (request["pubCodes"] != null)
			{
				PublicationCodes = request["pubCodes"];
			}
			else
			{
				PublicationCodes = string.Empty;
			}
		}

		/// <summary>
		/// parses date from MMddyyyy
		/// </summary>
		/// <param name="queryParam"></param>
		/// <returns></returns>
		public static DateTime? GetDateTime(string queryParam)
		{
			DateTime parsedTime;
			if (DateTime.TryParseExact(queryParam, "MMddyyyyhhmmtt", CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedTime))
			{
				return parsedTime;
			}
			return null;
		}

		public static string GetQueryDateTime(DateTime? dateTime)
		{
			if (dateTime == null) return null;
			return string.Format("{0:MMddyyyyhhmmtt}", dateTime);
		}

		/// <summary>
		/// Composes query string using PublicationIdValue, IssueIdValue,
		/// StartDate, EndDate, ColumnKeysInOrder, Descending, SortColumnKey, ShouldRun
		/// </summary>
		/// <returns></returns>
		public string GetQueryString()
		{
			string query = "";
			if (PublicationIdValue != null)
			{
				query += PublicationIdParam + "=" + PublicationIdValue + "&";
			}
			if (IssueIdValue != null)
			{
				query += IssueIdParam + "=" + IssueIdValue + "&";
			}
			if (StartDate != null)
			{
				query += StartDateParam + "=" + GetQueryDateTime(StartDate) + "&";
			}
			if (EndDate != null)
			{
				query += EndDateParam + "=" + GetQueryDateTime(EndDate) + "&";
			}
			if (ColumnKeysInOrder.Count > 0)
			{
				query += ColumnOrderParam + "="
						+ String.Join(ColumnOrderDelimiter.ToString(), ColumnKeysInOrder.ToArray()) + "&";
			}
			if (Descending)
			{
				query += DescendingParam + "=1&";
			}
			if (NumResultsValue != null)
			{
				query += NumResultsParam + "=" + NumResultsValue + "&";
			}
			if (InProgressValue)
			{
				query += InProgressParam + "=" + InProgressValue + "&";
			}
			if (SortColumnKey != null)
			{
				query += SortColumnParam + "=" + SortColumnKey + "&";
			}
			if (ShouldRun)
			{
				query += RunParam + "=1&";
			}
			if (string.IsNullOrEmpty(PublicationCodes) == false)
			{
				query += "pubCodes=" + PublicationCodes + "&";
			}
			query += "sc_mode=normal";
			return query;
		}

		public void MoveColumnLeft(string columnKey)
		{
			if (ColumnKeysInOrder.Contains(columnKey))
			{
				var prev = ColumnKeysInOrder.IndexOf(columnKey);
				if (prev > 0)
				{
					ColumnKeysInOrder.Insert(prev - 1, columnKey);
					ColumnKeysInOrder.RemoveAt(prev + 1);
				}
			}
		}

		public void MoveColumnRight(string columnKey)
		{
			if (ColumnKeysInOrder.Contains(columnKey))
			{
				var prev = ColumnKeysInOrder.IndexOf(columnKey);
				if (prev < ColumnKeysInOrder.Count - 1)
				{
					ColumnKeysInOrder.Insert(prev + 2, columnKey);
					ColumnKeysInOrder.RemoveAt(prev);
				}
			}
		}

		public VwbQuery Clone()
		{
			var clone = new VwbQuery();
			var temp = new string[ColumnKeysInOrder.Count];
			ColumnKeysInOrder.CopyTo(temp);

			clone.ColumnKeysInOrder = temp.ToList();
			clone.StartDate = StartDate;
			clone.EndDate = EndDate;
			clone.ShouldRun = ShouldRun;
			clone.SortColumnKey = SortColumnKey;
			clone.PublicationIdValue = PublicationIdValue;
			clone.IssueIdValue = IssueIdValue;
			clone.ColumnOrderValue = ColumnOrderValue;
			clone.Descending = Descending;
			clone.NumResultsValue = NumResultsValue;
			clone.InProgressValue = InProgressValue;
			clone.PublicationCodes = PublicationCodes;
			return clone;
		}
	}
}