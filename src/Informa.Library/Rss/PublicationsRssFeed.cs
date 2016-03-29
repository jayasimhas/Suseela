using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Newtonsoft.Json;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Syndication;
using Sitecore.Web;

namespace Informa.Library.Rss
{
    public class PublicationsRssFeed : PublicFeed
    {
        protected ISitecoreContext _sitecoreContext;
        private string _siteLink;
        protected string _hostName;
        protected ItemReferences _itemReferences;


        protected override void SetupFeed(SyndicationFeed feed)
        {
            base.SetupFeed(feed);

            _sitecoreContext = new SitecoreContext();
            _itemReferences = new ItemReferences();
            _hostName = WebUtil.GetHostName();
            _siteLink = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            feed.Language = base.FeedItem["Language"];

            feed.ElementExtensions.Add(new XElement("webMaster", base.FeedItem["Web Master"]));
        }

        protected override SyndicationItem RenderItem(Item item)
        {
            var syndicationItem = base.RenderItem(item);

            var article = _sitecoreContext.GetItem<IArticle>(item.ID.ToString());

            if (article != null)
            {
                syndicationItem = AddImageToFeedItem(syndicationItem, article);

                syndicationItem.Id = article._AbsoluteUrl;

                var content = syndicationItem.Content as TextSyndicationContent;
                var descriptonText = HttpUtility.HtmlEncode(content.Text);
                syndicationItem.Content = new TextSyndicationContent(descriptonText);

                var titleText = syndicationItem.Title.Text;

                if (article.Media_Type != null)
                {
                    if(!string.IsNullOrEmpty(article.Media_Type.Item_Name))
                    {
                        titleText = article.Media_Type.Item_Name.ToUpper() + ": " + titleText;
                    }
                }

                syndicationItem.Title = new TextSyndicationContent(titleText);

            }

            return syndicationItem;
        }

        private SyndicationItem AddImageToFeedItem(SyndicationItem syndicationItem, IArticle article)
        {
            if (article.Featured_Image_16_9 != null)
            {
                var imageElement = new XElement("image");

                var urlElement = new XElement("url");
                urlElement.Value = _siteLink + article.Featured_Image_16_9.Src;
                imageElement.Add(urlElement);

                var titleElement = new XElement("title");
                titleElement.Value = article.Featured_Image_16_9.Alt;
                imageElement.Add(titleElement);

                var linkElement = new XElement("link");
                linkElement.Value = _siteLink;
                imageElement.Add(linkElement);

                syndicationItem.ElementExtensions.Add(imageElement.CreateReader());
            }

            return syndicationItem;
        }

        public override IEnumerable<Item> GetSourceItems()
        {
            string searchPageId = _itemReferences.PublicatonsRssSearchPage.ToString().ToLower().Replace("{", "").Replace("}", "");
            string url = string.Format("{0}://{1}/api/informasearch?pId={2}&sortBy=date&sortOrder=desc", HttpContext.Current.Request.Url.Scheme, _hostName, searchPageId);

            var client = new WebClient();
            var content = client.DownloadString(url);

            var results = JsonConvert.DeserializeObject<SearchResults>(content);

            var resultItems = new List<Item>();

            foreach (var searchResult in results.results)
            {
                var theItem = Context.Database.GetItem(searchResult.ItemId);

                if (theItem == null)
                {
                    continue;
                }

                resultItems.Add(theItem);
            }

            return resultItems;
        }
    }
}