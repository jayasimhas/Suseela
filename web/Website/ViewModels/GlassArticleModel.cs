using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.Common;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{  
    public class GlassArticleModel : GlassViewModel<IArticle>, IArticleModel
    {   
        public IEnumerable<ILinkable> TaxonomyItems
            => GlassModel.Taxonomies.Select(x => new LinkableModel {LinkableText = x.Item_Name, LinkableUrl = "/search?tag=" + x._Id});

        #region Implementation of IArticleModel

        public string Title => GlassModel.Title;
        public string Sub_Title => GlassModel.Sub_Title;
        public string Body => GlassModel.Body;
        public IHierarchyLinks TaxonomyHierarchy => new HierarchyLinksViewModel(GlassModel);
        public DateTime Date => GlassModel.Actual_Publish_Date;
        //TODO: Extract to a dictionary.
        public string Content_Type => GlassModel.Content_Type?.Item_Name;
        public string Media_Type => GlassModel.Media_Type?.Item_Name == "Data" ? "chart" : GlassModel.Media_Type?.Item_Name?.ToLower() ?? "";

        public IEnumerable<IPersonModel> Authors => GlassModel.Authors.Select(x => new PersonModel(x));
        public string Category => GlassModel.Article_Category;
        public IEnumerable<IListable> RelatedArticles => GlassModel.Related_Articles.Select(x => new ArticleListItemModel(x));
        public IFeaturedImage Image => new ArticleFeaturedImage(GlassModel);

        #endregion

        #region Implementation of ILinkable

        public string LinkableText => Title;
        public string LinkableUrl => GlassModel._Url;

        #endregion

        #region Implementation of IListable

        public IEnumerable<ILinkable> ListableAuthors
            =>
                Authors.Select(
                    x => new LinkableModel {LinkableText = x.Name, LinkableUrl = $"mailto://{x.Email_Address}"});

        public DateTime ListableDate => Date;
        public string ListableImage => Image.ImageUrl;
        //TODO: Get real summary
        public string ListableSummary
            => string.IsNullOrWhiteSpace(GlassModel.Summary) ? Body.Substring(0, 200) : GlassModel.Summary;

        public string ListableTitle => Title;

        public IEnumerable<ILinkable> ListableTopics
            =>
                GlassModel.Taxonomies?.Take(3)
                    .Select(x => new LinkableModel {LinkableText = x.Item_Name, LinkableUrl = "/search?tag=" + x._Id});

        public string ListableType => Media_Type;

        #endregion
    }

    public class HierarchyLinksViewModel : GlassViewModel<I___BaseTaxonomy>, IHierarchyLinks
    {
        private HierarchyLinks model;
        public HierarchyLinksViewModel(I___BaseTaxonomy glassModel)
        {
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
                            Url = "/search?tag=" + item._Id,
                            Children = new List<HierarchyLinks>()
                        });
                    }

                    var lItem = new HierarchyLinks
                    {
                        Text = item.Item_Name,
                        Url = "/search?tag=" + item._Id,
                        Children = new List<HierarchyLinks>()
                    };

                    taxonomyItems.Add(item._Id, lItem);   
                    var parent = taxonomyItems[item._Parent._Id];
                    var pList = parent.Children.ToList();
                    pList.Add(lItem);

                    parent.Children = pList; 
                }

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


        public string Url => model.Url;
    }

    public class ArticleFeaturedImage : IFeaturedImage
    {
        private IArticle _glassModel;
        public ArticleFeaturedImage(IArticle glassModel)
        {
            _glassModel = glassModel;
        }

        public string ImageUrl => _glassModel.Featured_Image_16_9.Src;
        public string ImageCaption => _glassModel.Featured_Image_Caption;
        public string ImageSource => _glassModel.Featured_Image_Source;
    }
    
}