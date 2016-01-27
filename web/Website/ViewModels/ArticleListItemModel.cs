using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
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
        public ISiteRootContext SiterootContext { get; set; }
        public ArticleListItemModel(IArticle innerItem, ISiteRootContext siteRootContext)
        {
            _innerItem = innerItem;
            SiterootContext = siteRootContext;
        }
       
        public IEnumerable<ILinkable> ListableAuthors => _innerItem.Authors.Select(x => new LinkableModel {LinkableText = x.First_Name + " " + x.Last_Name});


        public DateTime ListableDate => _innerItem.Actual_Publish_Date;


        public string ListableImage => _innerItem.Featured_Image_16_9?.Src;


        public string ListableSummary => _innerItem.Summary;

        public string ListableTitle => _innerItem.Title;
        public string ListableByline => Publication;

        public virtual IEnumerable<ILinkable> ListableTopics => _innerItem.Taxonomies.Select(x => new LinkableModel {LinkableText = x.Item_Name, LinkableUrl = "/Search?QueryParam"});
        public string ListableType => _innerItem.Media_Type?.Item_Name == "Data" ? "chart" : _innerItem.Media_Type?.Item_Name?.ToLower() ?? "";


        public virtual Link ListableUrl => new Link {Url = _innerItem._Url, Text = _innerItem.Title};

        #region Implementation of ILinkable

        public string LinkableText => _innerItem.Content_Type.Item_Name;
        public string LinkableUrl => _innerItem._Url;
        public string Publication => SiterootContext?.Item?.Publication_Name?.StripHtml();

        #endregion
    }
}