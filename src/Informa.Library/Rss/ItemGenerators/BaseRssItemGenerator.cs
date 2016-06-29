using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Informa.Library.Rss.Utils;
using Informa.Library.Search.Utilities;
using Informa.Library.Utilities.StringUtils;
using Informa.Library.Utilities.TokenMatcher;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Rss.ItemGenerators
{
	public class BaseRssItemGenerator
	{
		public string GetItemTitle(IArticle article)
		{
			//Build the title string, if there is a media type that is prepended onto the title
			//We need to decode it before encoding it again because strings like &amp; may
			//be in the original title and if we encode that it comes out as &amp;amp;
			var titleText = HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(HtmlUtil.StripHtml(article.Title)));

			if (article.Media_Type != null)
			{
				if (!string.IsNullOrEmpty(article.Media_Type.Item_Name))
				{
					titleText = article.Media_Type.Item_Name.ToUpper() + ": " + titleText;
				}
			}

			return titleText;
		}

		/// <summary>
		///     Add any authors to the rendered item
		/// </summary>
		/// <param name="syndicationItem"></param>
		/// <param name="article"></param>
		/// <returns></returns>
		public SyndicationItem AddAuthorsToFeedItem(SyndicationItem syndicationItem, IArticle article)
		{
			if (article.Authors != null)
			{
				if (article.Authors.Any())
				{
					foreach (var authorItem in article.Authors)
					{
						if(authorItem == null) continue;

						var authorElement = new XElement(RssConstants.AtomNamespace + "author");

						var authorName = authorItem.First_Name + " " + authorItem.Last_Name;
						if (string.IsNullOrWhiteSpace(authorName))
						{
							authorName = authorItem._Name;
						}

						var authorElementName = new XElement(RssConstants.AtomNamespace + "name");
						authorElementName.Value = HttpUtility.HtmlEncode(authorName);

						var authorElementEmail = new XElement(RssConstants.AtomNamespace + "email");
						authorElementEmail.Value = HttpUtility.HtmlEncode(authorItem.Email_Address ?? string.Empty);

						authorElement.Add(authorElementName);
						authorElement.Add(authorElementEmail);

						syndicationItem.ElementExtensions.Add(authorElement.CreateReader());
					}
				}
			}

			return syndicationItem;
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

		public string GetItemSummary(IArticle article)
		{
			string summary = HttpUtility.HtmlDecode(article.Summary);
			summary = SearchSummaryUtil.GetTruncatedSearchSummary(article.Summary);
			return HttpUtility.HtmlDecode(summary);
		}
	}
}
