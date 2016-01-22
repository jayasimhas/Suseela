using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;

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
        //IEnumerable<HierarchyLinks> IArticleModel.TaxonomyItems { get; }
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