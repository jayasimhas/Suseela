using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Search.Results;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Pipelines;
using Sitecore.Pipelines.RenderField;
using Sitecore.Rules;
using Sitecore.Web;

namespace Informa.Library.Rss
{

    public class SearchResultsRequest
    {
        //   "pageId":"0ff66777-7ec7-40be-abc4-6a20c8ed1ef0",
        //"page":1,
        //"perPage":10,
        //"sortBy":"relevance",
        //"sortOrder":"asc",
        //"queryParameters":{      

        public string page { get; set; }
        public string pageId { get; set; }
        public string perPage { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        //  public List<string> queryParameters { get; set; }

    }

    public class SearchResults
    {
        public SearchResultsRequest request { get; set; }
        public string totalResults { get; set; }
        public List<InformaSearchResultItem> results { get; set; }
    }

    public class SearchRssFeed : Sitecore.Syndication.PublicFeed
    {
        protected ISitecoreContext _sitecoreContext;

        readonly XNamespace _atomNameSpace = "http://www.w3.org/2005/Atom";

        protected override void SetupFeed(SyndicationFeed feed)
        {
            _sitecoreContext = new SitecoreContext();

            //Set some basic feed attributes
            feed.Title = new TextSyndicationContent(GetFeedTitle());
            feed.Language = "en-US";

            //Add atom namespace to the feed 
            feed.AttributeExtensions.Add(new XmlQualifiedName("atom", XNamespace.Xmlns.NamespaceName), _atomNameSpace.NamespaceName);

            feed = AddFeedLinks(feed);
            feed = AddCopyrightText(feed);
        }

        private string GetFeedTitle()
        {
            string title = "Search Feed";

            if (Sitecore.Context.Request.QueryString["q"] != null)
            {
                title += ": " + Sitecore.Context.Request.QueryString["q"];
            }

            return title;
        }

        private SyndicationFeed AddCopyrightText(SyndicationFeed feed)
        {
            ItemReferences itemReferences = new ItemReferences();

            var siteConfig = _sitecoreContext.GetItem<ISite_Config>(itemReferences.SiteConfig);
            feed.Copyright = new TextSyndicationContent(siteConfig.Copyright_Text);

            return feed;
        }

        protected SyndicationFeed AddFeedLinks(SyndicationFeed feed)
        {
            feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetSearchUrl())));
            feed.ElementExtensions.Add(new XElement(_atomNameSpace + "link", new XAttribute("href", GetSearchUrl()), new XAttribute("rel", "self"), new XAttribute("type", "application/rss+xml")));

            return feed;
        }

        protected override SyndicationItem RenderItem(Item item)
        {
            SyndicationItem syndicationItem = base.RenderItem(item);

            var article = _sitecoreContext.GetItem<IArticle>(item.ID.ToString());

            if (article != null)
            {
                syndicationItem = AddIdToFeedItem(syndicationItem, article);
                syndicationItem = AddCatgoryToFeedItem(syndicationItem, article);
                syndicationItem = AddAuthorsToFeedItem(syndicationItem, article);
                syndicationItem = AddTaxonomyToFeedItem(syndicationItem, article);
                syndicationItem = AddMediaTypeToFeedItem(syndicationItem, article);

                string titleText = HttpUtility.HtmlEncode(syndicationItem.Title.Text);

                if (article.Media_Type != null)
                {
                    titleText = article.Media_Type.Item_Name.ToUpper() + ": " + titleText;
                }

                syndicationItem.Title = new TextSyndicationContent(StripHtmlTags(titleText));

                TextSyndicationContent content = syndicationItem.Content as TextSyndicationContent;
                var descriptonText = HttpUtility.HtmlEncode(content.Text);
                syndicationItem.Content = new TextSyndicationContent(descriptonText);
            }



            return syndicationItem;
        }

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

        private SyndicationItem AddAuthorsToFeedItem(SyndicationItem syndicationItem, IArticle article)
        {
            if (article.Authors != null)
            {
                foreach (IAuthor author in article.Authors)
                {
                    string authorName = author.First_Name + " " + author.Last_Name;
                    if (string.IsNullOrEmpty(authorName))
                    {
                        authorName = author._Name;
                    }

                    syndicationItem.Authors.Add(new SyndicationPerson(author.Email_Address, authorName, ""));
                }
            }
            return syndicationItem;
        }

        private SyndicationItem AddCatgoryToFeedItem(SyndicationItem syndicationItem, IArticle article)
        {
            if (article.Content_Type != null)
            {
                syndicationItem.Categories.Add(new SyndicationCategory(article.Content_Type.Item_Name));
            }

            return syndicationItem;
        }

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
            if(String.IsNullOrEmpty(html)) return "";

               string decodedHtml = HttpUtility.HtmlDecode(html);

            return Regex.Replace(decodedHtml, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Get the url for the original search page
        /// </summary>
        /// <returns></returns>
        private string GetSearchUrl()
        {
            string hostName = WebUtil.GetHostName();

            string searchUrlBase = string.Format("http://{0}/search#?", hostName);

            if (!Sitecore.Context.RawUrl.Contains("?"))
            {
                return searchUrlBase;
            }

            string[] urlParts = Sitecore.Context.RawUrl.Split('?');

            if (urlParts.Length != 2)
            {
                return searchUrlBase;
            }

            return HttpUtility.HtmlEncode(string.Format("{0}{1}", searchUrlBase, urlParts[1]));
        }

        public override IEnumerable<Item> GetSourceItems()
        {
            if (!Sitecore.Context.RawUrl.Contains("?"))
            {
                return new List<Item>();
            }

            string[] urlParts = Sitecore.Context.RawUrl.Split('?');

            if (urlParts.Length != 2)
            {
                return new List<Item>();
            }

            string url =
                "http://informa.gabe.dev/api/informasearch?pId=0ff66777-7ec7-40be-abc4-6a20c8ed1ef0&" + urlParts[1];

            var client = new WebClient();
            var content = client.DownloadString(url);

            SearchResults results = JsonConvert.DeserializeObject<SearchResults>(content);

            List<Item> resultItems = new List<Item>();

            foreach (var searchResult in results.results)
            {
                Item theItem = Sitecore.Context.Database.GetItem(searchResult.ItemId);

                if (theItem == null)
                {
                    continue;
                }

                resultItems.Add(theItem);
            }

            return resultItems;
        }

        //private async Task<string> GetResults()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://localhost:9000/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        // New code:
        //        HttpResponseMessage response = await client.GetAsync("api/products/1");
        //        //if (response.IsSuccessStatusCode)
        //        //{
        //        //    Product product = await response.Content.ReadAsAsync > Product > ();
        //        //    Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
        //        //}
        //    }

        //    return string.Empty;
        //}
    }
}
