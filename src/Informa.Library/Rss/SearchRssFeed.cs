using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Xml.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Newtonsoft.Json;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Syndication;
using Sitecore.Syndication.Web;
using Sitecore.Web;
using Sitecore.Web.UI.WebControls;

namespace Informa.Library.Rss
{
    public class SearchRssFeed : PublicFeed
    {
        private readonly XNamespace _atomNameSpace = "http://www.w3.org/2005/Atom";
        protected ISitecoreContext _sitecoreContext;
        protected string _hostName;
        protected ItemReferences _itemReferences;

        protected override void SetupFeed(SyndicationFeed feed)
        {
               _sitecoreContext = new SitecoreContext();
            _hostName = WebUtil.GetHostName();
            _itemReferences = new ItemReferences();

            //Set some basic feed attributes
            feed.Title = new TextSyndicationContent(GetFeedTitle());
            feed.Language = "en-US";

            //Add atom namespace to the feed 
            feed.AttributeExtensions.Add(new XmlQualifiedName("atom", XNamespace.Xmlns.NamespaceName),
                _atomNameSpace.NamespaceName);

            feed = AddFeedLinks(feed);
            feed = AddCopyrightText(feed);
        }

        /// <summary>
        /// Get the RSS channel title that displays any search terms that were used in the search
        /// </summary>
        /// <returns></returns>
        private string GetFeedTitle()
        {
            var title = "Search Feed";

            if (Context.Request.QueryString["q"] != null)
            {
                title += ": " + Context.Request.QueryString["q"];
            }

            return title;
        }

        /// <summary>
        /// Add the copyright text to the rss channel, this is pulled from Sitecore
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        private SyndicationFeed AddCopyrightText(SyndicationFeed feed)
        {
            var siteConfig = _sitecoreContext.GetItem<ISite_Config>(_itemReferences.SiteConfig);
            feed.Copyright = new TextSyndicationContent(siteConfig.Copyright_Text);

            return feed;
        }

        /// <summary>
        /// Add the channel links
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        protected SyndicationFeed AddFeedLinks(SyndicationFeed feed)
        {
            feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetSearchUrl())));
            feed.ElementExtensions.Add(new XElement(_atomNameSpace + "link", new XAttribute("href", GetSearchUrl()),
                new XAttribute("rel", "self"), new XAttribute("type", "application/rss+xml")));

            return feed;
        }

        /// <summary>
        /// Render each RSS item, there are custom fields being added above and beyond the normal atom fields
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override SyndicationItem RenderItem(Item item)
        {
            var syndicationItem = base.RenderItem(item);

            var article = _sitecoreContext.GetItem<IArticle>(item.ID.ToString());

            if (article != null)
            {
                syndicationItem = AddIdToFeedItem(syndicationItem, article);
                syndicationItem = AddCatgoryToFeedItem(syndicationItem, article);
                syndicationItem = AddAuthorsToFeedItem(syndicationItem, article);
                syndicationItem = AddTaxonomyToFeedItem(syndicationItem, article);
                syndicationItem = AddMediaTypeToFeedItem(syndicationItem, article);
                syndicationItem = AddEmailSortOrderField(syndicationItem, article);

                var titleText = HttpUtility.HtmlEncode(syndicationItem.Title.Text);

                if (article.Media_Type != null)
                {
                    titleText = article.Media_Type.Item_Name.ToUpper() + ": " + titleText;
                }

                syndicationItem.Title = new TextSyndicationContent(StripHtmlTags(titleText));

                var content = syndicationItem.Content as TextSyndicationContent;
                var descriptonText = HttpUtility.HtmlEncode(content.Text);
                syndicationItem.Content = new TextSyndicationContent(descriptonText);
            }


            return syndicationItem;
        }

        private SyndicationItem AddEmailSortOrderField(SyndicationItem syndicationItem, IArticle article)
        {
            var emailPriorityElement = new XElement("e-mail_priority");
            if (article.Sort_Order > 0)
            {
                emailPriorityElement.Value = article.Sort_Order.ToString();
            }
            else
            {
                emailPriorityElement.Value = "0";
            }

            syndicationItem.ElementExtensions.Add(emailPriorityElement.CreateReader());

            return syndicationItem;
        }

        /// <summary>
        /// Add the custom media type field to a rendered item
        /// </summary>
        /// <param name="syndicationItem"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        private SyndicationItem AddMediaTypeToFeedItem(SyndicationItem syndicationItem, IArticle article)
        {
            if (article.Media_Type != null)
            {
                var mediaElement = new XElement("media_icon");
                mediaElement.Value = article.Media_Type.Item_Name;
                syndicationItem.ElementExtensions.Add(mediaElement.CreateReader());
            }

            return syndicationItem;
        }

        /// <summary>
        /// Add the custom taxonomy items to a rendered item
        /// </summary>
        /// <param name="syndicationItem"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        private SyndicationItem AddTaxonomyToFeedItem(SyndicationItem syndicationItem, IArticle article)
        {
            if (article.Taxonomies != null)
            {
                if (article.Taxonomies.Any())
                {
                    var taxonomyElement = new XElement("taxonomy_items");

                    foreach (var taxonomyItem in article.Taxonomies)
                    {
                        var taxonomyItemElement = new XElement("taxonomy_item");

                        taxonomyItemElement.Value = taxonomyItem.Item_Name;

                        taxonomyElement.Add(taxonomyItemElement);
                    }

                    syndicationItem.ElementExtensions.Add(taxonomyElement.CreateReader());
                }
            }

            return syndicationItem;
        }

        /// <summary>
        /// Add any authors to the rendered item
        /// </summary>
        /// <param name="syndicationItem"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        private SyndicationItem AddAuthorsToFeedItem(SyndicationItem syndicationItem, IArticle article)
        {
            if (article.Authors != null)
            {
                foreach (var author in article.Authors)
                {
                    var authorName = author.First_Name + " " + author.Last_Name;
                    if (string.IsNullOrEmpty(authorName))
                    {
                        authorName = author._Name;
                    }

                    syndicationItem.Authors.Add(new SyndicationPerson(author.Email_Address, authorName, ""));
                }
            }
            return syndicationItem;
        }

        /// <summary>
        /// Add the content type to the rendered item
        /// </summary>
        /// <param name="syndicationItem"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        private SyndicationItem AddCatgoryToFeedItem(SyndicationItem syndicationItem, IArticle article)
        {
            if (article.Content_Type != null)
            {
                syndicationItem.Categories.Add(new SyndicationCategory(article.Content_Type.Item_Name));
            }

            return syndicationItem;
        }

        /// <summary>
        /// ADd the ariticle number to the rendered item
        /// </summary>
        /// <param name="syndicationItem"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        private SyndicationItem AddIdToFeedItem(SyndicationItem syndicationItem, IArticle article)
        {
            if (!string.IsNullOrEmpty(article.Article_Number))
            {
                var newElement = new XElement("id");
                newElement.Value = article.Article_Number;
                syndicationItem.ElementExtensions.Add(newElement.CreateReader());
            }

            return syndicationItem;
        }

        public static string StripHtmlTags(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";

            var decodedHtml = HttpUtility.HtmlDecode(html);

            return Regex.Replace(decodedHtml, "<.*?>", string.Empty);
        }

        /// <summary>
        ///     Get the url for the original search page
        /// </summary>
        /// <returns></returns>
        private string GetSearchUrl()
        {

            var searchUrlBase = string.Format("http://{0}/search#?", _hostName);

            if (!Context.RawUrl.Contains("?"))
            {
                return searchUrlBase;
            }

            var urlParts = Context.RawUrl.Split('?');

            if (urlParts.Length != 2)
            {
                return searchUrlBase;
            }

            return HttpUtility.HtmlEncode(string.Format("{0}{1}", searchUrlBase, urlParts[1]));
        }

        public override IEnumerable<Item> GetSourceItems()
        {
            string searchPageId = _itemReferences.SearchPage.ToString().ToLower().Replace("{", "").Replace("}", "");
            string url = string.Format("http://{0}/api/informasearch?pId={1}", _hostName, searchPageId);

            if (Context.RawUrl.Contains("?"))
            {
                var urlParts = Context.RawUrl.Split('?');

                if (urlParts.Length == 2)
                {
                    url = string.Format("{0}&{1}", url, urlParts[1]);
                }
            }
            else
            {
                url = string.Format("{0}&sortBy=relevance&sortOrder=asc", url);
            }

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