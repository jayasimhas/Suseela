using System.Collections.Generic;
using Velir.Search.Core.Rules.Parser;
using Velir.Search.Models;

namespace Velir.Search.Core.Page
{
	public interface ISearchPageParser
	{
		ISearchRuleParser RuleParser { get; }
		I_Listing_Configuration ListingConfiguration { get; }
		IEnumerable<ISort> SortOptions { get; }
		IEnumerable<I_Refinement> RefinementOptions { get; }
	}
}
