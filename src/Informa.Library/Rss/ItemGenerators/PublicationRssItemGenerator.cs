using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Rss.Interfaces;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;

namespace Informa.Library.Rss.ItemGenerators
{
    public class PublicationRssItemGenerator : BaseRssItemGenerator, IRssItemGeneration
    {
        public SyndicationItem GetSyndicationItemFromSitecore(ISitecoreContext sitecoreContext, Item item)
        {
            var article = sitecoreContext.GetItem<IArticle>(item.ID.ToString());

            if (article == null)
            {
                return null;
            }

            //Build the basic syndicaton item
            var syndicationItem = new SyndicationItem(GetItemTitle(article),
                article.Summary,
                new Uri(article._AbsoluteUrl),
                article._AbsoluteUrl,
                article.Actual_Publish_Date);

            string siteLink  = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            syndicationItem = AddImageToFeedItem(syndicationItem, article, siteLink);
            syndicationItem = AddPubDateToFeedItem(syndicationItem, article);

            return syndicationItem;
        }


        private SyndicationItem AddImageToFeedItem(SyndicationItem syndicationItem, IArticle article,string siteLink)
        {
            if (article.Featured_Image_16_9 != null)
            {
                var imageElement = new XElement("image");

                var urlElement = new XElement("url");
                urlElement.Value = siteLink + article.Featured_Image_16_9.Src;
                imageElement.Add(urlElement);

                var titleElement = new XElement("title");
                titleElement.Value = article.Featured_Image_16_9.Alt;
                imageElement.Add(titleElement);

                var linkElement = new XElement("link");
                linkElement.Value = siteLink;
                imageElement.Add(linkElement);

                syndicationItem.ElementExtensions.Add(imageElement.CreateReader());
            }

            return syndicationItem;
        }
    }
}
