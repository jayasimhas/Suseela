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
using Informa.Library.Rss.Utils;
using Informa.Library.Search.Utilities;
using Informa.Library.Utilities.TokenMatcher;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;

namespace Informa.Library.Rss.ItemGenerators
{
    public class PublicationRssItemGenerator : BaseRssItemGenerator, IRssItemGeneration
    {
        public SyndicationItem GetSyndicationItemFromSitecore(ISitecoreContext sitecoreContext, Item item)
        {
            var article = sitecoreContext.GetItem<IArticle>(item.ID.ToString());
            string publicationName = "None";
            if (article == null)
            {
                return null;
            }

            if (article.Publication != null)
            {
                var publication = sitecoreContext.GetItem<Item>(article.Publication);
                publicationName = publication?.Name;
            }
            var articleUrl = string.Format("{0}?utm_source={1}&amp;utm_medium=RSS&amp;utm_campaign={2}_RSS_Feed", article._AbsoluteUrl, publicationName, publicationName);
            //Build the basic syndicaton item
            var syndicationItem = new SyndicationItem(GetItemTitle(article),
                GetItemSummary(article),
                new Uri(articleUrl),
                article._AbsoluteUrl,
                article.Actual_Publish_Date);

            string siteLink = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            syndicationItem = AddImageToFeedItem(syndicationItem, article, siteLink);
            syndicationItem = AddAuthorsToFeedItem(syndicationItem, article);
            syndicationItem = AddPubDateToFeedItem(syndicationItem, article);

            var content = syndicationItem.Content as TextSyndicationContent;
            var descriptonText = HttpUtility.HtmlEncode(content.Text);
            syndicationItem.Content = new TextSyndicationContent(descriptonText);

            return syndicationItem;
        }

        private SyndicationItem AddImageToFeedItem(SyndicationItem syndicationItem, IArticle article, string siteLink)
        {
            if (article.Featured_Image_16_9 != null)
            {
                var imageElement = new XElement(RssConstants.InformaNamespace + RssConstants.FieldImage);

                var urlElement = new XElement(RssConstants.InformaNamespace + RssConstants.FieldImageUrl);
                urlElement.Value = siteLink + article.Featured_Image_16_9.Src;
                imageElement.Add(urlElement);

                var titleElement = new XElement(RssConstants.InformaNamespace + RssConstants.FieldImageTitle);
                titleElement.Value = article.Featured_Image_16_9.Alt;
                imageElement.Add(titleElement);

                var linkElement = new XElement(RssConstants.InformaNamespace + RssConstants.FieldImageLink);
                linkElement.Value = siteLink;
                imageElement.Add(linkElement);

                syndicationItem.ElementExtensions.Add(imageElement.CreateReader());
            }

            return syndicationItem;
        }
    }
}
