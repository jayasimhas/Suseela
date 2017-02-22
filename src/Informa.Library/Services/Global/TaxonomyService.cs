using Jabberwocky.Core.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Search.Utilities;
using Informa.Library.Taxonomy;
using Informa.Models.Informa.Models.sitecore.templates.Common;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Services.Global {
    
    [AutowireService(LifetimeScope.SingleInstance)]
    public class TaxonomyService : ITaxonomyService {

        protected readonly ICacheProvider CacheProvider;

        public TaxonomyService(
            ICacheProvider cacheProvider)
        {
            CacheProvider = cacheProvider;

        }

        private string CreateCacheKey(string suffix) {
            return $"{nameof(TaxonomyService)}-{suffix}";
        }
        
        public IEnumerable<HierarchyLinks> GetHeirarchyChildLinks(I___BaseTaxonomy taxItem)
        {
            string cacheKey = CreateCacheKey($"HeirarchyChildLinks-{taxItem._Id}");
            return CacheProvider.GetFromCache(cacheKey, () => BuildHeirarchyChildLinks(taxItem));
        }
        public IEnumerable<HierarchyLinks> BuildHeirarchyChildLinks(I___BaseTaxonomy taxItem)
        {
            var children = new List<HierarchyLinks>();

            Dictionary<Guid, HierarchyLinks> taxonomyItems = new Dictionary<Guid, HierarchyLinks>();

            foreach (var taxonomy in taxItem.Taxonomies) {
                var taxonomyTree = GetTaxonomyHierarchy(taxonomy);

                if (!taxonomyItems.ContainsKey(taxonomyTree.Item1._Id)) {
                    taxonomyItems.Add(taxonomyTree.Item1._Id, new HierarchyLinks {
                        Text = taxonomyTree.Item1._Name,
                        Url = string.Empty,
                        Children = new List<HierarchyLinks>()
                    });
                }

                var folderItem = taxonomyItems[taxonomyTree.Item1._Id];

                foreach (var item in taxonomyTree.Item3) {
                    if (!taxonomyItems.ContainsKey(item._Parent._Id)) {
                        taxonomyItems.Add(item._Parent._Id, new HierarchyLinks {
                            Text = item.Item_Name,
                            Url = SearchTaxonomyUtil.GetSearchUrl(item),
                            Children = new List<HierarchyLinks>()
                        });
                    }

                    var lItem = new HierarchyLinks {
                        Text = item.Item_Name,
                        Url = SearchTaxonomyUtil.GetSearchUrl(item),
                        Children = new List<HierarchyLinks>()
                    };

                    if (!taxonomyItems.ContainsKey(item._Id))
                        taxonomyItems.Add(item._Id, lItem);
                    var parent = taxonomyItems[item._Parent._Id];
                    var pList = parent.Children.ToList();
                    if (!pList.Any(a => a.Text.Equals(lItem.Text)))
                        pList.Add(lItem);

                    parent.Children = pList;
                }

                if (!children.Any(x => x.Text.Equals(folderItem.Text)))
                    children.Add(folderItem);
            }

            return children;
        }
        //ISW 338 Serving ads based on section taxonomy
        public string GetTaxonomyParentFromItem(ITaxonomy_Item taxonomy)
        {
            List<ITaxonomy_Item> taxonomyItems = new List<ITaxonomy_Item>();

            taxonomyItems.Add(taxonomy);
            var parent = taxonomy._Parent;

            while (parent is ITaxonomy_Item)
            {
                var item = parent as ITaxonomy_Item;

                taxonomyItems.Add(item);
                parent = item._Parent;
            }

            if ((parent is IFolder))    //Will add Or conditions after merged with R2,R3 Release (parent is IIndustries_Folder)
            {
                return parent._Name;
            }
            else
            {
                return string.Empty;
            }
        }
        private Tuple<IFolder, Guid, IEnumerable<ITaxonomy_Item>> GetTaxonomyHierarchy(ITaxonomy_Item taxonomy) {
            List<ITaxonomy_Item> taxonomyItems = new List<ITaxonomy_Item>();

            taxonomyItems.Add(taxonomy);
            var parent = taxonomy._Parent;

            while (parent is ITaxonomy_Item) {
                var item = parent as ITaxonomy_Item;

                taxonomyItems.Add(item);
                parent = item._Parent;
            }

            if (!(parent is IFolder)) {
                throw new InvalidCastException("Not the correct data structure");
            }
            taxonomyItems.Reverse();

            return new Tuple<IFolder, Guid, IEnumerable<ITaxonomy_Item>>(parent as IFolder, taxonomy._Parent._Id, taxonomyItems);
        }
    }
}
