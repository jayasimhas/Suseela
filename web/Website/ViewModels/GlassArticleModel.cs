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
        public string Content_Type => GlassModel.Content_Type.Item_Name;
        public string Media_Type => GlassModel.Media_Type.Item_Name;

        public IEnumerable<IAuthorModel> Authors => GlassModel.Authors.Select(x => new AuthorModel(x));
        public string Category => GlassModel.Article_Category;

        #endregion
    }
}