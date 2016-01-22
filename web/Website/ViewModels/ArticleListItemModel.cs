using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Velir.Search.Models.FactoryInterface;
using Informa.Web.Models;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Items;

namespace Informa.Web.ViewModels
{
    public class ArticleListItemModel : GlassViewModel<IArticle>, IListable
    {
        private IArticle _innerItem;
        public ArticleListItemModel(IArticle innerItem)
        {
            _innerItem = innerItem;
        }

        public IEnumerable<ILinkable> ListableAuthors => GlassModel.Authors.Select(x => new LinkableModel {LinkableText = x.First_Name + " " + x.Last_Name});


        public DateTime ListableDate => GlassModel.Actual_Publish_Date;


        public string ListableImage => _innerItem.Featured_Image_16_9?.Src;


        public string ListableSummary => GlassModel.Summary;

        public string ListableTitle => GlassModel.Title;
        
        public virtual IEnumerable<ILinkable> ListableTopics => GlassModel.Taxonomies.Select(x => new LinkableModel {LinkableText = x.Item_Name, LinkableUrl = "/Search?QueryParam"});


        public virtual Link ListableUrl => new Link {Url = GlassModel._Url, Text = GlassModel.Title};

        #region Implementation of ILinkable

        public string LinkableText => GlassModel.Content_Type.Item_Name;
        public string LinkableUrl => GlassModel._Url;

        #endregion
    }
}