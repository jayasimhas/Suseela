using Sitecore.ContentSearch.Linq.Common;
using Velir.Search.Core.Reference;
using Velir.Search.Models;

namespace Velir.Search.Core.Search.Sorts
{
	public class SortOption
	{
		public SortOption()
		{
			
		}

		public SortOption(ISort sort)
		{
			FieldName = sort.Field_Name;
			SortDirection = sort.Sort_Ascending ? SortDirection.Ascending : SortDirection.Descending;
		}

		public SortOption(string sortBy, string sortDirection)
		{
			FieldName = sortBy;
			SortDirection = sortDirection == SiteSettings.QueryString.SortAscendingValue
				? SortDirection.Ascending
				: SortDirection.Descending;
		}

		public string FieldName { get; set; }
		public SortDirection SortDirection { get; set; }
	}
}