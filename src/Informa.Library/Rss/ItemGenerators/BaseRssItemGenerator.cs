using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Informa.Library.Rss.Utils;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Rss.ItemGenerators
{
    public class BaseRssItemGenerator
    {
        public string GetItemTitle(IArticle article)
        {
            //Build the title string, if there is a media type that is prepended onto the title
            var titleText = HttpUtility.HtmlEncode(article.Title);

            if (article.Media_Type != null)
            {
                if (!string.IsNullOrEmpty(article.Media_Type.Item_Name))
                {
                    titleText = article.Media_Type.Item_Name.ToUpper() + ": " + titleText;
                }
            }

            return titleText;
        }
        public SyndicationItem AddPubDateToFeedItem(SyndicationItem syndicationItem, IArticle article)
        {
            if (!string.IsNullOrEmpty(article.Article_Number))
            {
                var newElement = new XElement(RssConstants.FieldPubDate);
                newElement.Value = article.Actual_Publish_Date.ToString(RssConstants.DateFormat);
                syndicationItem.ElementExtensions.Add(newElement.CreateReader());
            }

            return syndicationItem;
        }

    }
}
