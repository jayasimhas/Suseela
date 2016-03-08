using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch.Linq;
using Velir.Search.Core.Facets.Sort;
using Velir.Search.Core.Results.Facets;
using Velir.Search.Models;

namespace Informa.Library.Search.Sort
{
    public class HierarchicalStrategyWithCountSort : HierarchicalStrategy
    {
        public HierarchicalStrategyWithCountSort(IHierarchical_Sort_Strategy sortItem) : base(sortItem)
        {
        }

        public override IEnumerable<FacetResultValue> OrderResults(IEnumerable<FacetValue> allValues,
            IEnumerable<string> selectedValues)
        {
            List<FacetResultValue> results = base.OrderResults(allValues, selectedValues).ToList();

            //Sort the sub facets by count
            foreach (FacetResultValue facetResultValue in results)
            {
                facetResultValue.Sublist = facetResultValue.Sublist.OrderByDescending(x => x.Count);
            }

            //Sort the main facets by count
            return results.OrderByDescending(x=> x.Count);
        }
    }
}
