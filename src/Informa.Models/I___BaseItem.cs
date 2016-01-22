using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Factory.Attributes;

namespace Informa.Models.FactoryInterface
{
    [GlassFactoryInterface]
    public interface ILinkable
    {
        string LinkableText { get; }
        string LinkableUrl { get; }
    }

    public interface IListable : ILinkable
    {                    
        IEnumerable<ILinkable> ListableAuthors { get; }  
        DateTime ListableDate { get; }                 
        string ListableImage { get; }                    
        string ListableSummary { get; }                
        string ListableTitle { get; }                   
        IEnumerable<ILinkable> ListableTopics { get; }   
    }

    public interface IFeaturedImage
    {
        string ImageUrl { get; }
        string ImageCaption { get; }
        string ImageSource { get; }
    }

    public class FeaturedImage : IFeaturedImage
    {
        #region Implementation of IFeaturedImage

        public string ImageUrl { get; set; }
        public string ImageCaption { get; set; }
        public string ImageSource { get; set; }

        #endregion
    }


    public class ListableModel : IListable
    {
        #region Implementation of ILinkable

        public string LinkableText { get; set; }
        public string LinkableUrl { get; set; }

        #endregion

        #region Implementation of IListable

        public IEnumerable<ILinkable> ListableAuthors { get; set; }
        public DateTime ListableDate { get; set; }
        public string ListableImage { get; set; }
        public string ListableSummary { get; set; }
        public string ListableTitle { get; set; }
        public IEnumerable<ILinkable> ListableTopics { get; set; }
        public Link ListableUrl { get; set; }

        #endregion
    }

    public class LinkableModel : ILinkable
    {
        #region Implementation of ILinkable

        public string LinkableText { get; set; }
        public string LinkableUrl { get; set; }

        #endregion
    }
}