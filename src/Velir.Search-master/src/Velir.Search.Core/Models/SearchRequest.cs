using System.Collections.Generic;
using System.Linq;
using Jabberwocky.Glass.Factory;
using Velir.Search.Core.Page;
using Velir.Search.Core.Refinements;
using Velir.Search.Core.Search.Sorts;

namespace Velir.Search.Core.Models
{
	public class SearchRequest : ISearchRequest
	{
		private readonly ISearchPageParser _parser;
		private readonly IGlassInterfaceFactory _factory;

		public SearchRequest(ISearchPageParser parser, IGlassInterfaceFactory factory)
		{
			_parser = parser;
			_factory = factory;

			PageId = string.Empty;
			Page = 1;
			PerPage = parser.ListingConfiguration != null ? parser.ListingConfiguration.Items_Per_Page : 10;
			SortBy = parser.ListingConfiguration != null && parser.ListingConfiguration.Default_Sort_Order != null ? parser.ListingConfiguration.Default_Sort_Order.Field_Name : string.Empty;
			SortOrder = parser.ListingConfiguration != null && parser.ListingConfiguration.Default_Sort_Order != null && parser.ListingConfiguration.Default_Sort_Order.Sort_Ascending ? "asc" : "desc";
			QueryParameters = new Dictionary<string, string>();
		}

		public SearchRequest(int page, int perPage)
		{
			PageId = string.Empty;
			Page = page;
			PerPage = perPage;
			SortBy = string.Empty;
			SortOrder = string.Empty;
			QueryParameters = new Dictionary<string, string>();
		}

		public string PageId { get; set; }
		public int Page { get; set; }
		public int PerPage { get; set; }
		public string SortBy { get; set; }
		public string SortOrder { get; set; }
		public IDictionary<string, string> QueryParameters { get; set; }

		public IEnumerable<SortOption> GetSorts()
		{
			if (!string.IsNullOrEmpty(SortBy))
			{
				return _parser.SortOptions.Where(x => x.Key == SortBy).Select(x => new SortOption(x.Field_Name, SortOrder));
			}

			if (_parser.ListingConfiguration != null)
			{
				return new[] { new SortOption(_parser.ListingConfiguration.Default_Sort_Order) };	
			}

			return new SortOption[0];
		}

		public IEnumerable<ISearchRefinement> GetRefinements()
		{
			return _factory.GetItems<ISearchRefinement>(_parser.RefinementOptions);
		}
	}
}
