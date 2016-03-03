using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.Search.Utilities;
using Informa.Models.Informa.Models.sitecore.templates.Common;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
    public class HierarchyLinksViewModel : GlassViewModel<I___BaseTaxonomy>, IHierarchyLinks
    {
        private HierarchyLinks model;
        protected readonly ITextTranslator TextTranslator;
        public HierarchyLinksViewModel(
            I___BaseTaxonomy glassModel,
            ITextTranslator textTranslator)
        {
            TextTranslator = textTranslator;

            model = new HierarchyLinks();

            model.Text = "Related Topics";
            model.Url = string.Empty;

            var children = new List<HierarchyLinks>();

            Dictionary<Guid, HierarchyLinks> taxonomyItems = new Dictionary<Guid, HierarchyLinks>();

            foreach (var taxonomy in glassModel.Taxonomies)
            {
                var taxonomyTree = GetTaxonomyHierarchy(taxonomy);

                if (!taxonomyItems.ContainsKey(taxonomyTree.Item1._Id))
                {
                    taxonomyItems.Add(taxonomyTree.Item1._Id, new HierarchyLinks
                    {
                        Text = taxonomyTree.Item1._Name,
                        Url = string.Empty,
                        Children = new List<HierarchyLinks>()
                    });
                }        

                var folderItem = taxonomyItems[taxonomyTree.Item1._Id];

                foreach (var item in taxonomyTree.Item3)
                {
                    if (!taxonomyItems.ContainsKey(item._Parent._Id))
                    {
                        taxonomyItems.Add(item._Parent._Id, new HierarchyLinks
                        {
                            Text = item.Item_Name,
                            Url = SearchTaxonomyUtil.GetSearchUrl(item),
                            Children = new List<HierarchyLinks>()
                        });
                    }

                    var lItem = new HierarchyLinks
                    {
                        Text = item.Item_Name,
                        Url = SearchTaxonomyUtil.GetSearchUrl(item),
                        Children = new List<HierarchyLinks>()
                    };

                    taxonomyItems.Add(item._Id, lItem);   
                    var parent = taxonomyItems[item._Parent._Id];
                    var pList = parent.Children.ToList();
                    pList.Add(lItem);

                    parent.Children = pList; 
                }
                
                if(!children.Any(x => x.Text.Equals(folderItem.Text)))
                    children.Add(folderItem);
            }

            model.Children = children;
        }

        //private IEnumerable<IGlassBase> GetHierarchy(ITaxonomy_Item item)
        //{
        //    this.
        //} 

        private Tuple<IFolder, Guid, IEnumerable<ITaxonomy_Item>> GetTaxonomyHierarchy(ITaxonomy_Item taxonomy)
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

            if (!(parent is IFolder))
            {
                throw new InvalidCastException("Not the correct data structure");
            }
            taxonomyItems.Reverse();

            return new Tuple<IFolder, Guid, IEnumerable<ITaxonomy_Item>>(parent as IFolder, taxonomy._Parent._Id, taxonomyItems);
        }

        public IEnumerable<IHierarchyLinks> Children => model.Children;

        public string Text => model.Text;

        public string RelatedTaxonomyHeader => TextTranslator.Translate("Article.RelTaxHeader");

        public string Url => model.Url;
    }
}