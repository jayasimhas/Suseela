using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using Sitecore.ContentSearch.Linq;
using Velir.Search.Core.Facets.Sort;
using Velir.Search.Core.Results.Facets;
using Velir.Search.Models;
using Glass.Mapper.Sc;
using Sitecore.Data.Items;
using Sitecore.Collections;

namespace Informa.Library.Search.Sort
{
	public class HierarchicalStrategyWithCountSort : HierarchicalStrategy
	{
        private readonly ISitecoreService _service;

        public HierarchicalStrategyWithCountSort(IHierarchical_Sort_Strategy sortItem) : base(sortItem)
		{
            _service = new SitecoreContext();
        }

		public override IEnumerable<FacetResultValue> OrderResults(IEnumerable<FacetValue> allValues,
				IEnumerable<string> selectedValues)
		{
            var facetDict = new Dictionary<string, FacetResultValue>();
            foreach (var value in allValues) {
                facetDict[value.Name] = new FacetResultValue(value.Name, value.AggregateCount, selectedValues.Contains(value.Name));
            }

            var orderedResults = new List<FacetResultValue>();

            if (InnerHierarchicalSortItem.Root_Item == null) return orderedResults;

            var rootItem = _service.GetItem<Item>(InnerHierarchicalSortItem.Root_Item._Id);
            string fieldName = InnerHierarchicalSortItem.Field_Name;

            foreach (Item child in rootItem.Children) {
                if (!InnerHierarchicalSortItem.Valid_Templates.Contains(child.TemplateID.Guid)) continue;

                string fieldValue = child[fieldName];

                if (!facetDict.ContainsKey(fieldValue)) continue;

                var firstLevelValue = facetDict[fieldValue];

                firstLevelValue.Sublist = GetSublistItems(facetDict, child.Children);

                orderedResults.Add(firstLevelValue);
            }

			return orderedResults;
		}
        
        public IEnumerable<FacetResultValue> GetSublistItems(Dictionary<string, FacetResultValue> f, ChildList cl) {
            List<FacetResultValue> results = new List<FacetResultValue>();
            foreach (Item i in cl) {
                string s = i[InnerHierarchicalSortItem.Field_Name].Replace("\r", "").Replace("\n", "");
                if (!InnerHierarchicalSortItem.Valid_Templates.Contains(i.TemplateID.Guid))
                    continue;
                if (!f.ContainsKey(s))
                    continue;
                if (f[s] != null)
                    results.Add(f[s]);
            }
            return results.OrderByDescending(x => x.Count);
        }
    }
}
