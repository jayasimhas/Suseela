using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using Sitecore.ContentSearch.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Velir.Search.Core.Facets.Sort;
using Velir.Search.Core.Results.Facets;
using Velir.Search.Models;

namespace Informa.Library.Search.Sort
{
	public class SitecoreOrderStrategy : HierarchicalStrategy
	{
		private readonly ISitecoreService _service;
		public SitecoreOrderStrategy(IHierarchical_Sort_Strategy sortItem) : base(sortItem)
		{
			_service = new SitecoreContext();
		}

		public override IEnumerable<FacetResultValue> OrderResults(IEnumerable<FacetValue> allValues, IEnumerable<string> selectedValues)
		{
			var facetDict = new Dictionary<string, FacetResultValue>();
			foreach (var value in allValues)
			{
				facetDict[value.Name] = new FacetResultValue(value.Name, value.AggregateCount, selectedValues.Any(val => val == value.Name));
			}

			var fieldName = InnerHierarchicalSortItem.Field_Name;
			var validTemplates = InnerHierarchicalSortItem.Valid_Templates.Select(guid => new ID(guid)).ToArray();
			var rootItem = _service.GetItem<Item>(InnerHierarchicalSortItem.Root_Item._Id);

			foreach (var root in rootItem.Children.Where(c => validTemplates.Any(id => id == c.TemplateID)))
			{
				var fieldVal = root[fieldName];
				if (facetDict.ContainsKey(fieldVal))
				{
					yield return facetDict[fieldVal];
				}
			}
		}
	}
}