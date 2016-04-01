﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;

namespace Informa.Library.Rss.Utils
{
	public class RssItemUtil
	{
		protected ISitecoreContext _sitecoreContext;

		public RssItemUtil(ISitecoreContext sitecoreContext)
		{
			_sitecoreContext = sitecoreContext;

		}

		public SyndicationItem GetSyndicationItemFromSitecore(Item item)
		{
			var article = _sitecoreContext.GetItem<IArticle>(item.ID.ToString());
			var publication = _sitecoreContext.GetItem<Item>(article.Publication);
			var searchTerm = Sitecore.Context.Request.QueryString["q"];
			if (article == null)
			{
				return null;
			}
			var articleUrl = (!string.IsNullOrEmpty(searchTerm)) ? string.Format("{0}?utm_source=search&utm_medium=RSS&utm_term={1}&utm_campaign=search_rss", article._AbsoluteUrl, searchTerm) : string.Format("{0}?utm_source={1}&utm_medium=RSS&utm_campaign={2}_RSS_Feed", article._AbsoluteUrl, publication.Name, publication.Name);
			var syndicationItem = new SyndicationItem(article.Title,
		article.Summary,
		new Uri(articleUrl),
		article._AbsoluteUrl,
		article.Actual_Publish_Date);

			syndicationItem = AddIdToFeedItem(syndicationItem, article);
			syndicationItem = AddCatgoryToFeedItem(syndicationItem, article);
			syndicationItem = AddAuthorsToFeedItem(syndicationItem, article);
			syndicationItem = AddTaxonomyToFeedItem(syndicationItem, article);
			syndicationItem = AddMediaTypeToFeedItem(syndicationItem, article);
			syndicationItem = AddEmailSortOrderField(syndicationItem, article);

			var titleText = HttpUtility.HtmlEncode(syndicationItem.Title);

			if (article.Media_Type != null)
			{
				if (!string.IsNullOrEmpty(article.Media_Type.Item_Name))
				{
					titleText = article.Media_Type.Item_Name.ToUpper() + ": " + titleText;
				}


			}

			syndicationItem.Title = new TextSyndicationContent(StripHtmlTags(titleText));

			var content = syndicationItem.Content as TextSyndicationContent;
			var descriptonText = HttpUtility.HtmlEncode(content.Text);
			syndicationItem.Content = new TextSyndicationContent(descriptonText);

			return syndicationItem;
		}

		public static string StripHtmlTags(string html)
		{
			if (string.IsNullOrEmpty(html)) return "";

			var decodedHtml = HttpUtility.HtmlDecode(html);

			return Regex.Replace(decodedHtml, "<.*?>", string.Empty);
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
				if (!string.IsNullOrEmpty(article.Media_Type.Item_Name))
				{
					mediaElement.Value = article.Media_Type.Item_Name;
					syndicationItem.ElementExtensions.Add(mediaElement.CreateReader());
				}
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
	}
}
