using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Rss.Interfaces;
using Informa.Library.Rss.Utils;
using Informa.Library.Search.Utilities;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;

namespace Informa.Library.Rss.ItemGenerators
{
    public class SearchRssItemGenerator : BaseRssItemGenerator, IRssItemGeneration
    {
        public SyndicationItem GetSyndicationItemFromSitecore(ISitecoreContext sitecoreContext, Item item)
        {
            var article = sitecoreContext.GetItem<IArticle>(item.ID.ToString());

            if (article == null)
            {
                return null;
            }

            //Build the basic syndicaton item
            var searchTerm = Sitecore.Context.Request.QueryString["q"];
            var articleUrl = string.Format("{0}?utm_source=search&amp;utm_medium=RSS&amp;utm_term={1}&amp;utm_campaign=search_rss", article._AbsoluteUrl, searchTerm);
            var syndicationItem = new SyndicationItem(GetItemTitle(article),
                GetItemSummary(article),
                new Uri(articleUrl),
                article._AbsoluteUrl,
                article.Actual_Publish_Date);

            //Add the custom fields
            syndicationItem = AddIdToFeedItem(syndicationItem, article);
            syndicationItem = AddPubDateToFeedItem(syndicationItem, article);
            syndicationItem = AddCatgoryToFeedItem(syndicationItem, article);
            syndicationItem = AddAuthorsToFeedItem(syndicationItem, article);
            syndicationItem = AddTaxonomyToFeedItem(syndicationItem, article);
            syndicationItem = AddMediaTypeToFeedItem(syndicationItem, article);
            syndicationItem = AddEmailSortOrderField(syndicationItem, article);

            var content = syndicationItem.Content as TextSyndicationContent;
            var descriptonText = HttpUtility.HtmlEncode(content.Text);
            syndicationItem.Content = new TextSyndicationContent(descriptonText);

            return syndicationItem;
        }

        /// <summary>
        ///     Add the content type to the rendered item
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
        ///     ADd the ariticle number to the rendered item
        /// </summary>
        /// <param name="syndicationItem"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        private SyndicationItem AddIdToFeedItem(SyndicationItem syndicationItem, IArticle article)
        {
            if (!string.IsNullOrEmpty(article.Article_Number))
            {
                var newElement = new XElement(RssConstants.InformaNamespace + RssConstants.FieldId);
                newElement.Value = article.Article_Number;
                syndicationItem.ElementExtensions.Add(newElement.CreateReader());
            }

            return syndicationItem;
        }

        public string StripHtmlTags(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";

            var decodedHtml = HttpUtility.HtmlDecode(html);

            return Regex.Replace(decodedHtml, "<.*?>", string.Empty);
        }


        private SyndicationItem AddEmailSortOrderField(SyndicationItem syndicationItem, IArticle article)
        {
            var emailPriorityElement = new XElement(RssConstants.InformaNamespace + RssConstants.FieldEmailPriority);
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
        ///     Add the custom media type field to a rendered item
        /// </summary>
        /// <param name="syndicationItem"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        private SyndicationItem AddMediaTypeToFeedItem(SyndicationItem syndicationItem, IArticle article)
        {
            if (article.Media_Type != null)
            {
                var mediaElement = new XElement(RssConstants.InformaNamespace + RssConstants.FieldMediaIcon);
                if (!string.IsNullOrEmpty(article.Media_Type.Item_Name))
                {
                    mediaElement.Value = HttpUtility.HtmlEncode(article.Media_Type.Item_Name);
                    syndicationItem.ElementExtensions.Add(mediaElement.CreateReader());
                }
            }

            return syndicationItem;
        }

        /// <summary>
        ///     Add the custom taxonomy items to a rendered item
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
                    var taxonomyElement = new XElement(RssConstants.InformaNamespace + RssConstants.FieldTaxonomyItems);

                    foreach (var taxonomyItem in article.Taxonomies)
                    {
                        var taxonomyItemElement = new XElement(RssConstants.InformaNamespace + RssConstants.FieldTaxonomyItem);

                        taxonomyItemElement.Value = HttpUtility.HtmlEncode(taxonomyItem.Item_Name);

                        taxonomyElement.Add(taxonomyItemElement);
                    }

                    syndicationItem.ElementExtensions.Add(taxonomyElement.CreateReader());
                }
            }

            return syndicationItem;
        }
    }
}