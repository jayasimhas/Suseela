﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Rss.Interfaces;
using Informa.Library.Search.Utilities;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Rss;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Web;
using Velir.Search.Models;
using Log = Sitecore.Diagnostics.Log;

namespace Informa.Library.Rss.ItemRetrieval
{
	public class RssSearchResultsItemRetrieval : IRssSitecoreItemRetrieval
	{
		public IEnumerable<Item> GetSitecoreItems(Item feedItem)
		{
			ISearch_Rss_Feed searchRssFeed = feedItem.GlassCast<ISearch_Rss_Feed>(inferType: true);

			var feedUrl = GetSearchApiUrl(searchRssFeed);
			return GetItemsFromApi(feedUrl);
		}

		public string GetSearchApiUrl(ISearch_Rss_Feed rssFeedItem)
		{
			if (rssFeedItem == null)
			{
				Log.Error("Error Getting RSS Feed Item.", this);
				return string.Empty;
			}

			Item searchPageItem = Sitecore.Context.Database.GetItem(rssFeedItem.Search_Page.ToString());

			if (searchPageItem == null)
			{
				Log.Error("Error Search Page Item For RSS Feed.", this);
				return string.Empty;
			}

			ISearch_Listing searchListing = searchPageItem.GlassCast<ISearch_Listing>(inferType: true);
			
			//Build the base api URL
			var searchPageId = searchPageItem.ID.ToGuid().ToString("D").ToLower();
			var feedUrl = string.Format("{0}://{1}{2}?pId={3}", HttpContext.Current.Request.Url.Scheme, WebUtil.GetHostName(),
					searchListing.Base_Endpoint_Url, searchPageId);

			ISite_Root siteRoot = rssFeedItem.Crawl<ISite_Root>();
			if (siteRoot != null)
			{
				feedUrl = $"{feedUrl}&publication={siteRoot.Publication_Name}";
			}

			var defaultSortBy = "date";
			var defaultSortOrder = "";

			//If the default sort has not been chosen fall back to relevance ascending sorting
			if (searchListing.Default_Sort_Order == null)
			{
				//set default to date
				defaultSortBy = "date";
				defaultSortOrder = "desc";
			}
			else
			{
				defaultSortOrder = "desc";
				if (!searchListing.Default_Sort_Order.Sort_Ascending)
				{
					defaultSortOrder = "desc";
				}

				defaultSortBy = searchListing.Default_Sort_Order.Key;
			}

			feedUrl = string.Format("{0}&sortBy={1}&sortOrder={2}", feedUrl, defaultSortBy, defaultSortOrder);
			////}

			return feedUrl;
		}


		/// <summary>
		///     Retrieves Sitecore items from the search API
		/// </summary>
		/// <param name="apiUrl"></param>
		/// <returns></returns>
		private IEnumerable<Item> GetItemsFromApi(string apiUrl)
		{
			try
			{
				var results = SearchWebClientUtil.GetSearchResultsFromApi(apiUrl);

				if (results == null)
				{
					return new List<Item>();
				}

				var resultItems =
						results.results.Select(searchResult => Context.Database.GetItem(searchResult.ItemId))
								.Where(theItem => theItem != null);


				if (Context.RawUrl.ToLower().Contains("emailordering=1"))
				{
					resultItems = resultItems.OrderByDescending(r => r[IArticleConstants.Sort_OrderFieldId]).ToList();
				}

				return resultItems;
			}
			catch (Exception exc)
			{
				Log.Error("Could not retrieve items from the search service for URL: " + apiUrl + " : " + exc.Message, this);
				return new List<Item>();
			}
		}
	}
}
