using System;
using System.Collections.Generic;
using Informa.Models.FactoryInterface;
using Sitecore.Resources.Media;

namespace Informa.Web.ViewModels.PDF
{
    /// <summary>
    /// Pdf Property
    /// </summary>
    public class PersonalizedPdfViewModel
    {

        public string Body { get; set; }
        public string Summary { get; set; }
        public DateTime PublishDate { get; set; }
        public IEnumerable<LinkableModel> Texonomies { get; set; }
        public string Title { get; set; }
        public IEnumerable<IPersonModel> Author { get; set; }
        public string abslouteUrl { get; set; }
        public string ContentType { get; set; }
        public string Sub_Title { get; set; }
        public string ImageUrl { get; set; } 
        public string ImageCaption { get; set; } 
        public string ImageSource { get; set; } 
        public string ImageAltText { get; set; }
        public string GetScaledImage(int width)
        {
            return HashingUtils.ProtectAssetUrl($"{ImageUrl}?w={width}");
        }
        public IEnumerable<IListable> RelatedArticles { get; set;}
        public string ExecutiveSummary { get; set; }
    }
}
